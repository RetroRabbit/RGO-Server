using RGO.Domain.Models;


namespace RGO.Repository.Entities
{
    public class Skill
    {
        public int id { get; set; }
        public User user { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public Skill()
        {

        }

        public Skill(SkillDto skill)
        {
            id = skill.id;
            userId = skill.userId;
            title = skill.title;
            description = skill.description;
        }

        public SkillDto ToDTO()
        {
            return new SkillDto 
            (
                id,
                userId,
                title,
                description
            );
        }


    }
}
