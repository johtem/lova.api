namespace LOVA.API.Models.Lova
{
    public class MaintenanceGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public Association Association { get; set; }

        public int AssociationId { get; set; }
    }
}
