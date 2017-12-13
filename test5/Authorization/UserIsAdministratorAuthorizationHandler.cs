/*   
 * Class not finished.
 *   
 * This class and other classes simliar were supposed to be 
 * used to determine if the user that is logged into the 
 * application is either a Administrator, Customer, Sales,
 * or Logistics Account to be able to determin view permissions.
 * 
*/

//using System.Threading.Tasks;
//using test5.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization.Infrastructure;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.WebSockets.Internal;

//namespace test5.Authorization
//{
//    public class UserIsAdministratorAuthorizationHandler
//        : AuthorizationHandler<OperationAuthorizationRequirement, User>
//    {
//        UserManager<User> _userManager;

//        public ContactIsOwnerAuthorizationHandler(UserManager<User>
//            userManager)
//        {
//            _userManager = userManager;
//        }

//        protected override Task
//            HandleRequirementAsync(AuthorizationHandlerContext context,
//                OperationAuthorizationRequirement requirement,
//                User resource)
//        {
//            if (context.User == null || resource == null)
//            {
//                return Task.FromResult(0);
//            }

//            // If we're not asking for CRUD permission, return.

//            if (requirement.Name != Constants.CreateOperationName &&
//                requirement.Name != Constants.ReadOperationName &&
//                requirement.Name != Constants.UpdateOperationName &&
//                requirement.Name != Constants.DeleteOperationName)
//            {
//                return Task.FromResult(0);
//            }

//            if (resource.OwnerID == _userManager.GetUserId(context.User))
//            {
//                context.Succeed(requirement);
//            }

//            return Task.FromResult(0);
//        }
//    }
//}
