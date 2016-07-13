using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShare.DataAccess
{
    public class CommentsRepository : GenericRepository<Comment>
    {
        public CommentsRepository(noteShareModel context) : base(context) { }

        public virtual List<Comment> GetAllByNoteId(int noteId)
        {
            return context.Comments.Where(x => x.NoteId == noteId).ToList();
        }
    }
}
