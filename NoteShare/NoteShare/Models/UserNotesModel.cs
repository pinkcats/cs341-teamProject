using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class UserNotesModel
    {
        public List<CourseNote> courseNotes { get; set; }
    }

    public class CourseNote
    {
        public Note note { get; set; }
        public Course course { get; set; }
    }
}