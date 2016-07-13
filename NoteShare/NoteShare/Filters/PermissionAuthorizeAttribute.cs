using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteShare.Resources;
using NoteShare.DataAccess;
using System.Web.Routing;

namespace NoteShare.Filters
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {
        public string redirectAction { get; set;}
        public PermissionEnum permission { get; set; }

        public PermissionAuthorizeAttribute(PermissionEnum value)
        {
            permission = value;
            redirectAction = "NotAuthorized";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UnitOfWork database = new UnitOfWork();
            var user = database.UserRepository.GetUserByUsername(HttpContext.Current.User.Identity.Name);
            if (user == null && this.permission.CompareTo(PermissionEnum.VIEWER) > 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "NotAuthorized" } });
            }
            else
            {
                var userPermission = database.PermissionRepository.GetPermissionByUserId(user.Id);
                PermissionEnum permValue = Permissions.GetPermissionFromValue(userPermission.PermissionLevel);

                if (permValue.CompareTo(this.permission) < 0)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Login" }, { "action", "NotAuthorized" } });
                }
            }
        }
    }
}