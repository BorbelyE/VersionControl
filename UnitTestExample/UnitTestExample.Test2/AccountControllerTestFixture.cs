using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test2
{
    public class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.ValidateEmail(email);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [
              Test,
              TestCase("abcdgffgnfREG", false),
              TestCase("WHWHD467", false),
              TestCase("seadgrz56tzh", false),
              TestCase("roVid1", false),
              TestCase("MegfeleloJelszo12", true)
        ]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            //Arrange
            AccountController ac = new AccountController();
            //Act
            var actualResult = ac.ValidatePassword(password);
            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        public void TestRegisterHappyPath(string email, string password)
        {
            // Arrange
            var accountController = new AccountController();

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }
    }
}
