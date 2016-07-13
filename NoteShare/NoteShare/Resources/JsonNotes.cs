using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteShare.Resources
{
    public class JsonNotes
    {
        public DateTime dateAdded { get; set; }
        public string name { get; set; }
        public double rating { get; set; }
        public int noteId { get; set; }
    }
}