using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShare.DataAccess
{
    public class PermissionRepository : GenericRepository<Permission>
    {
        public PermissionRepository(noteShareModel context) : base(context) { }

        public virtual Permission GetPermissionByUserId(int userId)
        {
            return context.Permissions.SingleOrDefault(x => x.UserId == userId);
        }
    }
}
