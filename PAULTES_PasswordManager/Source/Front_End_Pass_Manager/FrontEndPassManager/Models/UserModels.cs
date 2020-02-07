using System.ComponentModel.DataAnnotations;

namespace FrontEndPassManager.Models
{
    public class UserModels
    {
        [Display(Name = "Nom de compte :")]
        public string User { get; set; }
        public long Id { get; set; }
        [Display(Name = "Mot de passe :")]
        public string Mdp { get; set; }
        [Display(Name = "Email :")]
        public string Email { get; set; }
        public int Role { get; set; }
        public string HA { get; set; }
        public byte[] Salt { get; set; }
        public string ChallengeUser { get; set; }
    }
}
