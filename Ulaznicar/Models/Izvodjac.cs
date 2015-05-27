namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Izvodjac")]
    public partial class Izvodjac
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string naziv { get; set; }
    }
}
