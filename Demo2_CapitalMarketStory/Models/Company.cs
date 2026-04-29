using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo2_CapitalMarketStory.Models
{
    public class Company
    {
        public int CompanyId { get; set; } //primary key autoincrement

        public string? UserId { get; set; }


        [Display(Name = "Company name")]
        [StringLength(150, MinimumLength = 3)]
        [Required()]
        public string Name { get; set; } = string.Empty;


        [Display(Name = "Company's tax identification number")]
        [Range(0, 999999)]
        [Required()]
        public int CUI { get; set; }


        [Display(Name = "Date the company was founded")]
        [DataType(DataType.Date)]
        [Required()]
        public DateTime FoundedDate { get; set; }


        [Display(Name = "Type of activity CAEN")]
        [Range(0, 9999)]
        [Required()]
        public int CAEN { get; set; }


        [Display(Name = "Company's headquarters address")]
        [StringLength(250, MinimumLength = 5)]
        [Required()]
        public string HeadquartersAddress { get; set; } = string.Empty;


        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string? PhoneNumber { get; set; }


        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150, MinimumLength = 5)]
        public string? Email { get; set; }


        [Display(Name = "Company's website")]
        [DataType(DataType.Url)]
        [StringLength(150, MinimumLength = 5)]
        public string? Website { get; set; }



        //navigation property
        public ICollection<Import> Imports { get; set; } = new List<Import>();

    }
}
