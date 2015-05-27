namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Korisnik")]
    public partial class Korisnik
    {
        public Korisnik()
        {
            Burza = new HashSet<Burza>();
            KupljeneKarte = new HashSet<KupljeneKarte>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string korisnickoime { get; set; }

        [Required]
        [StringLength(50)]
        public string ime { get; set; }

        [Required]
        [StringLength(50)]
        public string prezime { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(11)]
        public string OIB { get; set; }

        public int IdGrad { get; set; }

        public virtual ICollection<Burza> Burza { get; set; }

        public virtual ICollection<KupljeneKarte> KupljeneKarte { get; set; }
    }
}
