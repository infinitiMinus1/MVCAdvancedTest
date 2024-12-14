using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCData.Models
{
    public class Verhuring
    {
        [Key]
        public int VerhuurId { get; set; }
        public int KlantId { get; set; }
        public int FilmId { get; set; }
        public DateTime VerhuurDatum { get; set; }

        public virtual Film Film { get; set; }
        public virtual Klant Klant { get; set; }
    }
}
