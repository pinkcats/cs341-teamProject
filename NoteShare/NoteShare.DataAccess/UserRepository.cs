using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShare.DataAccess
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(noteShareModel context) : base(context) { }

        public virtual User GetUserByUsername(string username)
        {
            return context.Users.SingleOrDefault(x => x.Username == username);
        }

        public virtual User GetExactMatch(User user)
        {
            return context.Users.Where(x => x.Username == user.Username &&
                                            x.Email == user.Email &&
                                            x.UniversityId == user.UniversityId &&
                                            x.PasswordHash == user.PasswordHash).FirstOrDefault();
        }
    }
}
