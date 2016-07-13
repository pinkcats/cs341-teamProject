using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NoteShare.DataAccess;
using NoteShare.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NoteShare.Resources;
using NoteShare.Models;

namespace NoteShare.Test
{
    [TestClass]
    public class TestLoginController
    {
        Mock<HttpContextBase> contextMock = null;
        Mock<ControllerContext> controllerContextMock = null;
        Mock<UnitOfWork> moqUoW = null;
        Mock<GenericRepository<University>> unRepo = null;
        Mock<UserRepository> uRepo = null;
        Mock<PermissionRepository> pRepo = null;
        Mock<GenericRepository<ReportedNote>> rRepo = null;
        Mock<NoteRepository> nRepo = null;
        Mock<CourseRepository> cRepo = null;
        Mock<CommentsRepository> cmRepo = null;
        LoginController controller = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        [TestInitialize]
        public void Initalize()
        {
            contextMock = new Mock<HttpContextBase>();

            Mock<noteShareModel> model = new Mock<noteShareModel>();
            moqUoW = new Mock<UnitOfWork>();

            unRepo = new Mock<GenericRepository<University>>(model.Object);
            uRepo = new Mock<UserRepository>(model.Object);
            pRepo = new Mock<PermissionRepository>(model.Object);
            rRepo = new Mock<GenericRepository<ReportedNote>>(model.Object);
            nRepo = new Mock<NoteRepository>(model.Object);
            cRepo = new Mock<CourseRepository>(model.Object);
            cmRepo = new Mock<CommentsRepository>(model.Object);

            moqUoW.Setup(x => x.UniversityRepository).Returns(unRepo.Object);
            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            moqUoW.Setup(x => x.NoteRepository).Returns(nRepo.Object);
            moqUoW.Setup(x => x.PermissionRepository).Returns(pRepo.Object);
            moqUoW.Setup(x => x.ReportedNoteRepository).Returns(rRepo.Object);
            moqUoW.Setup(x => x.CourseRepository).Returns(cRepo.Object);
            moqUoW.Setup(x => x.CommentRepository).Returns(cmRepo.Object);

            controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controller = new LoginController(moqUoW.Object);
            controller.ControllerContext = controllerContextMock.Object;
        }

        [TestMethod]
        public void TestUpdateAccount()
        {
            //Arrange
            User returnUser = new User() { Id=0, Username="grayk58",PasswordHash="test", Email="grayk58" };
            UpdateAccountModel mod = new UpdateAccountModel() { universityId = 4375, email="graysideofthings@gmail.com"};
            User returnUser1 = new User() {Id=1, Username="grayk", PasswordHash="test", Email="gryak@gmail.com"};
            UpdateAccountModel mod1 = new UpdateAccountModel() { universityId = -1, email = "grayk@gmail.com" };
            var fakeIdentity = new GenericIdentity("grayk58");
            var principal = new GenericPrincipal(fakeIdentity, null);
            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result1 = controller.UpdateAccount(mod) as ViewResult;

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser1);
            var result2 = controller.UpdateAccount(mod1) as ViewResult;
            //Assert
            Assert.AreEqual("UpdateAccount", result1.ViewName);
            Assert.AreEqual("Error", result2.ViewName);
        }

        [TestMethod]
        public void TestChangePassword()
        {
            var password1 = PasswordHash.CreateHash("testing1");
            var password2 = PasswordHash.CreateHash("catsrus5");
            var password3 = PasswordHash.CreateHash("cats");
            var password4 = PasswordHash.CreateHash("testing4");
            //Arrange
            User returnUser = new User() {Id= 0, Username="grayk581", PasswordHash= password1, Email="grayk581@uwosh.edu"};
            ChangePasswordModel mod1 = new ChangePasswordModel() {oldPassword = "testing1", password ="test", passwordConfirm = "test"};
            User returnUser1 = new User() { Id = 1, Username = "grayk5", PasswordHash = password2, Email = "grayk58@uwosh.edu" };
            ChangePasswordModel mod2 = new ChangePasswordModel() { oldPassword = "catsrus5", password = "catsrus5", passwordConfirm = "catsrus5" };
            User returnUser2 = new User() { Id = 2, Username = "grayk", PasswordHash = password3, Email = "grayk58@uwosh.edu" };
            ChangePasswordModel mod3 = new ChangePasswordModel() { oldPassword = "cheese", password = "test1", passwordConfirm = "test1" };
            User returnUser3 = new User() { Id = 3, Username = "gray", PasswordHash = password4, Email = "grayk58@uwosh.edu" };
            ChangePasswordModel mod4 = new ChangePasswordModel() { oldPassword = "testing4", password = "test1", passwordConfirm = "test144" };
            var fakeIdentity = new GenericIdentity("grayk58");
            var principal = new GenericPrincipal(fakeIdentity, null);
            contextMock.Setup(ctx => ctx.User)
                  .Returns(principal);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result1 = controller.ChangePassword(mod1) as ViewResult;
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser1);
            var result2 = controller.ChangePassword(mod2) as ViewResult;
            var resultModel1 = (PasswordChangeFailModel)result2.Model;
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser2);
            var result3 = controller.ChangePassword(mod3) as ViewResult;
            var resultModel2 = (PasswordChangeFailModel)result3.Model;
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser3);
            var result4 = controller.ChangePassword(mod4) as ViewResult;
            var resultModel3 = (PasswordChangeFailModel)result4.Model;
            //Assert
            Assert.AreEqual("PasswordChangeComplete", result1.ViewName);
            Assert.AreEqual("PasswordChangeFail", result2.ViewName);
            Assert.AreEqual("Your old password and new password you have entered are the same, please enter a different password", resultModel1.reason);
            Assert.AreEqual("PasswordChangeFail", result3.ViewName);
            Assert.AreEqual("The previous password was invalid. Please try again.", resultModel2.reason);
            Assert.AreEqual("PasswordChangeFail", result4.ViewName);
            Assert.AreEqual("Your new password and confirmed passwords do not match.", resultModel3.reason);
        }

        [TestMethod]
        public void TestPasswordChangeComplete()
        {
            //Act
            var result = controller.PasswordChangeComplete() as ViewResult;
            //Assert
            Assert.AreEqual("PasswordChangeComplete", result.ViewName);
        }

        [TestMethod]
        public void TestNotAuthorized()
        {
            //Act
            var result = controller.NotAuthorized() as ViewResult;
            //Assert
            Assert.AreEqual("NotAuthorized", result.ViewName);
        }
    }
}
