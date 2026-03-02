using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace Demo2_CapitalMarketStory.Models
{
    public class Import
    {
        public int ImportId { get; set; } //primary key autoincrement


        [Display(Name = "Upload date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2014-01-01", "2026-12-31", ErrorMessage = "Date out of bounds")]
        [Required()]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        [Display(Name = "File name")]
        [StringLength(150, MinimumLength = 5)]
        [Required()]
        public string FileName { get; set; }


        [Display(Name = "File description")]
        [StringLength(250, MinimumLength = 0)]
        public string Description { get; set; } 


        //foreign key and navigation property
        public int CompanyId { get; set; }
        public Company Company { get; set; }


        public List<YearlyFinancialReport> Reports { get; set; }



    }
}
