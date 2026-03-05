using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo2_CapitalMarketStory.Models
{
    public class YearlyFinancialReport
    {
        [Key]
        public int ReportId { get; set; } //primary key autoincrement


        [Display(Name = "Company's tax identification numer")]
        [Required()]
        public int CUI { get; set; }


        [Display(Name = "Year of the report")]
        [Range(2014, 2024, ErrorMessage = "Year out of bounds")]
        [Required()]
        public int YearReported { get; set; }


        [Display(Name = "Active imobilizate")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal ActiveImobilizate { get; set; }


        [Display(Name = "Active circulante")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal ActiveCirculante { get; set; }


        [Display(Name = "Stocuri")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Stocuri { get; set; }


        [Display(Name = "Creante")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Creante { get; set; }


        [Display(Name = "Casa")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Casa { get; set; }


        [Display(Name = "Cheltuieli in avans")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal CheltuieliAvans { get; set; }


        [Display(Name = "Datorii")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Datorii { get; set; }


        [Display(Name = "Venituri in avant")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal VenituriAvans { get; set; }


        [Display(Name = "Provizioane")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Provizioane { get; set; }

        [Display(Name = "Capitaluri sociale totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal CapitaluriTotale { get; set; }


        [Display(Name = "Capitaluri varsate")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal CapitaluriVarsate { get; set; }


        [Display(Name = "Patrimoniu")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal Patrimoniu { get; set; }


        [Display(Name = "Cifra de afaceri neta")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal CifraAfaceriNet { get; set; }


        [Display(Name = "Venituri totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal VenituriTotale { get; set; }


        [Display(Name = "Cheltuieli totale")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal CheltuieliTotale { get; set; }


        [Display(Name = "Profit brut")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal ProfitBrut { get; set; }


        [Display(Name = "Pierdere bruta")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal PierdereBrut { get; set; }


        [Display(Name = "Profit net")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal ProfitNet { get; set; }


        [Display(Name = "Pierdere net")]
        [Range(typeof(decimal), "0", "9999999999")]
        public decimal PierdereNet { get; set; }


        [Display(Name = "Numarul de salariati")]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int NumarSalariati { get; set; }


        [Display(Name = "ROA Rentabilitatea activelor")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal ROA { get; set; }
        //interval optim 5%-15%


        [Display(Name = "ROE Rentabilitatea capitalului propriu")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal ROE { get; set; }
        //interval optim 15%-20%


        [Display(Name = "Marja de profit net")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal MarjaProfit { get; set; }
        //interval optim 1%-15%


        [Display(Name = "Rata de crestere a cifrei de afaceri nete")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal RataCrestereCifraAfaceriNet { get; set; }


        [Display(Name = "Rata de crestere a profitului")]
        [Column(TypeName = "decimal(14,4)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public decimal RataCrestereProfitNet { get; set; }


        //foreign key and navigation property
        public int? ImportId { get; set; }
        public Import? Import { get; set; }


    }
}
