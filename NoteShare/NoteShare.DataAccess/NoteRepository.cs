using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShare.DataAccess
{
    public class NoteRepository : GenericRepository<Note>
    {
        public NoteRepository(noteShareModel context) : base(context) { }

        public virtual IEnumerable<Note> GetSearchNotes(string searchText, int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return context.Notes.Where(x => (x.Title.Contains(searchText) || x.Description.Contains(searchText) || x.FileType.Contains(searchText)));
            }
            else
            {
                return context.Notes.Where(x => (!x.IsPrivate || x.UserId == userId) && (x.Title.Contains(searchText) || x.Description.Contains(searchText) || x.FileType.Contains(searchText)));
            }
        }

        public virtual IEnumerable<Note> GetPublicNotes()
        {
            return context.Notes.Where(x => !x.IsPrivate);
        }

        public virtual IEnumerable<Note> GetUserNotes(int userId)
        {
            return context.Notes.Where(x => x.UserId == userId);
        }
    }
}
