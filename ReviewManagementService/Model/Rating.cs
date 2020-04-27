using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewManagementService.Model
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rateid { get; set; }
        public decimal rating { get; set; }
        public string review { get; set; }
        public int userid { get; set; }
        public int restaurentid { get; set; }
        public DateTime ratedate { get; set; }
    }
}
