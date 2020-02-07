namespace BackEndPassManag.Models
{
    public partial class BaseMdps
    {
        public string User { get; set; }
        public long Id { get; set; }
        public string Mdp { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string Key { get; set; }
        public string HA { get; set; }
        public byte[] Salt { get; set; }
        public string ChallengeUser { get; set; }
    }
}
