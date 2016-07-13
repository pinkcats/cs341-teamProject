using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NoteShare.Models;
using NoteShare.DataAccess;
using NoteShare.Resources;
using System.Security.Principal;

namespace NoteShare.Controllers
{
    public class LoginController : Controller
    {
        private UnitOfWork database;

        public LoginController()
        {
            this.database = new UnitOfWork();
        }

        public LoginController(UnitOfWork database)
        {
            this.database = database;
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var user = database.UserRepository.GetUserByUsername(model.username);

            if (user != null)
            {
                var perm = database.PermissionRepository.GetPermissionByUserId(user.Id);
                if (user.IsSuspended == false)
                {
                    var result = PasswordHash.ValidatePassword(model.password, user.PasswordHash);

                    if (result)
                    {
                        user.FailedLoginAttempts = 0;
                        FormsAuthentication.SetAuthCookie(model.username, true);

                        HttpCookie myCookie = new HttpCookie("UserSettings");
                        myCookie.Expires = DateTime.Now.AddDays(1d);
                        myCookie.Value = (perm.PermissionLevel == 2) ? "Admin" : "User";
                        myCookie.Name = "IsAdmin";
                        Response.Cookies.Add(myCookie);
                        database.Save();
                    }
                    else
                    {
                        user.FailedLoginAttempts += 1;
                        if (user.FailedLoginAttempts >= 3)
                        {
                            user.IsSuspended = true;
                        }

                        database.Save();
                        //Replace with locked account message later
                        //got that setup but doesnt offer solution
                        return RedirectToAction("ErrorLogin", "Home");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Replace with locked account message later
                    //got that setup but doesn't offer solution
                    return RedirectToAction("ErrorLoginSuspended", "Home");
                }
                
            }
            else
            {
                return RedirectToAction("ErrorLogin", "Home");
            }
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {
            var redirectPage = "RegistrationFailed";
            RegistrationFailedModel rmodel = new RegistrationFailedModel();
            InputValidator iv = new InputValidator();
            bool inputValid = iv.validateRegistration(model.username, model.email, model.password, model.passwordConfirm);
            bool isNotActiveUser = (database.UserRepository.GetUserByUsername(model.username) == null);

            if (isNotActiveUser && inputValid)
            {
                User user = new User();
                user.Email = model.email;
                user.Username = model.username;
                user.UniversityId = model.universityId;
                user.PasswordHash = PasswordHash.CreateHash(model.password);
                database.UserRepository.Insert(user);
                database.Save();

                var userid = database.UserRepository.GetExactMatch(user).Id;

                Permission userPermission = new Permission();
                userPermission.PermissionLevel = 1;     //Set new users to default permissions (USER)
                userPermission.UserId = userid;
                database.PermissionRepository.Insert(userPermission);
                database.Save();
                
                redirectPage = "RegistrationComplete";
                return this.View(redirectPage);
            }

            if (!isNotActiveUser)
            {
                rmodel.reason = "A user with that username already exists.";
            }
            else if(!inputValid)
            {
                rmodel.reason = iv.lastFail();
            }

            return this.View(redirectPage, rmodel);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies.Remove("IsAdmin");
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult Account()
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var universities = database.UniversityRepository.GetAll();
            var list = universities.Select(x => new KeyValuePair() { key = x.Id, value = x.Name }).ToList();
            AccountModel model = new AccountModel() { universities = list, university = user.UniversityId, email = user.Email };
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UpdateAccount(UpdateAccountModel model)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            InputValidator iv = new InputValidator();
            bool inputValid = iv.validateUpdateAccount(model.email, model.universityId);

            if (inputValid)
            {
                user.UniversityId = model.universityId;
                user.Email = model.email;
                database.UserRepository.Update(user);
                database.Save();
                return this.View("UpdateAccount");
            }
            else
            {
                ErrorModel errorModel = new ErrorModel();
                errorModel.error = iv.lastFail();
                return this.View("Error", errorModel);   
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            User user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            PasswordChangeFailModel failedModel = new PasswordChangeFailModel();
            var result = PasswordHash.ValidatePassword(model.oldPassword, user.PasswordHash);
            var redirectpage = "PasswordChangeFail";
            if (!model.oldPassword.Equals(model.password) && model.password.Equals(model.passwordConfirm) && result)
            {
                user.PasswordHash = PasswordHash.CreateHash(model.password);
                database.UserRepository.Update(user);
                database.Save();
                redirectpage = "PasswordChangeComplete";
            }
            if (model.oldPassword.Equals(model.password))
            {
                failedModel.reason = "Your old password and new password you have entered are the same, please enter a different password";
            }
            else if (!result)
            {
                failedModel.reason = "The previous password was invalid. Please try again.";
            }
            else if (!model.password.Equals(model.passwordConfirm))
            {
                failedModel.reason = "Your new password and confirmed passwords do not match.";
            }
            return this.View(redirectpage, failedModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult PasswordChangeComplete()
        {
            return this.View("PasswordChangeComplete");
        }

        [HttpGet]
        public ActionResult NotAuthorized()
        {
            return this.View("NotAuthorized");
        }
    }
}