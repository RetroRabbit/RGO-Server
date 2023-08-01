﻿using RGO.Domain.Models;
using System.Text.RegularExpressions;

namespace RGO.Repository.Entities
{
    public class Social
    {
        public int id { get; set; }
        public string discord { get; set; } = null!;
        public string codewars{ get; set; } = null!;
        public string github { get; set; } = null!;
        public string linkedin { get; set; } = null!;

        public Social()
        {

        }

        public Social(SocialDto social)
        {
            id = social.id;
            discord = social.discord;
            codewars = social.codewars;
            github  = social.github;
            linkedin = social.linkedin;
        }

        public SocialDto ToDTO()
        {
            return new SocialDto(
                id,
                discord,
                codewars,
                github,
                linkedin);
        }
    }
}
