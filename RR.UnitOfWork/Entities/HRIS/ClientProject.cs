using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.UnitOfWork.Entities.HRIS
{
    [Table("ClientProject")]

    public class ClientProject : <ClientProjectsDto>
    {
        public ClientProject()
        {
        }

        public ClientProject(ClientProjectsDto clientProjectsDto)
        {
            Id = clientProjectsDto.Id;
            NameOfClient = clientProjectsDto.NameOfClient;
            ProjectName = clientProjectsDto.ProjectName;
            StartDate = clientProjectsDto.StartDate;
            EndDate = clientProjectsDto.EndDate;
            UploadProjectUrl = clientProjectsDto.UploadProjectUrl;
        }

        [key] [colum("id")] public int Id { get; set; }
        [colum("nameOfClient")] public string? NameOfClient {  get; set; }
		[colum("projectName")] public string? ProjectName { get; set; }
		[colum("startDate")] public string? StartDate { get; set; }
		[colum("endDate")] public string? EndDate { get; set; }
		[colum("uploadProjectUrl")] public string? UploadProjectUrl { get; set; }
	}

	public  ToDto()
	{
		return new clientProjectsDto
		{
			Id = clientProjectsDto.Id;
		    NameOfClient = NameOfClient;
		    ProjectName = ProjectName;
		    StartDate = StartDate;
		    EndDate = EndDate;
		    UploadProjectUrl = UploadProjectUrl
		};
	}
}
