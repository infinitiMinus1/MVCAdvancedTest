using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Models
{
    public class KlantViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string VoorNaam { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht.")]
        public int Postcode { get; set; }
    }
}
