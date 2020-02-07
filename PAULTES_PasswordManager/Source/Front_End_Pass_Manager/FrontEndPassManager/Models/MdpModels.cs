using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FrontEndPassManager.Models
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

    public class TousLesMdp
    {
        public List<MdpModels> LesMdp { get; set; }
    }
}
