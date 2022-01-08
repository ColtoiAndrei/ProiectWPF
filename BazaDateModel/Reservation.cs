namespace BazaDateModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reservation")]
    public partial class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        public int? CustId { get; set; }

        public int? MovieId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
