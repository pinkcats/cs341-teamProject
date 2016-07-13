using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.DataAccess;
using NoteShare.Resources;

namespace NoteShare.Models
{
    public class UploadModel
    {
        public int university { get; set; }
        public string universityName { get; set; }
        public string topic { get; set; }
        public string professor { get; set; }
        public string courseName { get; set; }
        public string courseNumber { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public string semester { get; set; }
        public string noteText { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}