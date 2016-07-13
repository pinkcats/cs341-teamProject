using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteShare.Resources;

namespace NoteShare.Test
{
    [TestClass]
    public class TestInputValidator
    {
        [TestMethod]
        public void TestInputValidRegister1()
        {

            //tests valid input for each field

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser";
            String email = "good@email.com";
            String password = "goodpassword";
            String passwordConfirm = password;
            bool expected = true;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister2()
        {

            //tests too long username

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12";
            String email = "good@email.com";
            String password = "goodpassword";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister3()
        {
            //tests too short username

            //arrange
            InputValidator iv = new InputValidator();
            String username = "";
            String email = "good@email.com";
            String password = "goodpassword";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister4()
        {
            //tests email too long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12";
            String email = "threeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblind@email.com";
            String password = "goodpassword";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister5()
        {
            //tests email too short

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12";
            String email = "a@.com";
            String password = "goodpassword";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister6()
        {
            //tests pass too long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12";
            String email = "threeblind@email.com";
            String password = "twentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslong";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister7()
        {
            //tests pass too short

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12";
            String email = "threeblind@email.com";
            String password = "not8";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputValidRegister8()
        {
            //tests pass not matching

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12";
            String email = "threeblind@email.com";
            String password = "goodpassword";
            String passwordConfirm = "badpassword";
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void TestInputRegister9()
        {
            //tests all short

            //arrange
            InputValidator iv = new InputValidator();
            String username = "";
            String email = "test";
            String password = "test";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister10()
        {
            //tests all too long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12";
            String email = "threeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblind@email.com";
            String password = "twentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslong";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister11()
        {
            //tests username short email long pass long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "";
            String email = "threeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblind@email.com";
            String password = "twentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslong";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister12()
        {
            //tests username short email short pass long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "";
            String email = "short";
            String password = "twentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslong";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister13()
        {
            //tests username long email short pass long

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12";
            String email = "short";
            String password = "twentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslongtwentycharacterslong";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister14()
        {
            //tests username long email short pass short

            //arrange
            InputValidator iv = new InputValidator();
            String username = "gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12gooduser12";
            String email = "short";
            String password = "short";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInputRegister15()
        {
            //tests username short email long pass short

            //arrange
            InputValidator iv = new InputValidator();
            String username = "";
            String email = "threeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblindthreeblind@email.com";
            String password = "short";
            String passwordConfirm = password;
            bool expected = false;
            //act
            bool result = iv.validateRegistration(username, email, password, passwordConfirm);
            //assert
            Assert.AreEqual(expected, result);
        }

        //email: max/min/valid
        //univeristyid: 0<id<7769

        [TestMethod]
        public void validateUpdateAccount1()
        {
            //all valid input
                        
            //arrange
            InputValidator iv = new InputValidator();
            string email = "normal@normal.com";
            int univId = 4375;
            bool expected = true;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void validateUpdateAccount2()
        {
            // all input invalid

            //arrange
            InputValidator iv = new InputValidator();
            string email = "com";
            int univId = 9000;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount3()
        {
            //email valid past univid beynod max

            //arrange
            InputValidator iv = new InputValidator();
            string email = "normal@normal.com";
            int univId = 9000;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount4()
        {
            //email valid univid less that min

            //arrange
            InputValidator iv = new InputValidator();
            string email = "normal@normal.com";
            int univId = -1;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount5()
        {
            //email small univ id valid

            //arrange
            InputValidator iv = new InputValidator();
            string email = "@.com";
            int univId = 4375;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount6()
        {
            //email large univ id valid

            //arrange
            InputValidator iv = new InputValidator();
            string email = "somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!somethingthatislong!@.com";
            int univId = 4375;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount7()
        {
            //email valid length no @ univ id valid

            //arrange
            InputValidator iv = new InputValidator();
            string email = "something.com";
            int univId = 4375;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void validateUpdateAccount8()
        {
            //email valid length no . univ id valid

            //arrange
            InputValidator iv = new InputValidator();
            string email = "something@normalcom";
            int univId = 4375;
            bool expected = false;
            //act
            bool result = iv.validateUpdateAccount(email, univId);
            //assert
            Assert.AreEqual(expected, result);
        }
    }
}
