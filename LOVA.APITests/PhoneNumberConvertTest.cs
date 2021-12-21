using LOVA.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOVA.APITests
{

    [TestClass()]
    public class PhoneNumberConvertTest
    {
        [TestMethod()]
        public void PhoneNumberNullTest()
        {
            var phoneNumber = new PhoneNumberConverter();
            Assert.IsNotNull(phoneNumber);

        }

        [TestMethod()]
        public void VerifyPhoneNumberTest()
        {

            // Arrange
            var phoneNumber = new PhoneNumberConverter();

            // Act
            var match1 = phoneNumber.ConvertPhoneNumber("0734435407");
            var match2 = phoneNumber.ConvertPhoneNumber("0734 435 407");
            var match3 = phoneNumber.ConvertPhoneNumber("0734-435407");
            var match4 = phoneNumber.ConvertPhoneNumber("46734-435407");
            var match5 = phoneNumber.ConvertPhoneNumber("+46734-435407");
            var match6 = phoneNumber.ConvertPhoneNumber("+460734-435407");
            

            // Assert
            Assert.AreEqual(match1, match2);
            Assert.AreEqual("+46734435407", match1);
            Assert.AreEqual("+46734435407", match3);
            Assert.AreEqual("+46734435407", match4);
            Assert.AreEqual("+46734435407", match5);
            Assert.AreEqual("+46734435407", match6);


        }
    }
}
