namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pokupka_oborudovaniya
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pokupka_oborudovaniya()
        {
            Oborudovanie = new HashSet<Oborudovanie>();
        }

        [Key]
        public int ID_pokupki { get; set; }

        public int ID_oborudovaniya { get; set; }

        public int ID_klienta { get; set; }

        [Column(TypeName = "money")]
        public decimal Summa { get; set; }

        public virtual Klienty Klienty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oborudovanie> Oborudovanie { get; set; }
    }
}
