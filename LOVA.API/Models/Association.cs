using LOVA.API.Models.Lova;
using System.Collections.Generic;

namespace LOVA.API.Models
{
    public class Association
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string VAT { get; set; }

        public ICollection<MaintenanceGroup> MaintenanceGroups { get; set; }

    }
}
