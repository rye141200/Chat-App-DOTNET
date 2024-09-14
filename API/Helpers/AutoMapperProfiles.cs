using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>().
        ForMember(memberDto => memberDto.Age,
        options => options.MapFrom(appUser => appUser.DateOfBirth.CalculateAge())
        ).
        ForMember(memberDto => memberDto.PhotoUrl, options => options.MapFrom(
            appUser =>
            appUser.Photos.FirstOrDefault(photo => photo.IsMain)!.Url
            )
        );
        CreateMap<Photo, PhotoDto>();
    }
}
