using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoteShare.Resources;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class RegistrationModel
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
        public int universityId { get; set; }
        public List<KeyValuePair> universityNameList { get; set; }
    }
}