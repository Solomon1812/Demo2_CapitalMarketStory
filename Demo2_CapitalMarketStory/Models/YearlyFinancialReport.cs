using System.ComponentModel.DataAnnotations;

namespace Demo2_CapitalMarketStory.Models
{
    public class YearlyFinancialReport
    {
        public int ReportId { get; set; } //primary key autoincrement


        [Display(Name = "Date imported")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2014-01-01", "2025-12-31", ErrorMessage = "Date out of bounds")]
        [Required()]
        public DateTime ImportDate { get; set; }


        [Display(Name = "Year of the report")]
        [Range(2014, 2024, ErrorMessage = "Year out of bounds")]
        [Required()]
        public int YearReported { get; set; }


        [Display(Name = "Active imobilizate")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double ActiveImobilizate { get; set; }


        [Display(Name = "Active circulante")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double ActiveCirculante { get; set; }


        [Display(Name = "Stocuri")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Stocuri { get; set; }


        [Display(Name = "Creante")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Creante { get; set; }


        [Display(Name = "Casa")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Casa { get; set; }


        [Display(Name = "Cheltuieli in avans")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double CheltuieliAvans { get; set; }


        [Display(Name = "Datorii")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Datorii { get; set; }


        [Display(Name = "Venituri in avant")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double VenituriAvans { get; set; }


        [Display(Name = "Provizioane")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Provizioane { get; set; }

        [Display(Name = "Capitaluri sociale totale")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double CapitaluriTotale { get; set; }


        [Display(Name = "Capitaluri varsate")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double CapitaluriVarsate { get; set; }


        [Display(Name = "Patrimoniu")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double Patrimoniu { get; set; }


        [Display(Name = "Cifra de afaceri neta")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double CifraAfaceriNet { get; set; }


        [Display(Name = "Venituri totale")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double VenituriTotale { get; set; }


        [Display(Name = "Cheltuieli totale")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double CheltuieliTotale { get; set; }


        [Display(Name = "Profit brut")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double ProfitBrut { get; set; }


        [Display(Name = "Pierdere bruta")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double PierdereBrut { get; set; }


        [Display(Name = "Profit net")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double ProfitNet { get; set; }


        [Display(Name = "Pierdere net")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public double PierdereNet { get; set; }


        [Display(Name = "Numarul de salariati")]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int NumarSalariati { get; set; }


        [Display(Name = "ROA Rentabilitatea activelor")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int ROA { get; set; }


        [Display(Name = "ROE Rentabilitatea capitalului propriu")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int ROE { get; set; }


        [Display(Name = "Marja de profit net")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int MarjaProfit { get; set; }


        [Display(Name = "Rata de crestere a cifrei de afaceri nete")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int RataCrestereCifraAfaceriNet { get; set; }


        [Display(Name = "Rata de crestere a profitului")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative")]
        public int RataCrestereProfitNet { get; set; }



    }
}
