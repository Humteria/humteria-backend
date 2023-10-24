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
        //User
        CreateMap<RegisterRequestDTO, User>();
        // JWT
        CreateMap<LoginResponseDTO, JWTUserForTokenDTO>();
    }

    
}

