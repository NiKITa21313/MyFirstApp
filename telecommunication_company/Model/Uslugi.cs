namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Uslugi")]
    public partial class Uslugi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uslugi()
        {
            Zayavka = new HashSet<Zayavka>();
        }

        [Key]
        public int ID_uslugi { get; set; }

        [Required]
        [StringLength(50)]
        public string Nazvanie_uslugi { get; set; }

        [Required]
        public string Opisanie_uslugi { get; set; }

        [Column(TypeName = "money")]
        public decimal? Stoimost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zayavka> Zayavka { get; set; }
    }
}
