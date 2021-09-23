using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Models
{
    public class SurveyCheckbox
    {
        public long Id { get; set; }
        public long SurveyId { get; set; }
        public Survey Survey { get; set; }

        public int  QueryNumber { get; set; }

        public int Value { get; set; }

    }
}
