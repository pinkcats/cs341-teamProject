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
    public class TestAdminController
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
        AdminController controller = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        [TestInitialize]
        public void Initialize()
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

            controller = new AdminController(moqUoW.Object);
            controller.ControllerContext = controllerContextMock.Object;
        }

        [TestMethod]
        public void TestReportedNotes()
        {
          
        }

        [TestMethod]
        public void TestRecent()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 1, UserId = returnUser.Id };
            List<Note> returnNotes = new List<Note>();
            returnNotes.Add(new Note() { Title = "Test1", Id = 1, IsPrivate = false, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test2", Id = 2, IsPrivate = false, CourseId = 1, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test3", Id = 3, IsPrivate = false, CourseId = 2, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test4", Id = 4, IsPrivate = false, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test5", Id = 5, IsPrivate = false, CourseId = 1, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test6", Id = 6, IsPrivate = false, CourseId = 2, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test7", Id = 7, IsPrivate = true, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test8", Id = 8, IsPrivate = true, CourseId = 1, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test9", Id = 9, IsPrivate = true, CourseId = 2, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test10", Id = 10, IsPrivate = true, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test11", Id = 11, IsPrivate = true, CourseId = 1, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "Test12", Id = 12, IsPrivate = true, CourseId = 2, UserId = returnUser.Id });

            List<Course> returnCourses = new List<Course>();
            returnCourses.Add(new Course() { Id = 0, Name = "Course1", Department = "Comp Sci" });
            returnCourses.Add(new Course() { Id = 1, Name = "Course2", Department = "English" });
            returnCourses.Add(new Course() { Id = 2, Name = "Course3", Department = "History" });

            List<Comment> returnComments = new List<Comment>();
            returnComments.Add(new Comment() { Id = 1, Message = "Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!", NoteId = 1, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 2, Message = "Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!", NoteId = 2, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 3, Message = "Thiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100char", NoteId = 3, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 4, Message = "Thiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100char", NoteId = 4, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 5, Message = "Comment message 5", NoteId = 5, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 6, Message = "Comment message 6", NoteId = 6, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 7, Message = "Comment message 7", NoteId = 7, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 8, Message = "Comment message 8", NoteId = 8, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 9, Message = "Comment message 9", NoteId = 9, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 10, Message = "Comment message 10", NoteId = 10, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 11, Message = "Comment message 11", NoteId = 11, UserId = returnUser.Id });
            returnComments.Add(new Comment() { Id = 12, Message = "Comment message 12", NoteId = 12, UserId = returnUser.Id });

            var fakeIdentity = new GenericIdentity("gottsj36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            nRepo.Setup(x => x.GetAll()).Returns(returnNotes);
            cRepo.Setup(x => x.GetAll()).Returns(returnCourses);
            cmRepo.Setup(x => x.GetAll()).Returns(returnComments);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.Recent() as ViewResult;
            var resultModel = (RecentModel)result.Model;

            //Assert
            Assert.AreEqual("Recent", result.ViewName);
            Assert.IsTrue(resultModel.noteList[0].Id == 1);
            Assert.IsTrue(resultModel.noteList[1].Id == 2);
            Assert.IsTrue(resultModel.noteList[2].Id == 3);
            Assert.IsTrue(resultModel.noteList[3].Id == 4);
            Assert.IsTrue(resultModel.noteList[4].Id == 5);
            Assert.IsTrue(resultModel.noteList[5].Id == 6);
            Assert.IsTrue(resultModel.noteList[6].Id == 7);
            Assert.IsTrue(resultModel.noteList[7].Id == 8);
            Assert.IsTrue(resultModel.noteList[8].Id == 9);
            Assert.IsTrue(resultModel.noteList[9].Id == 10);
            Assert.IsTrue(resultModel.noteList[10].Id == 11);
            Assert.IsTrue(resultModel.noteList[11].Id == 12);
            Assert.IsTrue(resultModel.commentList[0].Id == 1);            
            Assert.IsTrue(resultModel.commentList[1].Id == 2);            
            Assert.IsTrue(resultModel.commentList[2].Id == 3);            
            Assert.IsTrue(resultModel.commentList[3].Id == 4);            
            Assert.IsTrue(resultModel.commentList[4].Id == 5);
            Assert.IsTrue(resultModel.commentList[5].Id == 6);
            Assert.IsTrue(resultModel.commentList[6].Id == 7);
            Assert.IsTrue(resultModel.commentList[7].Id == 8);
            Assert.IsTrue(resultModel.commentList[8].Id == 9);
            Assert.IsTrue(resultModel.commentList[9].Id == 10);
            Assert.IsTrue(resultModel.commentList[10].Id == 11);
            Assert.IsTrue(resultModel.commentList[11].Id == 12);
            Assert.IsTrue(resultModel.commentList[0].Message.Length <= 100);
            Assert.IsTrue(resultModel.commentList[1].Message.Length <= 100);
            Assert.IsTrue(resultModel.commentList[2].Message.Length <= 100);
            Assert.IsTrue(resultModel.commentList[3].Message.Length <= 100);

        }

        [TestMethod]
        public void TestDashboard()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 1, UserId = returnUser.Id };
            List<Note> returnNotes = new List<Note>();
            returnNotes.Add(new Note() { Title = "Test1", Id = 1, IsPrivate = false, CourseId = 0, UserId = 1 });
            returnNotes.Add(new Note() { Title = "Test2", Id = 2, IsPrivate = false, CourseId = 1, UserId = 1 });
            returnNotes.Add(new Note() { Title = "Test3", Id = 3, IsPrivate = false, CourseId = 2, UserId = 2 });
            returnNotes.Add(new Note() { Title = "Test4", Id = 4, IsPrivate = false, CourseId = 0, UserId = 2 });
            returnNotes.Add(new Note() { Title = "Test5", Id = 5, IsPrivate = false, CourseId = 1, UserId = 3 });
            returnNotes.Add(new Note() { Title = "Test6", Id = 6, IsPrivate = false, CourseId = 2, UserId = 3 });
            List<User> returnUsers = new List<User>();
            returnUsers.Add(new User() { Id = 1, Username = "guy1", PasswordHash = "test", Email = "some1@email.com" });
            returnUsers.Add(new User() { Id = 2, Username = "guy2", PasswordHash = "test", Email = "some2@email.com" });
            returnUsers.Add(new User() { Id = 3, Username = "guy3", PasswordHash = "test", Email = "some3@email.com" });
            List<Comment> returnComments = new List<Comment>();
            returnComments.Add(new Comment() { Id = 1, Message = "Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!", NoteId = 1, UserId = returnUser.Id,Rating = 5 });
            returnComments.Add(new Comment() { Id = 2, Message = "Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!Thiscommentismorethan100chars!", NoteId = 2, UserId = returnUser.Id, Rating = 4 });
            returnComments.Add(new Comment() { Id = 3, Message = "Thiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100char", NoteId = 3, UserId = returnUser.Id, Rating = 4 });
            returnComments.Add(new Comment() { Id = 4, Message = "Thiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100charThiscommentis100char", NoteId = 4, UserId = returnUser.Id, Rating = 4 });

            var fakeIdentity = new GenericIdentity("gottsj36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetAll()).Returns(returnUsers);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            nRepo.Setup(x => x.GetAll()).Returns(returnNotes);
            cmRepo.Setup(x => x.GetAll()).Returns(returnComments);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.DashBoard() as ViewResult;
            var resultModel = (AdminNotesModel)result.Model;

            //Assert
            Assert.AreEqual("DashBoard", result.ViewName);
            Assert.AreEqual(1, resultModel.userNotes[0].user.Id);
            Assert.AreEqual(1, resultModel.userNotes[0].notes[0].Id);
            Assert.AreEqual(2, resultModel.userNotes[0].notes[1].Id);
            Assert.AreEqual(4.5, resultModel.userNotes[0].rating);

            Assert.AreEqual(2, resultModel.userNotes[1].user.Id);
            Assert.AreEqual(3, resultModel.userNotes[1].notes[0].Id);
            Assert.AreEqual(4, resultModel.userNotes[1].notes[1].Id);
           // Assert.AreEqual(4.5, resultModel.userNotes[1].rating);

            Assert.AreEqual(3, resultModel.userNotes[2].user.Id);
            Assert.AreEqual(5, resultModel.userNotes[2].notes[0].Id);
            Assert.AreEqual(6, resultModel.userNotes[2].notes[1].Id);
           // Assert.AreEqual(4.5, resultModel.userNotes[2].rating);

        }

        [TestMethod]
        public void TestUnlockUser()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 2, UserId = returnUser.Id };
            User suspendUser = new User() { Id = 1, Username = "someuser", PasswordHash = "test", Email = "gooduser@something.com", IsSuspended = true, FailedLoginAttempts = 3 };

            var fakeIdentity = new GenericIdentity("gottjs36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            uRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(suspendUser);

            int expected = 0;
            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.UnlockUser(1);

            //Assert
            Assert.IsFalse(suspendUser.IsSuspended);
            Assert.AreEqual(expected, suspendUser.FailedLoginAttempts);
        }

        [TestMethod]
        public void TestSuspendUser()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 2, UserId = returnUser.Id };
            User suspendUser = new User() { Id = 1, Username = "someuser", PasswordHash = "test", Email = "gooduser@something.com", IsSuspended = false };

            var fakeIdentity = new GenericIdentity("gottjs36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            uRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(suspendUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.SuspendUser(1);

            //Assert
            Assert.IsTrue(suspendUser.IsSuspended);
        }

        [TestMethod]
        public void TestUnsuspendUser()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 2, UserId = returnUser.Id };
            User suspendUser = new User() { Id = 1, Username = "someuser", PasswordHash = "test", Email = "gooduser@something.com", IsSuspended = true };

            var fakeIdentity = new GenericIdentity("gottjs36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            uRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(suspendUser);
            
            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.UnsuspendUser(1);

            //Assert
            Assert.IsFalse(suspendUser.IsSuspended);
        }

        [TestMethod]
        public void TestSuspendNote()
        {
            //Arrange 
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 2, UserId = returnUser.Id };
            Note note = new Note() { Id = 0, IsSuspended = false };

            var fakeIdentity = new GenericIdentity("gottjs36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            moqUoW.Setup(x => x.NoteRepository).Returns(nRepo.Object);
            nRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(note);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.SuspendNote(0);

            //Assert
            Assert.IsTrue(note.IsSuspended);
        }

        [TestMethod]
        public void UnsuspendNote()
        {
            //Arrange 
            User returnUser = new User() { Id = 0, Username = "gottsj36", PasswordHash = "test", Email = "gottsj36@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 2, UserId = returnUser.Id };
            Note note = new Note() { Id = 0, IsSuspended = true };

            var fakeIdentity = new GenericIdentity("gottsj36");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            moqUoW.Setup(x => x.NoteRepository).Returns(nRepo.Object);
            nRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(note);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            
            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.UnsuspendNote(0);

            //Assert
            Assert.IsFalse(note.IsSuspended);
        }

    }
}
