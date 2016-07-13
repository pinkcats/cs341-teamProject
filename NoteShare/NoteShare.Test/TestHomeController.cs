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
using System.IO;
using System.Net.Mail;
using NoteShare.Resources;
using NoteShare.Models;

namespace NoteShare.Test
{
    [TestClass]
    public class TestHomeController
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
        Mock<CommentsRepository> coRepo = null;
        HomeController controller = null;
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
            coRepo = new Mock<CommentsRepository>(model.Object);

            moqUoW.Setup(x => x.UniversityRepository).Returns(unRepo.Object);
            moqUoW.Setup(x => x.UserRepository).Returns(uRepo.Object);
            moqUoW.Setup(x => x.NoteRepository).Returns(nRepo.Object);
            moqUoW.Setup(x => x.PermissionRepository).Returns(pRepo.Object);
            moqUoW.Setup(x => x.ReportedNoteRepository).Returns(rRepo.Object);
            moqUoW.Setup(x => x.CourseRepository).Returns(cRepo.Object);
            moqUoW.Setup(x => x.CommentRepository).Returns(coRepo.Object);

            controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Setup(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            controller = new HomeController(moqUoW.Object);
            controller.ControllerContext = controllerContextMock.Object;
        }

        [TestMethod]
        public void TestIndex()
        {
            //Act
            var result = controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestSearch_GET()
        {
            //Arrange
            List<University> universities = new List<University>();
            universities.Add(new University()
            {
                Id = 0,
                Address = "ABC",
                AdmissionURL = "test.com",
                Chancellor = "Derrick H",
                City = "Oshkosh",
                FinancialAidUrl = "fintest.com",
                Name = "UWO",
                State = "WI",
                Zip = "54901",
                URL = "url.com",
                Telephone = "4444444444",
                Latitude = 90,
                Longitude = 90
            });

            universities.Add(new University()
            {
                Id = 1,
                Address = "CDE",
                AdmissionURL = "hello.com",
                Chancellor = "Kiley G",
                City = "Poopy town",
                FinancialAidUrl = "test.com",
                Name = "UWP",
                State = "WI",
                Zip = "53908",
                URL = "poopytown.com",
                Telephone = "5555555555",
                Latitude = 80,
                Longitude = 10
            });

            unRepo.Setup(x => x.GetAll()).Returns(universities);

            //Act
            var result = controller.Search() as ViewResult;
            UniversitySearchModel resultModel = (UniversitySearchModel)result.Model;

            //Assert
            Assert.AreEqual("Search", result.ViewName);
            Assert.AreEqual(2, resultModel.universities.Count);
            Assert.AreEqual(0, resultModel.universities[0].key);
            Assert.AreEqual("UWO", resultModel.universities[0].value);
            Assert.AreEqual(1, resultModel.universities[1].key);
            Assert.AreEqual("UWP", resultModel.universities[1].value);
        }

        [TestMethod]
        public void TestSearch_POST()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 1, UserId = returnUser.Id };
            SearchModel mod = new SearchModel() { professor = "GT", description = "test" };

            DateTime date = DateTime.Now;
            List<Note> notes = new List<Note>();
            notes.Add(new Note() { Id = 0, CourseId = 0, Title = "Title1", DateAdded = date, Description = "note", IsPrivate = false });
            notes.Add(new Note() { Id = 1, CourseId = 1, Title = "Title2", DateAdded = date, Description = "Test note", IsPrivate = false });
            notes.Add(new Note() { Id = 2, CourseId = 1, Title = "Title3", DateAdded = date, Description = "Hello note", IsPrivate = false });
            notes.Add(new Note() { Id = 3, CourseId = 1, Title = "Title4", DateAdded = date, Description = "NotestNote", IsPrivate = false });
            notes.Add(new Note() { Id = 4, CourseId = 1, Title = "Title5", DateAdded = date, Description = "test note", IsPrivate = false });
            notes.Add(new Note() { Id = 5, CourseId = 2, Title = "Title6", DateAdded = date, Description = "test", IsPrivate = false });

