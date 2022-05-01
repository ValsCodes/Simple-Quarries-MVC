using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int ID_User { get; set; }
        [StringLength(100)]
        public string ProblemDesc { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "изчакващ";
        public int Id_Technition { get; set; }
    }
}
