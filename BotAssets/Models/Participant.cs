namespace BotAssets.Models
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool ShouldSerializeParticipantId() { return ParticipantId > 0; }
    }
}
