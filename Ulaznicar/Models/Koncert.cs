namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Koncert")]
    public partial class Koncert
    {
        public int Id { get; set; }

        public int IdDogadjaj { get; set; }

        public int idIzvodjac { get; set; }
    }
}
