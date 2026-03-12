using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo2_CapitalMarketStory.Models
{
    public class YearlyFinancialReport
    {
        [Key]
        [Ignore]
        public int ReportId { get; set; } //primary key autoincrement


        [Display(Name = "Company's tax identification numer")]
        [Required()]
        public int CUI { get; set; }


        [Name("An")] //legatura cu csv numele coloanei
        [Display(Name = "Year of the report")]
        [Range(2014, 2024, ErrorMessage = "Year out of bounds")]
        [Required()]
        public int YearReported { get; set; }


        [Name("Active Imobilizate - Total")]
        [Display(Name = "Active imobilizate")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal ActiveImobilizate { get; set; }


        [Name("Active Circulante - Total")]
        [Display(Name = "Active circulante")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal ActiveCirculante { get; set; }


        [Name("Stocuri")]
        [Display(Name = "Stocuri")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Stocuri { get; set; }


        [Name("Creante")]
        [Display(Name = "Creante")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Creante { get; set; }


        [Name("Casa si conturi la banci")]
        [Display(Name = "Casa")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Casa { get; set; }


        [Name("Cheltuieli in avans")]
        [Display(Name = "Cheltuieli in avans")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal CheltuieliAvans { get; set; }


        [Name("Datorii")]
        [Display(Name = "Datorii")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Datorii { get; set; }


        [Name("Venituri in avans")]
        [Display(Name = "Venituri in avans")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal VenituriAvans { get; set; }


        [Name("Provizioane")]
        [Display(Name = "Provizioane")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Provizioane { get; set; }


        [Name("Capitaluri - Total")]
        [Display(Name = "Capitaluri sociale totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal CapitaluriTotale { get; set; }


        [Name("Capital subscris varsat")]
        [Display(Name = "Capitaluri varsate")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal CapitaluriVarsate { get; set; }


        [Name("Patrimoniul regiei")]
        [Display(Name = "Patrimoniu")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal Patrimoniu { get; set; }


        [Name("Cifra de afaceri neta")]
        [Display(Name = "Cifra de afaceri neta")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal CifraAfaceriNet { get; set; }


        [Name("Venituri Totale")]
        [Display(Name = "Venituri totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal VenituriTotale { get; set; }


        [Name("Cheltuieli Totale")]
        [Display(Name = "Cheltuieli totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal CheltuieliTotale { get; set; }


        [Name("Profitul Brut")]
        [Display(Name = "Profit brut")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal ProfitBrut { get; set; }


        [Name("Pierderea Bruta")]
        [Display(Name = "Pierdere bruta")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal PierdereBrut { get; set; }


        [Name("Profitul Net")]
        [Display(Name = "Profit net")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal ProfitNet { get; set; }


        [Name("Pierderea Neta")]
        [Display(Name = "Pierdere net")]
        [Range(typeof(decimal), "0", "9999999999")]
        [Default(0)]
        public decimal PierdereNet { get; set; }


        [Name("Numar mediu de salariati")]
        [Display(Name = "Numarul de salariati")]
        [Range(0, int.MaxValue)]
        public int? NumarSalariati { get; set; }


        [Ignore]
        [Display(Name = "ROA Rentabilitatea activelor")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal ROA { get; set; }
        //interval optim 5%-15%


        [Ignore]
        [Display(Name = "ROE Rentabilitatea capitalului propriu")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal ROE { get; set; }
        //interval optim 15%-20%


        [Ignore]
        [Display(Name = "Marja de profit net")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal MarjaProfit { get; set; }
        //interval optim 1%-15%


        [Ignore]
        [Display(Name = "Rata de crestere a cifrei de afaceri nete")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal RataCrestereCifraAfaceriNet { get; set; }


        [Ignore]
        [Display(Name = "Rata de crestere a profitului")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal RataCrestereProfitNet { get; set; }


        //foreign key and navigation property
        [Ignore]
        public int? ImportId { get; set; }
        [Ignore]
        public Import? Import { get; set; }


    }
}
