using NoteShare.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net.Mime;
using NoteShare.Resources;
using NoteShare.DataAccess;
using NoteShare.Filters;
namespace NoteShare.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork database;

        public HomeController()
        {
            this.database = new UnitOfWork();
        }

        public HomeController(UnitOfWork database)
        {
            this.database = database;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        public ActionResult Search()
        {
            var universities = database.UniversityRepository.GetAll().Select(x => new KeyValuePair() { key = x.Id, value = x.Name }).ToList();
            UniversitySearchModel model = new UniversitySearchModel() { universities = universities };
            return View("Search", model);
        }

        [HttpGet]
        public JsonResult SearchBar(string term)
        {
            var userId = -1;
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            Permission permission = null;
            if(User.Identity.IsAuthenticated) {
                userId = user.Id;
                permission = database.PermissionRepository.GetPermissionByUserId(user.Id);
            }

            List<Note> results = new List<Note>();
            bool isAdmin = false;
            if (permission != null)
            {
                if (PermissionEnum.ADMIN == (PermissionEnum)permission.PermissionLevel)
                {
                    isAdmin = true;
                }
            }

            results = database.NoteRepository.GetSearchNotes(term, userId, isAdmin).ToList();

            return this.Json(results.Select(x => new { key = x.Id, value = x.Title }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Search(SearchModel model)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            IEnumerable<Note> notes = new List<Note>();
            if (user != null)
            {
                var permission = database.PermissionRepository.GetPermissionByUserId(user.Id);
                if ((PermissionEnum)permission.PermissionLevel == PermissionEnum.ADMIN)
                {
                    notes = database.NoteRepository.GetAll();
                }
                else
                {
                    notes = database.NoteRepository.GetPublicNotes();
                }
            }
            else
            {
                notes = database.NoteRepository.GetPublicNotes();
            }
            
            var courses = database.CourseRepository.GetAll();

            if (model.description != null)
            {
                notes = notes.Where(x => x.Description.ToLower().Contains(model.description.ToLower()) || x.Title.ToLower().Contains(model.description.ToLower()));
            }

            if (model.university != -1)
            {
                var fcourses = courses.Where(x => x.UniversityId == model.university);
                notes = notes.Where(x => fcourses.Any(y => y.Id == x.CourseId));
            }

            if (model.fileType != null)
            {
                notes = notes.Where(x => FileHelper.searchFileTypeMap(model.fileType).Contains(x.FileType));
            }

            if (model.professor != null)
            {
                var fcourses = courses.Where(x => x.ProfessorName.ToLower().Contains(model.professor.ToLower()));
                notes = notes.Where(x => fcourses.Any(y => y.Id == x.CourseId));
            }

            if (model.department != null)
            {
                var fcourses = courses.Where(x => x.Department.ToLower().Contains(model.department.ToLower()));
                notes = notes.Where(x => fcourses.Any(y => y.Id == x.CourseId));
            }

            if (model.className != null)
            {
                var fcourses = courses.Where(x => x.Name.ToLower().Contains(model.className.ToLower()));
                notes = notes.Where(x => fcourses.Any(y => y.Id == x.CourseId));
            }

            var allComments = database.CommentRepository.GetAll();
           IEnumerable<Comment> comments = new List<Comment>();

            double rating = -1;
            List<JsonNotes> listOfNotes = new List<JsonNotes>();
            foreach (Note note in notes)
            {
                comments = allComments.Where(x => x.NoteId == note.Id);
                rating = -1;

                if (comments.Count<Comment>() > 0)
                {
                    rating = comments.Average(x => x.Rating);
                }

                listOfNotes.Add(new JsonNotes() { dateAdded = note.DateAdded, name = note.Title, noteId = note.Id, rating = rating });
            }

            return this.Json(listOfNotes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Notes()
        {
            UserNotesModel model = new UserNotesModel();
            model.courseNotes = new List<CourseNote>();
            User user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var notes = database.NoteRepository.GetAll().Where(x => x.UserId == user.Id).ToList();
            var courses = database.CourseRepository.GetAll();

            foreach (var note in notes)
            {
                CourseNote cn = new CourseNote();
                cn.note = note;
                cn.course = courses.SingleOrDefault(x => x.Id == note.CourseId);
                model.courseNotes.Add(cn);
            }
            
            return this.View("Notes", model);
        }

        [HttpPost]
        [Authorize]
        public void DeleteNote(int noteId)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var note = database.NoteRepository.GetByID(noteId);
            var permission = database.PermissionRepository.GetPermissionByUserId(user.Id);
            
            if (note.UserId == user.Id)
            {
                database.NoteRepository.Delete(noteId);
                database.Save();
            }
            if (permission.PermissionLevel == 2)
            {
                database.NoteRepository.Delete(noteId);
                database.Save();
            }

        }

        [HttpPost]
        [Authorize]
        public void MakePrivate(int noteId)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var note = database.NoteRepository.GetByID(noteId);

            if (note.UserId == user.Id)
            {
                database.NoteRepository.GetByID(noteId).IsPrivate = true;
                database.Save();
            }
        }

        [HttpPost]
        [Authorize]
        public void MakePublic(int noteId)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var note = database.NoteRepository.GetByID(noteId);

            if (note.UserId == user.Id)
            {
                database.NoteRepository.GetByID(noteId).IsPrivate = false;
                database.Save();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Upload()
        {
            UploadModel model = new UploadModel();
            User user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var uni = database.UniversityRepository.GetByID(user.UniversityId);

            model.university = user.UniversityId;
            model.universityName = uni.Name;

            return this.View("Upload", model);
        }

        [HttpPost]
        public ActionResult Upload(UploadModel model)
        {

            //needs to build first to do validation
            Course course = new Course();
            course.Name = model.courseName;
            course.ProfessorName = model.professor;
            course.Department = model.topic;
            course.UniversityId = model.university;
            course.Year = model.year;
            course.Semester = model.semester;
            if(model.noteText == null)
            {
                model.noteText = "(no description)";
            }

            //validation
            InputValidator iv = new InputValidator();
            String error;
            bool fileTypeValid = iv.validateUploadFile(model.file);
            error = iv.lastFail(); 
            bool courseValid = iv.validateUploadCourse(course);
            error = error + iv.lastFail();
            bool noteValid = iv.validateUploadNote(model.title, model.noteText);
            error = error + iv.lastFail();
            bool fileSizeTooBig = (model.file.ContentLength > 2000000000);
            if (fileSizeTooBig)
            {
                error = error + "The max file size is 2GB. ";
            }

            if (fileTypeValid && courseValid && noteValid)
            {
                var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
                byte[] data = new byte[1];
                string type = "";
                if (model.file != null)
                {
                    MemoryStream target = new MemoryStream();
                    model.file.InputStream.CopyTo(target);
                    data = target.ToArray();
                    type = model.file.ContentType;
                    
                }

                int courseId = -1;
                Course inDatabase = database.CourseRepository.GetExactMatch(course);
                if (inDatabase == null)
                {
                    database.CourseRepository.Insert(course);
                    database.Save();

                    Course courseJustAdded = database.CourseRepository.GetExactMatch(course);
                    courseId = courseJustAdded.Id;
                }
                else
                {
                    courseId = inDatabase.Id;
                }

                Note note = new Note();
                note.DateAdded = DateTime.Now;
                note.FileContents = data;
                note.FileType = type;
                note.IsPrivate = false;
                note.Title = model.title;
                note.UserId = user.Id;
                note.CourseId = courseId;
                note.Description = model.noteText;

                database.NoteRepository.Insert(note);
                database.Save();

                return UploadSuccess();
            }
            else
            {
                return ErrorUpload(error);
            }
        }

        [HttpGet]
        public ActionResult UploadSuccess()
        {
            return this.View("UploadSuccess");
        }

        [HttpGet]
        public ActionResult Document(int id)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var permission = (user != null) ? database.PermissionRepository.GetPermissionByUserId(user.Id) : null;
            var document = database.NoteRepository.GetByID(id);
            ViewDocumentModel model = new ViewDocumentModel();

            if (permission == null || ((PermissionEnum)permission.PermissionLevel != PermissionEnum.ADMIN))
            {
                if (document.IsPrivate && user == null)
                {
                    return this.View("NotAuthorized");
                }
                else if (document.IsPrivate && document.UserId != user.Id)
                {
                    return this.View("NotAuthorized");
                }
            }
            
            if (document != null)
            {
                model.documentId = document.Id;

                if (document.FileType == "text/plain")
                {
                    model.contents = Encoding.Default.GetString(document.FileContents);
                }
                else
                {
                    model.contents = "data:" + document.FileType + ";base64," + Convert.ToBase64String(document.FileContents);
                }

                model.fileType = FileHelper.getFileTypeEnumFromMime(document.FileType);
                model.title = document.Title;
                model.uploadDate = document.DateAdded;
                model.description = document.Description;

                List<User> users = database.UserRepository.GetAll();
                List<Comment> comments = database.CommentRepository.GetAllByNoteId(document.Id);

                List<UserComment> userComments = new List<UserComment>();
                foreach (Comment c in comments)
                {
                    UserComment uc = new UserComment();
                    User u = users.SingleOrDefault(x => x.Id == c.UserId);
                    uc.comment = c;
                    uc.user = (u != null) ? u.Username : "No username";

                    userComments.Add(uc);
                }

                model.comments = userComments;

                return this.View("Document", model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public void ReportNote(ReportNoteModel model)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);

            if (user != null)
            {
                database.ReportedNoteRepository.Insert(new ReportedNote() { NoteId = model.noteId, Reason = model.text, UserId = user.Id });
                database.Save();
            }
        }

        [HttpGet]
        public FileContentResult Download(int id)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            var document = database.NoteRepository.GetByID(id);

            if (document.IsPrivate && user == null)
            {
                return null;
            }
            else if (document.IsPrivate && document.UserId != user.Id)
            {
                return null;
            }

            var fileName = FileHelper.getDownloadFileName(document.FileType);

            var cd = new ContentDisposition
            {
                FileName = fileName,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document.FileContents, document.FileType);
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegistrationModel model = new RegistrationModel();
            model.universityNameList = new List<KeyValuePair>();
            var universityPull = database.UniversityRepository.GetAll();
            foreach(var uni in universityPull)
            {
                model.universityNameList.Add(new KeyValuePair() { key = uni.Id, value = uni.Name });
            }

            return this.View("Register", model);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return this.View("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            ErrorModel errModel = new ErrorModel();
            var user = database.UserRepository.GetUserByUsername(model.username);

            if (user != null)
            {
                if (!(model.email).Equals(user.Email))
                {
                    errModel.error = "Input does match a valid account.";
                    return this.View("Error", errModel);

                }
                else if ((model.email).Equals(user.Email))
                {
                    //generates random pass to be displayed
                    PasswordReset pr = new PasswordReset();
                    string password = pr.reset(12);
                    //hashes and sets the random password, resets login attempts, unsuspends user
                    user.PasswordHash = PasswordHash.CreateHash(password);
                    user.FailedLoginAttempts = 0;
                    user.IsSuspended = false;
                    database.UserRepository.Update(user);
                    database.Save();
                    //emails user the password has been changed
                    pr.mailNotify(user.Email, password);
                    //show complete
                    return this.View("PasswordResetComplete");
                }
                else
                {
                    errModel.error = "An unexpected error occurred contact support";
                    return this.View("Error", errModel);
                }
            }
            else
            {
                errModel.error = "Input does match a valid account.";
                return this.View("Error", errModel);
            }
        }

        [HttpPost]
        public void AddComment(AddCommentModel model)
        {
            var user = database.UserRepository.GetUserByUsername(User.Identity.Name);
            Comment com = new Comment();
            InputValidator iv = new InputValidator();
            bool inputValid = iv.validateAddComment(model.message);

            if (inputValid)
            {
                com.Message = model.message;
                com.NoteId = model.noteId;
                com.UserId = user.Id;
                com.Rating = model.rating;
                database.CommentRepository.Insert(com);
                database.Save();
            }
            else
            {
                //do something to inform user
                
            }
        }

        [HttpPost]
        public void DeleteComment(int id)
        {
            database.CommentRepository.Delete(id);
            database.Save();
        }

        [HttpGet]
        public ActionResult ErrorLogin()
        {
            ErrorModel errModel = new ErrorModel();
            errModel.error = "Invalid username/password combination.";
            return View("Error", errModel);
        }

        [HttpGet]
        public ActionResult ErrorLoginSuspended()
        {
            ErrorModel errModel = new ErrorModel();
            errModel.error = "Account has been suspended.";
            return View("Error", errModel);
        }

        [HttpGet]
        public ActionResult University(int id)
        {
            return this.View("University", new UniversityModel() { university = database.UniversityRepository.GetByID(id) });
        }

        [HttpGet]
        public ActionResult ErrorUpload(String error)
        {
            ErrorModel errModel = new ErrorModel();
            errModel.error = error;
            return View("Error", errModel);
        }
    }
}