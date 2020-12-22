using LOVA.API.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Extensions
{
    public static class DateExtensions
    {
        public static List<DateTime> TotalHours(DateTime newDate, DateTime savedDate)
        {

            DateTime NewDate = RemoveMinutesAndSeconds(newDate);
            DateTime SavedDate = RemoveMinutesAndSeconds(savedDate);

            TimeSpan timeSpan = (NewDate - SavedDate);

            // Number of missing hours
            double missingHours = timeSpan.TotalHours;

            var dates = new List<DateTime>();

            if (missingHours == 0)
            {
                dates.Add(NewDate);
            }
            else
            {
                for (var dt = SavedDate.AddHours(1); dt <= NewDate; dt = dt.AddHours(1))
                {
                    dates.Add(dt);
                }
            }

            return dates;
        }

        public static bool NewHour(DateTime newDate, DateTime savedDate)
        {

            DateTime NewDate = RemoveMinutesAndSeconds(newDate);
            DateTime SavedDate = RemoveMinutesAndSeconds(savedDate);

            bool newHour = true;

            if (NewDate.Hour != SavedDate.Hour)
            {
                newHour = false;
            }


            return newHour;
        }

        private static DateTime RemoveMinutesAndSeconds(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }
    }
}
