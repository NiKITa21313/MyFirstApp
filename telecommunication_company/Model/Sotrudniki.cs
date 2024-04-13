namespace telecommunication_company.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sotrudniki")]
    public partial class Sotrudniki
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sotrudniki()
        {
            Vipolnenie_zayavki = new HashSet<Vipolnenie_zayavki>();
        }

        [Key]
        public int ID_sotrudnika { get; set; }

        public int? ID_usera { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Поле ФАМИЛИЯ не должно иметь больше 50 символов")]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "Фамилия должна состоять из букв")]
        public string Familiya { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Поле ИМЯ не должно иметь больше 50 символов")]
        [RegularExpression("[А-Яа-я]+$", ErrorMessage = "Имя должно состоять из букв")]
        public string Imya { get; set; }

        [StringLength(50)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "Отчество должно состоять из букв")]
        public string Otchestvo { get; set; }

        [Required(ErrorMessage = "Дата обязательна(формат ввода: ГГГГ-ММ-ДД)")]
        [Column(TypeName = "date")]
        public DateTime? Data_rojdeniya { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Выберите должность из предложенного списка")]
        public int Doljnost { get; set; }

        [Required(ErrorMessage = "Адрес обязателен")]
        [RegularExpression(@"^\w+\s\d+$", ErrorMessage = "Введите улицу и через пробел номер дома")]
        [StringLength(50)]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен(формат ввода номера +79999541234)")]
        [RegularExpression(@"^\+7\d{3}\d{3}\d{4}$", ErrorMessage = "Введите номер в формате +79991234567")]
        public string Nomer_telefona { get; set; }

        [Required(ErrorMessage = "Почта обязательна")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "формат ввода почты abc@mail.com")]
        [StringLength(50)]
        public string Pochta { get; set; }

        public int? id_pasporta { get; set; }

        public virtual Doljnost Doljnost1 { get; set; }

        public virtual Pasport Pasport { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vipolnenie_zayavki> Vipolnenie_zayavki { get; set; }
    }
}
