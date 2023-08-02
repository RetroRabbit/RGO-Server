using RGO.Domain.Models;

namespace RGO.Repository.Entities
{
    public class Social
    {
        public int id { get; set; }
        public string discord { get; set; } = null!;
        public string codeWars{ get; set; } = null!;
        public string gitHub { get; set; } = null!;
        public string linkedIn { get; set; } = null!;

        public Social()
        {

        }

        public Social(SocialDto social)
        {
            id = social.id;
            discord = social.discord;
            codeWars = social.codeWars;
            gitHub  = social.gitHub;
            linkedIn = social.linkedIn;
        }

        public SocialDto ToDTO()
        {
            return new SocialDto(
                id,
                discord,
                codeWars,
                gitHub,
                linkedIn
                );
        }
    }
}
