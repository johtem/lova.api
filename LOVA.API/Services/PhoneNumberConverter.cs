using PhoneNumbers;
using System;

namespace LOVA.API.Services
{
    public class PhoneNumberConverter
    {

        public string ConvertPhoneNumber(string orgNumber)
        {
            var util = PhoneNumberUtil.GetInstance();
            var number = util.Parse(orgNumber, "SE");
            string intNumber = $"+{number.CountryCode}{number.NationalNumber}";


            return intNumber;
        }
    }
}
