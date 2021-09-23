using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOVA.API.ViewModels.Lottingelund
{
    public class SurveyViewModel
    {
        public string UserName { get; set; }
        public string Query1 { get; set; }
        public string Query2 { get; set; }
        public string Query3 { get; set; }
        public string Query4 { get; set; }
        public string Query5 { get; set; }
        public string Query6 { get; set; }
        public string Query7 { get; set; }
        public string Query8 { get; set; }
        public string Query9 { get; set; }
        public string Query10 { get; set; }
        public List<int> AreChecked1 { get; set; }
    }
}
