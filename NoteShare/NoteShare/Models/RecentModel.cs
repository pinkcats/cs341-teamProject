using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class RecentModel
    {
       public List<Note> noteList { get; set; }
       public List<Comment> commentList { get; set; }
    }
}