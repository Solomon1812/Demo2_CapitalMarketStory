using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace Demo2_CapitalMarketStory.Models
{
    public class Import
    {
        public int ImportId { get; set; } 


        [Display(Name = "Upload date")]
        [DataType(DataType.Date)]
        [Required()]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        [Display(Name = "File name")]
        [StringLength(150, MinimumLength = 5)]
        [Required()]
        public string FileName { get; set; }


        [Display(Name = "File description")]
        [StringLength(250, MinimumLength = 0)]
        public string Description { get; set; }

        public int StartYear { get; set; }
        public int EndYear { get; set; }


        public int CompanyId { get; set; }
        public Company? Company { get; set; }



        public ICollection<YearlyFinancialReport> Reports { get; set; } = new List<YearlyFinancialReport>();


    }
}
