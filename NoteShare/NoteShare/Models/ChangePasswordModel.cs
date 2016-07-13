using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteShare.Models
{
    public class ChangePasswordModel
    {
        public string oldPassword { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
    }
}