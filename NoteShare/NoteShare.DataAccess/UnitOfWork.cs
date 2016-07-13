using System;

namespace NoteShare.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private noteShareModel context = new noteShareModel();
        public UserRepository userRepository;
        public CourseRepository courseRepository;
        public NoteRepository noteRepository;
        public PermissionRepository permissionRepository;
        public GenericRepository<University> universityRepository;
        public CommentsRepository commentRepository;
        public GenericRepository<ReportedNote> reportedNoteRepository;

        public virtual UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                    this.userRepository = new UserRepository(context);
                return userRepository;
            }
        }

        public virtual CourseRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                    this.courseRepository = new CourseRepository(context);
                return courseRepository;
            }
        }

        public virtual NoteRepository NoteRepository
        {
            get
            {
                if (this.noteRepository == null)
                    this.noteRepository = new NoteRepository(context);
                return noteRepository;
            }
        }

        public virtual PermissionRepository PermissionRepository
        {
            get
            {
                if (this.permissionRepository == null)
                    this.permissionRepository = new PermissionRepository(context);
                return permissionRepository;
            }
        }

        public virtual GenericRepository<University> UniversityRepository
        {
            get
            {
                if (this.universityRepository == null)
                    this.universityRepository = new GenericRepository<University>(context);
                return universityRepository;
            }
        }

        public virtual CommentsRepository CommentRepository
        {
            get
            {
                if (this.commentRepository == null)
                    this.commentRepository = new CommentsRepository(context);
                return commentRepository;
            }
        }

        public virtual GenericRepository<ReportedNote> ReportedNoteRepository
        {
            get
            {
                if (this.reportedNoteRepository == null)
                    this.reportedNoteRepository = new GenericRepository<ReportedNote>(context);
                return reportedNoteRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}