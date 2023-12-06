namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zayavka")]
    public partial class Zayavka
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Zayavka()
        {
            Vipolnenie_zayavki = new HashSet<Vipolnenie_zayavki>();
            Oborudovanie = new HashSet<Oborudovanie>();
        }

        [Key]
        public int ID_zayavki { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data_zayavki { get; set; }

        public int Usluga { get; set; }

        public int ID_klienta { get; set; }

        public virtual Klienty Klienty { get; set; }

        public virtual Uslugi Uslugi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vipolnenie_zayavki> Vipolnenie_zayavki { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oborudovanie> Oborudovanie { get; set; }
    }
}