            List<Course> courses = new List<Course>();
            courses.Add(new Course() { Id = 0, ProfessorName = "Naps" });
            courses.Add(new Course() { Id = 1, ProfessorName = "GT" });
            courses.Add(new Course() { Id = 2, ProfessorName = "Summers" });

            List<Comment> comments1= new List<Comment>();
            comments1.Add(new Comment() { Id = 0, Rating = 1, Message = "Good note", UserId = 5, NoteId = 1 });
            comments1.Add(new Comment() { Id = 1, Rating = 5, Message = "Bad", UserId = 5, NoteId = 1 });
            comments1.Add(new Comment() { Id = 2, Rating = 3, Message = "Test note", UserId = 5, NoteId = 1 });

            List<Comment> comments3 = new List<Comment>();
            comments3.Add(new Comment() { Id = 3, Rating = 0, Message = "Good note", UserId = 5, NoteId = 3 });
            comments3.Add(new Comment() { Id = 4, Rating = 0, Message = "Bad", UserId = 5, NoteId = 3 });
            comments3.Add(new Comment() { Id = 5, Rating = 0, Message = "Test note", UserId = 5, NoteId = 3 });

            List<Comment> comments4 = new List<Comment>();

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            pRepo.Setup(x => x.GetPermissionByUserId(It.IsAny<int>())).Returns(perm);
            cRepo.Setup(x => x.GetAll()).Returns(courses);
            nRepo.Setup(x => x.GetPublicNotes()).Returns(notes);
            coRepo.Setup(x => x.GetAllByNoteId(1)).Returns(comments1);
            coRepo.Setup(x => x.GetAllByNoteId(3)).Returns(comments3);
            coRepo.Setup(x => x.GetAllByNoteId(4)).Returns(comments4);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.Search(mod) as JsonResult;
            var data = result.Data;
            List<JsonNotes> resultItems = serializer.Deserialize<List<JsonNotes>>(serializer.Serialize(data));

            //Assert
            Assert.AreEqual(3, resultItems.Count);

            Assert.AreEqual("Title2", resultItems[0].name);
            Assert.AreEqual(1, resultItems[0].noteId);
            Assert.AreEqual(3, resultItems[0].rating);

            Assert.AreEqual("Title4", resultItems[1].name);
            Assert.AreEqual(3, resultItems[1].noteId);
            Assert.AreEqual(0, resultItems[1].rating);

