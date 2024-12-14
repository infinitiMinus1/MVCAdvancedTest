using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Models
{
    public class Klant
    {
        public int KlantId { get; set; }
        public string Naam { get; set; }
        public string VoorNaam { get; set; }
        public string Straat_Nr { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public string KlantStat {  get; set; }
        public int HuurAantal { get; set; }
        [DisplayFormat(DataFormatString = "{0:d-M-yyyy}")]
        public DateTime DatumLid { get; set; }
        public bool LidGeld {  get; set; }

        public virtual ICollection<Verhuring> Verhuur {  get; set; }

    }
}
