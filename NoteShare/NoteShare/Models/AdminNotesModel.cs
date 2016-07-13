using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class AdminNotesModel
    {
        public List<UserNotes> userNotes { get; set; } 
    }
    public class UserNotes
    {
        public List<Note> notes { get; set; }
        public User user { get; set; }
        public double rating { get; set; }
    }

}