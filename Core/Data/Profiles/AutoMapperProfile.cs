using AutoMapper;
using Humteria.Data.DTOs.UserDTO;
using Humteria.Data.DTOs.UserDTO.Request;
using Humteria.Data.DTOs.UserDTO.Response;
using Humteria.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Humteria.Data.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        // Users
        CreateMap<RegisterRequestDTO, User>();
        CreateMap<User, LoginResponseDTO>();
        CreateMap<User, RegisterResponseDTO>();
        CreateMap<LoginResponseDTO, User>();

        // SQL
        CreateMap<User, JWTUserForTokenDTO>();
        CreateMap<JWTUserForTokenDTO, User>();
        CreateMap<JWTUserForTokenDTO, LoginResponseDTO>();
        CreateMap<JWTUserForTokenDTO, RegisterResponseDTO>();
        CreateMap<LoginResponseDTO, JWTUserForTokenDTO>();
    }

    
}

