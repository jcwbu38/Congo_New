/* UserController.cs
 * This contoller provides access to the user database for update.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SQLitePCL;
using test5.Extensions;
using test5.Models;
using test5.Models.UserViewModels;
using test5.Models.ManageViewModels;
using test5.Services;

namespace test5.Controllers
{

    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private const string Issuer = "https://congo.com";

        public UserController(
            UserContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Administrators", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role, "Logistics", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role, "Sales", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role, "Tests", ClaimValueTypes.String, Issuer),
                new Claim(ClaimTypes.Role, "Customers", ClaimValueTypes.String, Issuer)
            };
            var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");


                    /*
                    var theUser = await _userManager.Users.FirstAsync(m => m.Email.Equals(model.Email));
                    if ( theUser != null )
                    {
                        await _userManager.AddClaimAsync(theUser, new Claim(ClaimTypes.Role, "Tests"));
                        await _userManager.AddClaimAsync(theUser, new Claim(ClaimTypes.Name, "Tests"));
                    }
                    */
                    //return RedirectToLocal(returnUrl);
                    return View("LoginSuccess", model);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new {returnUrl, model.RememberMe});
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SetRole(User model)
        {
            string userEmail = User.Identities.First(u => u.HasClaim(c => c.Type == ClaimTypes.Name)).FindFirst(ClaimTypes.Name).Value;
            if (userEmail != null)
            {
                var theUser = await _userManager.Users.FirstAsync(m => m.Email.Equals(userEmail));

                if (theUser != null && !String.IsNullOrEmpty(theUser.UserType))
                {
                    Claim roleClaim = null;
                    try
                    {
                        roleClaim = User.Identities.First(u => u.HasClaim(c => c.Type == ClaimTypes.Role)).FindFirst(ClaimTypes.Role);
                    }
                    catch { }
                    if (roleClaim != null)
                    {
                        if (!roleClaim.Value.Equals(theUser.UserType))
                        {
                            User.Identities.Last(u => u.HasClaim(c => c.Type == ClaimTypes.Role)).RemoveClaim(roleClaim);
                            var newClaim = new Claim(ClaimTypes.Role, theUser.UserType);
                            await _userManager.AddClaimAsync(theUser, newClaim);
                            await _signInManager.RefreshSignInAsync(theUser);
                        }

                    }
                    else
                    {
                        var newClaim = new Claim(ClaimTypes.Role, theUser.UserType);
                        await _userManager.AddClaimAsync(theUser, newClaim);
                        await _signInManager.RefreshSignInAsync(theUser);
                    }
                }
                else // Add the "Customer" role to the user.
                {
                    var newClaim = new Claim(ClaimTypes.Role, "Customers");
                    theUser.UserType = "Customers";
                    await _userManager.UpdateAsync(theUser);
                    await _userManager.AddClaimAsync(theUser, newClaim);
                    await _signInManager.RefreshSignInAsync(theUser);
                }
            }
            return RedirectToAction("Index", "Home" );
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel {RememberMe = rememberMe};
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe,
            string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result =
                await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe,
                    model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model,
            string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User {UserName = model.Email, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var userId = user.Id.ToString();
                    var callbackUrl = Url.EmailConfirmationLink(userId, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation("User created a new account with password.");

                    var theUser = await _userManager.Users.FirstAsync(u => u.Email.Equals(model.Email));

                    
                    //User.Identities.Last(u => u.HasClaim(c => c.Type == ClaimTypes.Name)).FindFirst(ClaimTypes.Name).Value;
                    /*var newClaims = new List<Claim>
                    {
                        new Claim("Name", "Customer")
                    };
                    var userType = new ClaimsIdentity( newClaims );
                    User.AddIdentity(userType);
                    */

                    return View("RegisterSuccess", model);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            ShoppingCartController.products.Clear();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "User", new {returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel {Email = email});
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model,
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new User {UserName = model.Email, Email = model.Email};
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var userId = user.Id.ToString();
                var callbackUrl = Url.ResetPasswordCallbackLink(userId, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel {Code = code};
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Administrators, Tests")]
        public async Task<IActionResult> ListUsers()
        {
            IEnumerable<User> users = await _context.Users.ToListAsync();
            if( users == null)
            {
                return Content("Error, users is null.");
            }
            return View(users);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        [AllowAnonymous]
        [Authorize(Roles = "Administrators, Tests")]
        public async Task<IActionResult> AdminDetails(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Content("Error, user email cannot be empty.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(id));
            if (user == null)
            {
                return Content("Error, user email " + id + " could not be found in the User database.");
            }

            return View(user);
        }

        [Authorize(Roles = "Administrators, Tests")]
        // GET: User/Create
        public IActionResult AdminCreate()
        {
            
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators, Tests")]
        public async Task<IActionResult> AdminCreate(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user);
                await _context.SaveChangesAsync();

                var theUser = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(user.Email));

                if (theUser != null)
                {
                    await _userManager.AddClaimAsync(theUser, new Claim(ClaimTypes.Role, user.UserType));

                    await _signInManager.RefreshSignInAsync(theUser);
                }
                return RedirectToAction(nameof(ListUsers));
            }
            return View(user);
        }

        [Authorize(Roles = "Administrators, Tests")]
        //  GET: User/Edit/5
        public async Task<IActionResult> AdminEdit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Content("Error, user email cannot be empty.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(id));
            if (user == null)
            {
                return Content("Error, user email " + id + " could not be found in the User database.");
            }

            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AdminUpdateUser(User user)
        {
            User theUser = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(user.Email));
            if (theUser != null)
            {

                IList<Claim> userClaims = null;
                try
                {
                    //roleClaim = User.Identities.First(u => u.HasClaim(c => c.Type == ClaimTypes.Role)).FindFirst(ClaimTypes.Role);
                    userClaims = await _userManager.GetClaimsAsync(theUser);
                }
                catch { }
                if (userClaims != null)
                {
                    foreach (var aClaim in userClaims)
                    {
                        if (aClaim.Type.Equals(ClaimTypes.Role) && !aClaim.Value.Equals(theUser.UserType))
                        {
                            await _userManager.RemoveClaimAsync(theUser, aClaim);
                        }
                    }
                    var newClaim = new Claim(ClaimTypes.Role, user.UserType);
                    await _userManager.AddClaimAsync(theUser, newClaim);

                }
                else
                {
                    var newClaim = new Claim(ClaimTypes.Role, user.UserType);
                    await _userManager.AddClaimAsync(theUser, newClaim);
                    await _signInManager.RefreshSignInAsync(theUser);
                }

                theUser.UserType = user.UserType;
                theUser.First = user.First;
                theUser.Last = user.Last;
                theUser.Address1 = user.Address1;
                theUser.Address2 = user.Address2;
                theUser.City = user.City;
                theUser.State = user.State;
                theUser.Zip = user.Zip;
                theUser.Phone = user.PhoneNumber;
                theUser.NameOnCard = user.NameOnCard;
                theUser.CardNumber = user.CardNumber;
                theUser.ExpDate = user.ExpDate;
                theUser.Svc = user.Svc;

                _context.Update(theUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ListUsers));

        }




        [AllowAnonymous]
        public async Task<IActionResult> Edit(IndexViewModel user)
        {
            User theUser = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(user.Email));
            if (theUser != null)
            {
                theUser.First = user.First;
                theUser.Last = user.Last;
                theUser.Address1 = user.Address1;
                theUser.Address2 = user.Address2;
                theUser.City = user.City;
                theUser.State = user.State;
                theUser.Zip = user.Zip;
                theUser.Phone = user.PhoneNumber;
                theUser.NameOnCard = user.NameOnCard;
                theUser.CardNumber = user.CardNumber;
                theUser.ExpDate = user.ExpDate;
                theUser.Svc = user.Svc;

                _context.Update(theUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Manage");

        }

        [Authorize(Roles = "Administrators, Tests")]
        // POST: User/Delete/5
        public async Task<IActionResult> AdminDelete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Content("Error, user email cannot be empty.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(id));
            if (user == null)
            {
                return Content("Error, user email " + id + " could not be found in the User database.");
            }

            return View(user);
        }

        // POST: User/Delete/5

        [Authorize(Roles = "Administrators, Tests")]
        public async Task<IActionResult> DeleteConfirmed(User id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Email.Equals(id.Email));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListUsers));
        }

        [AllowAnonymous]
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Email.Equals(id));
        }

        [Authorize(Roles = "Administrators, Tests")]
        public IActionResult changePassword()
        {
            return RedirectToAction("ResetPassword", "Manage");
        }

        #endregion
    }
}












