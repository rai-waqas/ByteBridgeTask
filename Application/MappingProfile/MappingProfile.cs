using AutoMapper;
using Business.Dtos;
using DataAccess.Dtos;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using File = DataAccess.Entities.File;

namespace Business.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<Clientdetail, ClientDetailsDto>().ReverseMap();
            CreateMap<Clientdetail, AddClientDetailsDto>().ReverseMap();
            CreateMap<File, FileDto>().ReverseMap();
            CreateMap<IFormFile, File>()
            .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Filedata, opt => opt.MapFrom(src => GetFileData(src)));
        }
        private static byte[] GetFileData(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
