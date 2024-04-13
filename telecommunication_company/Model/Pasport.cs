namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pasport")]
    public partial class Pasport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pasport()
        {
            Sotrudniki = new HashSet<Sotrudniki>();
        }

        [Key]
        public int id_pasporta { get; set; }

        [Required(ErrorMessage = "Серия паспорта обязательна")]
        [Range(1000, 9999)]
        public int? seriya { get; set; }

        [Required(ErrorMessage = "Номер паспорта обязателен")]
        [Range(100000, 999999)]
        public int? nomer { get; set; }

        [Required(ErrorMessage = "Обязательно напишите кем выдан паспорт")]
        [StringLength(100)]
        public string Vydan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sotrudniki> Sotrudniki { get; set; }
    }
}
