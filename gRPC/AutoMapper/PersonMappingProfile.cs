using AutoMapper;

namespace gRPC.AutoMapper
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile() {
            CreateMap<gRPC.Person, Models.Person>();
            CreateMap<Models.Person, gRPC.Person>();
        }
    }
}