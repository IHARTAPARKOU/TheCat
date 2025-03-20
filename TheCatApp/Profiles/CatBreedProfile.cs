using AutoMapper;
using DataAccess.Entities;
using TheCatApp.Models;

namespace TheCatApp.Profiles;

public class CatBreedProfile : Profile
{
    public CatBreedProfile()
    {
        CreateMap<CatBreedDto, CatBreed>()
            .ForMember(_ => _.PhotoUrl, _ => _.MapFrom(src => src.Image != null ? src.Image.Url : null))
            .ReverseMap();

        CreateMap<CatBreed, CachedCatBreedDto>();
    }
}
