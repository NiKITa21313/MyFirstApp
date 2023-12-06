namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Klienty")]
    public partial class Klienty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Klienty()
        {
            Pokupka_oborudovaniya = new HashSet<Pokupka_oborudovaniya>();
            Zayavka = new HashSet<Zayavka>();
        }

        [Key]
        public int ID_klienta { get; set; }

        [Required]
        [StringLength(50)]
        public string Familiya { get; set; }

        [Required]
        [StringLength(50)]
        public string Imya { get; set; }

        [StringLength(50)]
        public string Otchestvo { get; set; }

        [Required]
        [StringLength(16)]
        public string Nomer_telefona { get; set; }

        [Required]
        [StringLength(50)]
        public string Adres { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pokupka_oborudovaniya> Pokupka_oborudovaniya { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zayavka> Zayavka { get; set; }
    }
}
