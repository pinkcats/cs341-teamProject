using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteShare.DataAccess;

namespace NoteShare.Models
{
    public class HomeModel
    {
        public string stringExample { get; set; }
        public int intExample { get; set; }
        public DateTime dateExample { get; set; }
        public List<User> users { get; set; }
    }
}