// Old User Controller Code



    //public class UserController : Controller
    //{
    //    private readonly UserContext _context;

    //    public UserController(UserContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: User
    //    public async Task<IActionResult> Index()
    //    {
    //        return View(await _context.User.ToListAsync());
    //    }

    //    // GET: User/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _context.User
    //            .SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(user);
    //    }

    //    // GET: User/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: User/Create
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("ID,first,last,address1,address2,state,zip,email")] User user)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(user);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(user);
    //    }

    //    // GET: User/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _context.User.SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(user);
    //    }

    //    // POST: User/Edit/5
    //    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("ID,first,last,address1,address2,state,zip,email")] User user)
    //    {
    //        if (id != Int32.Parse(user.Id))
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(user);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!UserExists(Int32.Parse(user.Id)))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(user);
    //    }

    //    // GET: User/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _context.User
    //            .SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(user);
    //    }

    //    // POST: User/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var user = await _context.User.SingleOrDefaultAsync(m => Int32.Parse(m.Id) == id);
    //        _context.User.Remove(user);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Save(User user)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            var viewModel = new CreateUserViewModel
    //            {
    //                User = user
    //            };

    //            return View("Create", viewModel);
    //        }

    //        if (Int32.Parse(user.Id) == 0)
    //            _context.User.Add(user);
    //        else
    //        {
    //            var customerInDb = _context.User.Single(u => u.Id == user.Id);
    //            customerInDb.First = user.First;
    //        }

    //        _context.SaveChanges();

    //        return RedirectToAction("Create", "User");
    //    }

    //    private bool UserExists(int id)
    //    {
    //        return _context.User.Any(e => Int32.Parse(e.Id) == id);
    //    }
    //}

