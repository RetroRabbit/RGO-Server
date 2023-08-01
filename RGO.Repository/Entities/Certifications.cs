using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Entities
{
    public class Certifications
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public Certifications()
        {
            
        }

        public Certifications(CertificationsDto certifications)
        {
            id = certifications.id;
            title = certifications.title;
            description = certifications.description;
        }

        public CertificationsDto ToDTO()
        {
            return new CertificationsDto
                (
                id,
                title, 
                description
                );
        }
    }

    
    
}
