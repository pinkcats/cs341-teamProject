using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NoteShare.Resources;

namespace NoteShare.Test
{
    [TestClass]
    public class TestPasswordHash
    {
        [TestMethod]
        public void TestValidatePassword()
        {
            //Arrange
            var hash = "1000:enCBS5BfYLlbLyoP+0bvVfppjcHuKnMu:JJ6Qrza9VoGI7lUOcJQTHGWJtrYKOfFW";
            var password = "password";
            var passwordWrong = "test";

            //Act - Hash result is the result of one run of create hash
            var result1 = PasswordHash.ValidatePassword(password, hash);
            var result2 = PasswordHash.ValidatePassword(passwordWrong, hash);

            //Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }
    }
}
