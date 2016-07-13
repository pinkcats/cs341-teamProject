using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using NoteShare.DataAccess;

namespace NoteShare.Resources
{
    public class InputValidator
    {
        private String failReason;

        public InputValidator()
        {
            this.clearLastFail();
        }

        public Boolean validateRegistration(String username, String email, String password, String passwordConfirm)
        {
            this.clearLastFail();
            bool usernameValid = true;
            bool emailValid = true;
            bool passwordValid = true;

            if (!validMaxLength(100, username))
            {
                usernameValid = false;
                failReason = failReason + "Username exceeded max length of 100. ";
            }
            else if (!validMinLength(1, username))
            {
                usernameValid = false;
                failReason = failReason + "Username does not meet min length of 1. ";
            }
            else if (!validMaxLength(100, email))
            {
                emailValid = false;
                failReason = failReason + "Email exceeded max length of 100. ";
            }
            else if (!validMinLength(7, email))
            {
                emailValid = false;
                failReason = failReason + "Email does not meet min length of 7. ";
            }
            else if (!validEmail(email))
            {
                emailValid = false;
                failReason = failReason + "Email does not meet .NET email criteria. ";
            } else if (!validMaxLength(20, password))
            {
                passwordValid = false;
                failReason = failReason + "Password exceeded max length of 255. ";
            }
            else if (!validMinLength(8, password))
            {
                passwordValid = false;
                failReason = failReason + "Password does not meet min length of 8. ";
            }
            else if (!inputMatches(password, passwordConfirm))
            {
                passwordValid = false;
                failReason = failReason + "Passwords do not match. ";
            }
            else
            {
                failReason = "";
            }

            return usernameValid && emailValid && passwordValid;
        }

        public Boolean validateUpdateAccount(String email, int universityId)
        {
            this.clearLastFail();
            bool emailValid = true;
            bool universityValid = true;

            if (!validMaxLength(100, email))
            {
                emailValid = false;
                failReason = failReason + "Email exceeded max length of 100.";
            }else if (!validMinLength(7, email))
            {
                emailValid = false;
                failReason = failReason + "Email does not meet min length of 7. ";//a@a.com <- min length example
            }else if (!validEmail(email))
            {
                emailValid = false;
                failReason = failReason + "Email does not meet .NET email criteria. ";
            }else if (!validUniversityId(universityId))
            {
                universityValid = false;
                failReason = failReason + "University is not in Noteshare database.";
            }
            else
            {
                failReason = "";
            }

            return emailValid && universityValid;

        }

        public Boolean validateAddComment(String comment)
        {
            this.clearLastFail();
            bool validComment = true;
            if(!validMaxLength(2000, comment))
            {
                failReason = failReason + "Comment exceeded mac length of 2000 chars. ";
                validComment = false;
            }

            return validComment;
        }
        
        public Boolean validateUploadFile(HttpPostedFileBase file)
        {
            this.clearLastFail();
            bool validFile = true;
            List<string> acceptedTypes = new List<string>{"application/pdf", "application/msword", "application/excel",
                                          "application/mspowerpoint", "application/powerpoint", "image/gif", "image/jpg",
                                          "image/jpeg", "image/png", "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                          "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                          "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation"};

            if (!acceptedTypes.Contains(file.ContentType))
            {
                validFile = false;
                failReason = failReason + file.ContentType + " is not a supported file type.";
            }

            return validFile;

        }
        
        public Boolean validateUploadCourse(Course course)
        {
            bool courseValid = true;
            this.clearLastFail();

            if(!validMaxLength(100, course.Name))
            {
                courseValid = false;
                failReason = failReason + "Course name exceeded 100 chars. ";
            } else if(!validMinLength(1, course.Name))
            {
                courseValid = false;
                failReason = failReason + "Course name cannot be empty. ";
            } else if (!validMaxLength(100, course.ProfessorName))
            {
                courseValid = false;
                failReason = failReason + "Professor name exceeded 100 chars. ";
            } else if(!validMinLength(1, course.ProfessorName))
            {
                courseValid = false;
                failReason = failReason + "Professor name cannot be empty. ";
            } else if(!validMaxLength(100, course.Department))
            {
                courseValid = false;
                failReason = failReason + "Dept name exceeded 100 chars. ";
            } else if (!validMinLength(1, course.Department))
            {
                courseValid = false;
                failReason = failReason + "Dept name cannot be empty. ";
            } else if (!validUniversityId(course.UniversityId))
            {
                courseValid = false;
                failReason = failReason + "UniversityID not found in Noteshare database.";
            } else if (!validYear(course.Year))
            {
                courseValid = false;
                failReason = failReason + "Invalid  year.";
            } else if (!validSemester(course.Semester))
            {
                courseValid = false;
                failReason = failReason + "Invalid semester";
            } else
            {

            }

            return courseValid;
        }

        public Boolean validateUploadNote(String title, String description)
        {
            bool noteValid = true;
            failReason = "";

            if(!validMaxLength(255, title))
            {
                noteValid = false;
                failReason = failReason + "Note title exceeded 250 chars. ";
            } else if (!validMinLength(1, title))
            {
                noteValid = false;
                failReason = failReason + "Note title must not be empty. ";
            } else if(!validMaxLength(2000, description))
            {
                noteValid = false;
                failReason = failReason + "Note description cannot exceed 2000 chars. ";
            } else
            {
              
            }

            return noteValid;
        }

        public String lastFail()
        {
            return failReason;
        }

        private void clearLastFail()
        {
            failReason = "";
        }

        private Boolean inputMatches(String input1, String input2)
        {
            return input1.Equals(input2);
        }

        private Boolean validMaxLength(int maxLength, String input)
        {
            if (input == null)
            {
                return true;
            }
            else
            {
                return (input.Length <= maxLength);
            }
        }

        private Boolean validMinLength(int minLength, String input)
        {
            return (input.Length >= minLength);
        }

        private Boolean validEmail(String email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private Boolean validUniversityId(int universityId)
        {
            return (universityId >= 0) && (universityId <= 7769);
        }

        private Boolean validYear(int year)
        {
            return (year > 1900 && year < 3000);
        }

        private Boolean validSemester(String semester)
        {
            return semester.Equals("Fall") || semester.Equals("Spring") || semester.Equals("Summer") || semester.Equals("Winterim") ||
                semester.Equals("Other");
        }
    } 
}