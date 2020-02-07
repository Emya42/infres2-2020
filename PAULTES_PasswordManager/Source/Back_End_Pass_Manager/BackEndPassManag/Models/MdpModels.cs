using System.ComponentModel.DataAnnotations;

namespace BackEndPassManag.Models
{
    public class MdpModels
    {
        [Display(Name = "ID")]
        public long Id { get; set; }
        [Display(Name = "Site")]
        public string ReferenceSite { get; set; }
        [Display(Name = "Mot de passe")]
        public string Mdp { get; set; }
    }
}
