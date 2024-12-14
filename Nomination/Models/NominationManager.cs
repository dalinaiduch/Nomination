namespace Nomination.Models
{
    public class NominationManager
    {
        public int Id { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string LeadershipMember { get; set; } = string.Empty;
    }
}
