using AutoMapper;
using Humteria.Data.DTOs.UserDTO;
using Humteria.Data.DTOs.UserDTO.Request;
using Humteria.Data.DTOs.UserDTO.Response;
using Humteria.Data.Models;

namespace Humteria.Data.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        // Users
        CreateMap<RegisterRequestDTO, User>();
        CreateMap<User, LoginResponseDTO>();
        CreateMap<User, RegisterResponseDTO>();

        // SQL
        CreateMap<User, JWTUserForTokenDTO>();
        CreateMap<JWTUserForTokenDTO, User>();
        CreateMap<JWTUserForTokenDTO, RegisterResponseDTO>();
    }

    
}

