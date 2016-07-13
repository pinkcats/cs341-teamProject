using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class AddCommentModel
    {
        public string message { get; set; }
        public int noteId { get; set; }
        public int rating { get; set; }
    }
}