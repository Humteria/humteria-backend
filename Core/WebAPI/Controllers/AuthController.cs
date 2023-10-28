using AutoMapper;
using Humteria.Application.DTOs.UserDTO;
using Humteria.Application.DTOs.UserDTO.Request;
using Humteria.Application.DTOs.UserDTO.Response;
using Humteria.Data.Models;
using Humteria.Data.Services;
using Humteria.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Humteria.WebAPI.Controllers;

[Route("api/auth")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMainInterface _repository;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthController(IJwtGenerator jwtGenerator, IMainInterface repository, IMapper mapper)
    {
        _jwtGenerator = jwtGenerator;
        _mapper = mapper;
        _repository = repository;
    }

    [HttpPost, Route("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDTO userRegisterRequest)
    {
        ProblemDetails registerError = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "Bad Request",
            Title = "Error while registering new user",
            Detail = "There was an error while registering the new user."
        };

        if (!ModelState.IsValid)
        {
            return BadRequest(registerError);
        }

        User? mailAdreadyExists = await _repository.GetUserByMail(userRegisterRequest.Email);
        User? usernameAlreadyExists = await _repository.GetUserByUsername(userRegisterRequest.Username);
        if (usernameAlreadyExists != null || mailAdreadyExists != null)
        {
            return BadRequest(registerError);
        }
           
        User userToAddToDb = _mapper.Map<User>(userRegisterRequest);
        if (userToAddToDb == null)
        {
            return BadRequest(registerError);
        }
        User userToAddToDB = _mapper.Map<User>(userRegisterRequest);
        userToAddToDB.Password = PasswordHelper.HashPassword(userRegisterRequest.Password);

        User? responseAddUser = await _repository.RegisterNewUser(userToAddToDB);
        if (responseAddUser != null && _repository.SaveChanges())
        {
            string token = _jwtGenerator.GenerateToken(_mapper.Map<JWTUserForTokenDTO>(responseAddUser), 1);
            RegisterResponseDTO userToReturn = _mapper.Map<RegisterResponseDTO>(responseAddUser);
            userToReturn.Token = token;
            return Ok(userToReturn);
        }
        return BadRequest(registerError);
    }

    [HttpPost, Route("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO userLoginRequest)
    {
        ProblemDetails loginError = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "Bad Request",
            Title = "Error during login of user",
            Detail = "There was an error during the login."
        };

        if (!ModelState.IsValid)
        {
            return BadRequest(loginError);
        }
        User? userFromDB = await _repository.GetUserByUsername(userLoginRequest.UsernameOrMail);
        if (userFromDB == null || !PasswordHelper.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
        {
            userFromDB = await _repository.GetUserByMail(userLoginRequest.UsernameOrMail);
            if (userFromDB == null || !PasswordHelper.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
            {
                return Unauthorized("Invalid Username or Password");
            }
        }

        LoginResponseDTO loginResponseDTO = _mapper.Map<LoginResponseDTO>(userFromDB);
        JWTUserForTokenDTO userForTokenDTO = _mapper.Map<JWTUserForTokenDTO>(userFromDB);
        loginResponseDTO.Token = _jwtGenerator.GenerateToken(userForTokenDTO);
  
        Thread.Sleep(PasswordHelper.GenerateRandomInt(1, 1000));
        return Ok(loginResponseDTO);
    }
}
