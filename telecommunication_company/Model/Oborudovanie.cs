namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Oborudovanie")]
    public partial class Oborudovanie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Oborudovanie()
        {
            Zayavka = new HashSet<Zayavka>();
            Pokupka_oborudovaniya = new HashSet<Pokupka_oborudovaniya>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_oborudovaniya { get; set; }

        [Required]
        [StringLength(50)]
        public string Nazvanie { get; set; }

        [Column(TypeName = "money")]
        public decimal Zena { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zayavka> Zayavka { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pokupka_oborudovaniya> Pokupka_oborudovaniya { get; set; }
    }
}
