namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lokacija")]
    public partial class Lokacija
    {
        public Lokacija()
        {
            Dogadjaj = new HashSet<Dogadjaj>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string adresa { get; set; }

        [StringLength(100)]
        public string naziv { get; set; }

        public int IdGrad { get; set; }

        public virtual ICollection<Dogadjaj> Dogadjaj { get; set; }

        public virtual Grad Grad { get; set; }
    }
}
