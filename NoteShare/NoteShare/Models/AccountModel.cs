using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.Resources;

namespace NoteShare.Models
{
    public class AccountModel
    {
        public List<KeyValuePair> universities { get; set; }
        public string email { get; set; }
        public int university { get; set; }
    }
}