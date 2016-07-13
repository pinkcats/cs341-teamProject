using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.Resources;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class ViewDocumentModel
    {
        public int documentId { get; set; }
        public string contents { get; set; }
        public FileTypeEnum fileType { get; set; }
        public string title { get; set; }
        public DateTime uploadDate { get; set; }
        public List<UserComment> comments { get; set; }
        public string description { get; set; }
    }

    public class UserComment
    {
        public string user { get; set; }
        public Comment comment { get; set; }
    }
}