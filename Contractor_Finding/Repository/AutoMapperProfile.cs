using Domain.Models;
using AutoMapper;
using Domain;

namespace Repository
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Userview, UserDisplayV2>();
        }
    }
}