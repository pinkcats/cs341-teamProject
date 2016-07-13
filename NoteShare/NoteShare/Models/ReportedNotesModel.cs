using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class ReportedNotesModel
    {
        public List<NoteReported> reportedNotes;
    }

    public class NoteReported
    {
        public string title { get; set; }
        public string noteUser { get; set; }
        public int noteId { get; set; }
        public DateTime noteDate { get; set; }
        public List<ReportedUser> reports { get; set; }
    }

    public class ReportedUser
    {
        public string username { get; set; }
        public string reportText { get; set; }
    }
}