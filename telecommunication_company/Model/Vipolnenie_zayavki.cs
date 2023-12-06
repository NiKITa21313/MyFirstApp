namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vipolnenie_zayavki
    {
        [Key]
        public int ID_vipolneniya_zayavki { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data_nachala { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data_okonchaniya { get; set; }

        public int ID_sotrudnika { get; set; }

        [Column(TypeName = "money")]
        public decimal Stoimost { get; set; }

        public int? ID_zayavki { get; set; }

        public virtual Sotrudniki Sotrudniki { get; set; }

        public virtual Zayavka Zayavka { get; set; }
    }
}
