using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace NoteShare.Models
{
    public class SearchModel
    {
        public SearchModel()
        {
            this.university = -1;
        }
        public int university { get; set; }
        public string className { get; set; }
        public string professor { get; set; }
        public string description { get; set; }
        public string department { get; set; }
        public string fileType { get; set; }
    }
}