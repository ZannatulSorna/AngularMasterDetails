using System.ComponentModel.DataAnnotations.Schema;

namespace AngularMasterDetails.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISO3 { get; set; }
        public string ISO2 { get; set; }
        [NotMapped]
        public List<City>  Cities { get; set; }
    }
}