            Assert.AreEqual("Title5", resultItems[2].name);
            Assert.AreEqual(4, resultItems[2].noteId);
            Assert.AreEqual(-1, resultItems[2].rating);
        }

        [TestMethod]
        public void TestSearchBar()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 1, UserId = returnUser.Id };
            List<Note> returnNotes = new List<Note>();
            returnNotes.Add(new Note() { Title = "Test1", Id = 1, IsPrivate = false });
            returnNotes.Add(new Note() { Title = "TestMethod2", Id = 2, IsPrivate = false });
            returnNotes.Add(new Note() { Title = "TestNote3", Id = 3, IsPrivate = false });
            returnNotes.Add(new Note() { Title = "TestNote4", Id = 4, IsPrivate = false });
            returnNotes.Add(new Note() { Title = "Test5", Id = 5, IsPrivate = false });
            returnNotes.Add(new Note() { Title = "TestNote6", Id = 6, IsPrivate = true });

            var result = returnNotes.Where(x => x.Title.ToLower().Contains("note") && !x.IsPrivate).ToList();
            var result1 = returnNotes.Where(x => x.Title.ToLower().Contains("note")).ToList();

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            nRepo.Setup(x => x.GetSearchNotes(It.IsAny<string>(), It.IsAny<int>(), false)).Returns(result);
            nRepo.Setup(x => x.GetSearchNotes(It.IsAny<string>(), It.IsAny<int>(), true)).Returns(result1);
            pRepo.Setup(x => x.GetPermissionByUserId(It.IsAny<int>())).Returns(perm);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var searchBarResult = controller.SearchBar("note");
            var data = searchBarResult.Data;

            List<KeyValuePair> resultItems = serializer.Deserialize<List<KeyValuePair>>(serializer.Serialize(data));

            //Act
            perm.PermissionLevel = 2;
            controller.ControllerContext = controllerContextMock.Object;
            var searchBarResult2 = controller.SearchBar("note");
            var data2 = searchBarResult2.Data;

            List<KeyValuePair> resultItems2 = serializer.Deserialize<List<KeyValuePair>>(serializer.Serialize(data2));

            //Assert
            Assert.AreEqual(2, resultItems.Count);
            Assert.AreEqual(resultItems[0].key, 3);
            Assert.AreEqual(resultItems[0].value, "TestNote3");
            Assert.AreEqual(resultItems[1].key, 4);
            Assert.AreEqual(resultItems[1].value, "TestNote4");

            Assert.AreEqual(3, resultItems2.Count);
            Assert.AreEqual(resultItems2[0].key, 3);
            Assert.AreEqual(resultItems2[0].value, "TestNote3");
            Assert.AreEqual(resultItems2[1].key, 4);
            Assert.AreEqual(resultItems2[1].value, "TestNote4");
            Assert.AreEqual(resultItems2[2].key, 6);
            Assert.AreEqual(resultItems2[2].value, "TestNote6");
        }

        [TestMethod]
        public void TestMakePublic()
        {
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Note note = new Note() { Id = 0, IsPrivate = true };

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            nRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(note);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            controller.ControllerContext = controllerContextMock.Object;
            controller.MakePublic(0);

            Assert.IsFalse(note.IsPrivate);
        }

        [TestMethod]
        public void TestMakePrivate()
        {
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Note note = new Note() { Id = 0, IsPrivate = false };

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            nRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(note);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            controller.ControllerContext = controllerContextMock.Object;
            controller.MakePrivate(0);

            Assert.IsTrue(note.IsPrivate);
        }

        [TestMethod]
        public void TestNotes()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Permission perm = new Permission() { Id = 0, PermissionLevel = 1, UserId = returnUser.Id };
            List<Note> returnNotes = new List<Note>();
            returnNotes.Add(new Note() { Title = "Test1", Id = 1, IsPrivate = false, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "TestMethod2", Id = 2, IsPrivate = false, CourseId = 1, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "TestNote3", Id = 3, IsPrivate = true, CourseId = 0, UserId = returnUser.Id });
            returnNotes.Add(new Note() { Title = "TestNote4", Id = 4, IsPrivate = false, CourseId = 1, UserId = 5 });
            returnNotes.Add(new Note() { Title = "Test5", Id = 5, IsPrivate = false, CourseId = 2, UserId = 5 });
            returnNotes.Add(new Note() { Title = "TestNote6", Id = 6, IsPrivate = true, CourseId = 2, UserId = 5 });

            List<Course> returnCourses = new List<Course>();
            returnCourses.Add(new Course() { Id = 0, Name = "Course1", Department = "Comp Sci" });
            returnCourses.Add(new Course() { Id = 1, Name = "Course2", Department = "English" });
            returnCourses.Add(new Course() { Id = 2, Name = "Course3", Department = "History" });

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            nRepo.Setup(x => x.GetAll()).Returns(returnNotes);
            cRepo.Setup(x => x.GetAll()).Returns(returnCourses);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.Notes() as ViewResult;
            var resultModel = (UserNotesModel)result.Model;

            //Assert
            Assert.AreEqual("Notes", result.ViewName);
            Assert.AreEqual(3, resultModel.courseNotes.Count());
            Assert.AreEqual(1, resultModel.courseNotes[0].note.Id);
            Assert.AreEqual("Test1", resultModel.courseNotes[0].note.Title);
            Assert.AreEqual("Course1", resultModel.courseNotes[0].course.Name);

            Assert.AreEqual(2, resultModel.courseNotes[1].note.Id);
            Assert.AreEqual("TestMethod2", resultModel.courseNotes[1].note.Title);
            Assert.AreEqual("Course2", resultModel.courseNotes[1].course.Name);

            Assert.AreEqual(3, resultModel.courseNotes[2].note.Id);
            Assert.AreEqual("TestNote3", resultModel.courseNotes[2].note.Title);
            Assert.AreEqual("Course1", resultModel.courseNotes[2].course.Name);
        }

        [TestMethod]
        public void TestDeleteNote()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu" };
            Note returnNotePass = new Note() { Title = "Test1", Id = 1, IsPrivate = false, CourseId = 0, UserId = returnUser.Id };
            Note returnNoteFail = new Note() { Title = "Test1", Id = 2, IsPrivate = false, CourseId = 0, UserId = 50 };
            Permission perm = new Permission() {Id = 0, PermissionLevel = 1, UserId = returnUser.Id};

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            pRepo.Setup(x => x.GetPermissionByUserId(It.IsAny<int>())).Returns(perm);
            nRepo.Setup(x => x.GetByID(1)).Returns(returnNotePass);
            
            
            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.DeleteNote(1);
            nRepo.Setup(x => x.GetByID(2)).Returns(returnNoteFail);
            controller.DeleteNote(2);

            //Assert
            nRepo.Verify(x => x.Delete(1), Times.Once());
            nRepo.Verify(x => x.Delete(2), Times.Never());
        }

        [TestMethod]
        public void TestUpload_GET()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            List<University> universities = new List<University>();
            universities.Add(new University()
            {
                Id = 0,
                Address = "ABC",
                AdmissionURL = "test.com",
                Chancellor = "Derrick H",
                City = "Oshkosh",
                FinancialAidUrl = "fintest.com",
                Name = "UWO",
                State = "WI",
                Zip = "54901",
                URL = "url.com",
                Telephone = "4444444444",
                Latitude = 90,
                Longitude = 90
            });

            universities.Add(new University()
            {
                Id = 1,
                Address = "CDE",
                AdmissionURL = "hello.com",
                Chancellor = "Kiley G",
                City = "Poopy town",
                FinancialAidUrl = "test.com",
                Name = "UWP",
                State = "WI",
                Zip = "53908",
                URL = "poopytown.com",
                Telephone = "5555555555",
                Latitude = 80,
                Longitude = 10
            });

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            unRepo.Setup(x => x.GetByID(It.IsAny<int>())).Returns(universities[0]);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.Upload() as ViewResult;
            UploadModel resultModel = (UploadModel)result.Model;

            //Assert
            Assert.AreEqual("Upload", result.ViewName);
            Assert.AreEqual("UWO", resultModel.universityName);
            Assert.AreEqual(0, resultModel.university);
        }

        [TestMethod]
        public void TestUpload_POST()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            Course returnCourse = new Course()
            {
                Name = "Software Engineering",
                ProfessorName = "GT",
                Department = "Computer Science",
                UniversityId = 4375,
                Year = 2016,
                Semester = "FALL"
            };

            Note returnNote = new Note()
            {
                DateAdded = DateTime.Now,
                FileContents = new byte[1],
                FileType = "text/plain",
                IsPrivate = false,
                Title = "New Note",
                UserId = returnUser.Id,
                CourseId = returnCourse.Id,
                Description = "New note information"
            };

            Mock<HttpPostedFileBase> moqFile = new Mock<HttpPostedFileBase>();
            moqFile.Setup(x => x.ContentType).Returns("text/plain");
            moqFile.Setup(x => x.InputStream).Returns(new MemoryStream());

            Stack<Course> returnCourses = new Stack<Course>();
            returnCourses.Push(returnCourse);
            returnCourses.Push(null);
            UploadModel uploadModel = new UploadModel();
            uploadModel.courseName = "Software Engineering";
            uploadModel.courseNumber = "123";
            uploadModel.file = moqFile.Object;
            uploadModel.noteText = "Description of note";
            uploadModel.professor = "GT";
            uploadModel.semester = "Fall";
            uploadModel.title = "New Notes";
            uploadModel.topic = "Computer Science";
            uploadModel.university = 4375;
            uploadModel.universityName = "University Of Wisconsin - Oshkosh";
            uploadModel.year = 2016;

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            cRepo.Setup(x => x.GetExactMatch(It.IsAny<Course>())).Returns(returnCourses.Pop);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.Upload(uploadModel) as ViewResult;

            //Assert
            cRepo.Verify(x => x.Insert(It.IsAny<Course>()), Times.Once());
            nRepo.Verify(x => x.Insert(It.Is<Note>(y => y.Title == "New Notes")), Times.Once());
            Assert.AreEqual("UploadSuccess", result.ViewName);
        }

        [TestMethod]
        public void TestDocument()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            Permission returnPermission = new Permission() { Id = 0, PermissionLevel = 1, UserId = 0 };
            Note returnNote = new Note() { Id = 0, IsPrivate = false, UserId = 1, FileContents = new byte[1], FileType = "application/pdf", Title = "Test PDF", Description = "Test note pdf", CourseId = 0, DateAdded = DateTime.Now };

            User commentUser1 = new User() { Id = 1, Username = "testUser" };
            User commentUser2 = new User() { Id = 2, Username = "u2" };

            List<Comment> comments = new List<Comment>();
            comments.Add(new Comment() { Id = 0, Message = "Test", NoteId = 0, UserId = commentUser1.Id });
            comments.Add(new Comment() { Id = 1, Message = "Tester", NoteId = 0, UserId = commentUser1.Id });
            comments.Add(new Comment() { Id = 2, Message = "Test Class", NoteId = 0, UserId = commentUser2.Id });

            List<User> users = new List<User>() { returnUser, commentUser1, commentUser2 };

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            pRepo.Setup(x => x.GetPermissionByUserId(0)).Returns(returnPermission);
            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            uRepo.Setup(x => x.GetAll()).Returns(users);
            coRepo.Setup(x => x.GetAllByNoteId(0)).Returns(comments);
            nRepo.Setup(x => x.GetByID(0)).Returns(returnNote);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result1 = controller.Document(0) as ViewResult;
            var mod = result1.Model as ViewDocumentModel;

            returnNote.IsPrivate = true;
            var result2 = controller.Document(0) as ViewResult;

            returnPermission.PermissionLevel = 2;
            nRepo.Setup(x => x.GetByID(0)).Returns<Note>(null);
            var result3 = controller.Document(0) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Document", result1.ViewName);
            Assert.AreEqual("Test PDF", mod.title);
            Assert.AreEqual(FileTypeEnum.PDF, mod.fileType);
            Assert.AreEqual(0, mod.documentId);

            Assert.AreEqual("Test", mod.comments[0].comment.Message);
            Assert.AreEqual("testUser", mod.comments[0].user);

            Assert.AreEqual("Tester", mod.comments[1].comment.Message);
            Assert.AreEqual("testUser", mod.comments[1].user);

            Assert.AreEqual("Test Class", mod.comments[2].comment.Message);
            Assert.AreEqual("u2", mod.comments[2].user);

            Assert.AreEqual("NotAuthorized", result2.ViewName);

            Assert.AreEqual("Index", (string)result3.RouteValues["action"]);
            Assert.AreEqual("Home", (string)result3.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestReportNote()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            ReportNoteModel reportedNoteModel = new ReportNoteModel() { noteId = 0, text = "test" };
            Note returnNote = new Note() { Title = "Test1", Id = 0, IsPrivate = false, CourseId = 0, UserId = returnUser.Id };

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.ReportNote(reportedNoteModel);

            //Assert
            rRepo.Verify(x => x.Insert(It.IsAny<ReportedNote>()), Times.Once());
        }

        [TestMethod]
        public void TestDownload()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            Note returnNote = new Note() { Id = 0, IsPrivate = false, FileType = "application/pdf", UserId = 1, FileContents = new byte[1] };
            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            var mockResponse = new Mock<HttpResponseBase>();

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);
            contextMock.Setup(x => x.Response)
                       .Returns(mockResponse.Object);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);
            nRepo.Setup(x => x.GetByID(0)).Returns(returnNote);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result1 = controller.Download(0);

            returnNote.IsPrivate = true;
            var result2 = controller.Download(0);

            //Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual("application/pdf", result1.ContentType);

            Assert.IsNull(result2);
        }

        [TestMethod]
        public void TestRegister()
        {
            //Arrange
            List<University> universities = new List<University>();
            universities.Add(new University()
            {
                Id = 0,
                Address = "ABC",
                AdmissionURL = "test.com",
                Chancellor = "Derrick H",
                City = "Oshkosh",
                FinancialAidUrl = "fintest.com",
                Name = "UWO",
                State = "WI",
                Zip = "54901",
                URL = "url.com",
                Telephone = "4444444444",
                Latitude = 90,
                Longitude = 90
            });

            universities.Add(new University()
            {
                Id = 1,
                Address = "CDE",
                AdmissionURL = "hello.com",
                Chancellor = "Kiley G",
                City = "Poopy town",
                FinancialAidUrl = "test.com",
                Name = "UWP",
                State = "WI",
                Zip = "53908",
                URL = "poopytown.com",
                Telephone = "5555555555",
                Latitude = 80,
                Longitude = 10
            });

            unRepo.Setup(x => x.GetAll()).Returns(universities);

            //Act
            var result = controller.Register() as ViewResult;
            var mod = (RegistrationModel)result.Model;

            //Assert
            Assert.AreEqual("UWO", mod.universityNameList[0].value);
            Assert.AreEqual(0, mod.universityNameList[0].key);

            Assert.AreEqual("UWP", mod.universityNameList[1].value);
            Assert.AreEqual(1, mod.universityNameList[1].key);
        }

        [TestMethod]
        public void TestForgotPassword_Get()
        {
            //Arrange
            controllerContextMock.Setup(con => con.HttpContext)
                                 .Returns(contextMock.Object);
            //Act
            var result = controller.ForgotPassword() as ViewResult;

            //Assert
            Assert.AreEqual("ForgotPassword", result.ViewName);
        }

        [TestMethod]
        public void TestForgotPassword_Post()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            ForgotPasswordModel mod = new ForgotPasswordModel() { email = "heined50@uwosh.edu", username = "heined50" };


            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            var result = controller.ForgotPassword(mod) as ViewResult;

            //Assert
            Assert.AreEqual("PasswordResetComplete", result.ViewName);
            uRepo.Verify(x => x.Update(It.IsAny<User>()), Times.Once());
        }

        [TestMethod]
        public void TestAddComment()
        {
            //Arrange
            User returnUser = new User() { Id = 0, Username = "heined50", PasswordHash = "test", Email = "heined50@uwosh.edu", UniversityId = 0 };
            AddCommentModel commentModel = new AddCommentModel()
            {
                message = "Test note up in here!",
                noteId = 0,
                rating = 4
            };

            var fakeIdentity = new GenericIdentity("heined50");
            var principal = new GenericPrincipal(fakeIdentity, null);

            contextMock.Setup(ctx => ctx.User)
                       .Returns(principal);

            uRepo.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(returnUser);

            //Act
            controller.ControllerContext = controllerContextMock.Object;
            controller.AddComment(commentModel);

            //Assert
            coRepo.Verify(x => x.Insert(It.IsAny<Comment>()), Times.Once());
        }


        [TestMethod]
        public void TestErrorLogin()
        {
            //Act
            var result = controller.ErrorLogin() as ViewResult;
            var mod = (ErrorModel)result.Model;

            //Assert
            Assert.AreEqual("Error", result.ViewName);
            Assert.AreEqual("Invalid username/password combination.", mod.error);
        }

        [TestMethod]
        public void TestErrorLoginSuspended()
        {
            //Act
            var result = controller.ErrorLoginSuspended() as ViewResult;
            var mod = (ErrorModel)result.Model;

            //Assert
            Assert.AreEqual("Error", result.ViewName);
            Assert.AreEqual("Account has been suspended.", mod.error);
        }

        [TestMethod]
        public void TestUniversity()
        {
            //Arrange
            University uni = new University() { Id = 0, Name = "University Test" };

            unRepo.Setup(x => x.GetByID(0)).Returns(uni);

            //Act
            var result = controller.University(0) as ViewResult;
            var mod = (UniversityModel)result.Model;

            //Assert
            Assert.AreEqual("University", result.ViewName);
            Assert.AreEqual(0, mod.university.Id);
            Assert.AreEqual("University Test", mod.university.Name);
        }

        [TestMethod]
        public void TestErrorUpload()
        {
            //Arrange
            string error = "error test";

            //Act
            var result = controller.ErrorUpload(error) as ViewResult;
            var mod = (ErrorModel)result.Model;

            //Assert
            Assert.AreEqual("Error", result.ViewName);
            Assert.AreEqual("error test", mod.error);
        }
    }
}