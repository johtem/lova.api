using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.Services
{
    public static class MyConsts
    {
        public const int HoursBackInTime = -48;
        public const int PageSize = 10;
        public const int DashboardItemSize = 8;




        /// <summary>
        ///  Azure storage blob containers   
        /// </summary>
        public const string boardDocuments = "styrelsedokument";
        public const string lottingelundDocuments = "lottingelundfiles";
        public const string lovaDocuments = "lovafiles";
        public const string lovaPhotos = "lovaphotos";
    }
}
