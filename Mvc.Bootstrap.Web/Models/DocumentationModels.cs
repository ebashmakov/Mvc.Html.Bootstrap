using System.ComponentModel.DataAnnotations;

namespace Mvc.Bootstrap.Web.Models
{
    public class SampleModel
    {
        [Required]
        [Display(Name = "Sample")]
        public string Sample { get; set; }
    }
}