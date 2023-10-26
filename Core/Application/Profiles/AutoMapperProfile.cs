using Humteria.Application.DTOs.UserDTO;
using Humteria.Application.DTOs.UserDTO.Request;
using Humteria.Application.DTOs.UserDTO.Response;
using Humteria.Data.Models;

namespace Humteria.Application.Profiles;

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

