using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace Demo2_CapitalMarketStory.Models
{
    public class ImportHistory
    {
        public int ImportId { get; set; } //primary key autoincrement


        [Display(Name = "Date the file was uploaded in the ImportHistory/Create page")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2014-01-01", "2026-12-31", ErrorMessage = "Date out of bounds")]
        [Required()]
        public DateTime ImportDate { get; set; } = DateTime.Now;

        [Display(Name = "Name of the file uploaded in the ImportHistory/Create page")]
        [StringLength(150, MinimumLength = 5)]
        [Required()]
        public string FileName { get; set; }


        [Display(Name = "File description")]
        [StringLength(250, MinimumLength = 0)]
        public string Description { get; set; } 


        //foreign key and navigation property
        public int CompanyId { get; set; }
        public Company Company { get; set; }


    }
}
