using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteShare.Filters;
using NoteShare.DataAccess;
using NoteShare.Models;
using NoteShare.Resources;

namespace NoteShare.Controllers
{
    [PermissionAuthorize(PermissionEnum.ADMIN)]
    public class AdminController : Controller
    {
        private UnitOfWork database;

        public AdminController()
        {
            this.database = new UnitOfWork();
        }

        public AdminController(UnitOfWork database)
        {
            this.database = database;
        }

        [HttpGet]
        public ActionResult Recent()
        {
            RecentModel rmodel = new RecentModel();
            DateTime currentDate = DateTime.Today;
            rmodel.noteList = new List<Note>();
            rmodel.commentList = new List<Comment>(); 
            rmodel.noteList = database.NoteRepository.GetAll();
            rmodel.commentList = database.CommentRepository.GetAll();

            foreach(var comment in rmodel.commentList)
            {
                if (comment.Message.Length > 100)
                {
                    comment.Message = comment.Message.Substring(0, 99);
                }
            }

            return this.View("Recent", rmodel);
        }

        [HttpGet]
        public ActionResult ReportedNotes()
        {
            var reportedNotes = database.ReportedNoteRepository.GetAll();
            var notes = database.NoteRepository.GetAll().Where(x => reportedNotes.Any(y => y.NoteId == x.Id)).ToList();
            var users = database.UserRepository.GetAll();
            List<NoteReported> listOfNotes = new List<NoteReported>();
            foreach (var note in notes)
            {
                NoteReported n = new NoteReported();
                n.title = note.Title;
                n.noteId = note.Id;
                var reports = reportedNotes.Where(x => x.NoteId == note.Id).ToList();
                n.reports = reports.Select(x => new ReportedUser() { reportText = x.Reason, username = users.Single(y => y.Id == x.UserId).Username }).ToList();
                n.noteUser = users.Single(x => x.Id == note.UserId).Username;
                n.noteDate = note.DateAdded;
                listOfNotes.Add(n);
            }

            ReportedNotesModel model = new ReportedNotesModel() { reportedNotes = listOfNotes };
            return this.View("ReportedNotes", model);
        }

        [HttpGet]
        public ActionResult DashBoard()
        {
           AdminNotesModel model = new AdminNotesModel();
           var users = database.UserRepository.GetAll();
           var allnotes = database.NoteRepository.GetAll();
           var allComments = database.CommentRepository.GetAll();
           List<UserNotes> notes = new List<UserNotes>();
           foreach (var user in users)
           {
               UserNotes userNotes = new UserNotes();
               userNotes.notes = allnotes.Where(x => x.UserId == user.Id).ToList();

               var comments = allComments.Where(x => userNotes.notes.Any(y => y.Id == x.NoteId));
               userNotes.rating = (comments.Count() > 0) ? comments.Average(x => x.Rating) : -1;
               userNotes.user = user;
               notes.Add(userNotes);
           }
           model.userNotes = notes;
            return this.View("DashBoard", model);
        }

        [HttpGet]
        public void UnlockUser(int id)
        {
            var user = database.UserRepository.GetByID(id);
            user.FailedLoginAttempts = 0;
            user.IsSuspended = false;
            database.Save();
        }

        [HttpPost]
        public void SuspendUser(int userId)
        {
            var user = database.UserRepository.GetByID(userId).IsSuspended = true;
            database.Save();
        }

        [HttpPost]
        public void UnsuspendUser(int userId)
        {
            var user = database.UserRepository.GetByID(userId).IsSuspended = false;
            database.Save();
        }

        [HttpPost]
        public void SuspendNote(int noteId)
        {
            database.NoteRepository.GetByID(noteId).IsSuspended = true;
            database.Save();
        }

        [HttpPost]
        public void UnsuspendNote(int noteId)
        {
            database.NoteRepository.GetByID(noteId).IsSuspended = false;
            database.Save();
        }
    }
}