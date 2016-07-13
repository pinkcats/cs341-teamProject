using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteShare.DataAccess
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(noteShareModel context) : base(context) { }

        public virtual Course GetExactMatch(Course course)
        {
            return context.Courses.Where(x => x.Department == course.Department &&
                                              x.Name == course.Name &&
                                              x.ProfessorName == course.ProfessorName &&
                                              x.Semester == course.Semester &&
                                              x.UniversityId == course.UniversityId &&
                                              x.Year == course.Year).FirstOrDefault();
        }
    }
}
