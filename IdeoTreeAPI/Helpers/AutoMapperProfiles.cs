using AutoMapper;
using IdeoTreeAPI.DTOs;
using IdeoTreeAPI.Model;

namespace IdeoTreeAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TreeNodeDB,NodeDTO>().ReverseMap();
        }

    }
}
