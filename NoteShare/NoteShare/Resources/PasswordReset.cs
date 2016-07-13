using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using NoteShare.DataAccess;

namespace NoteShare.Resources
{   
    /** Password reset offers tools to reset user passwords.
    *
    *   @author Justin Gottschalk
    *    @version 1.0
    *    @since 12/4/2015
    */
    public class PasswordReset
    {
        public SmtpClient smtp = new SmtpClient("mail.twc.com");
        public PasswordReset()
        {
        }

        /** Creates a random password of length n for using System.Web.Security
        *    with nonalphanumerics to increase potential results.
        */
        public virtual string reset(int length)
        {
            return Membership.GeneratePassword(length, 1);
        }

        /** Send an email from NoteShare indicating password has changed
        *
        *   @param email Email to be notified.
        *
        */
        public virtual void mailNotify(String email, string password)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add(email);
                message.Subject = "NOREPLY: NoteShare Password Reset";
                message.From = new MailAddress("noteshare@new.rr.com");
                message.Body = "Good Day, \n Your temporary NoteShare password is " + password + " if you didn't request this change please contact tech support immediately.";
                smtp.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
    }
}