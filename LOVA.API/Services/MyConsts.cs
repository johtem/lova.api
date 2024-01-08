using LOVA.API.Models;
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
        public const int LongActivationTime = 100;
        public const int resetGridWaitTime = 2;

        // Årsavgifter
        public const string vagAvgift = "2 100";
        public const string tomtAvgift = "800";
        public const string vaAvgift = "2 750";
        public const string enbartAvloppsAvgift = "3 750";
        public const string vaAnslutningsAvgiftService = "156 781";
        public const string vaAnslutningsAvgiftLagenhet = "53 799";
        public const string vaAnslutningsAvgift = "210 580";
        public const string tappvattenAvgift = "26,50";
        public const string rensAvgift = "3 500";
        
        public const string bredbandsAvgift = "109";
        public const string tvAvgift = "110";

        



        /// <summary>
        ///  Azure storage blob containers   
        /// </summary>
        public const string boardDocuments = "styrelsedokument";
        public const string lottingelundDocuments = "lottingelundfiles";
        public const string lovaDocuments = "lovafiles";
        public const string lovaPhotos = "lovaphotos";


        //public static readonly BoardMember Ordf = new BoardMember { Name = "Ronny Modigs", Email = "ronmod@gmail.com", Phone = "0706-040610" }; 
        public static readonly BoardMember Ordf = new BoardMember { Name = "Yvonne Wassberg", Email = "yvw@lottingelund.se", Phone = "076 188 66 88" }; 
        public static readonly BoardMember Kansli = new BoardMember { Name = "Magnus Larzon", Email = "magnus.larzon@lottingelund.se", Phone = "0733-327515" }; 
        public static readonly BoardMember Kassor = new BoardMember { Name = "Lilling Palmeklint", Email = "lilling@ownit.nu", Phone = "0706-201310" }; 
        public static readonly BoardMember VA = new BoardMember { Name = "Johan Tempelman", Email = "johan@tempelman.nu", Phone = "0734-435407" }; 
        public static readonly BoardMember Tomt = new BoardMember { Name = "Christer Anrell", Email = "christer@christeranrell.se", Phone = "08-343131" }; 
        public static readonly BoardMember Vag = new BoardMember { Name = "Lennart Källqvist", Email = "l.kallqvist@lottingelund.se", Phone = "0762-067985" }; 
        public static readonly BoardMember Suppleant1 = new BoardMember { Name = "Micke Vikström", Email = "micke@lottingelund.se", Phone = "0708-223315" }; 
        //public static readonly BoardMember Suppleant2 = new BoardMember { Name = "Stefan Wassberg (Adjungerad)", Email = "stw@lottingelund.se", Phone = "0702-560976" }; 
        public static readonly BoardMember Suppleant2 = new BoardMember { Name = "Olof Erixon", Email = "erixonolof@gmail.com", Phone = "" }; 


    }
}
