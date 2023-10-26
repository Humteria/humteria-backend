﻿using AutoMapper;
using Humteria.Data.DTOs.UserDTO;
using Humteria.Data.DTOs.UserDTO.Request;
using Humteria.Data.DTOs.UserDTO.Response;
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
    private readonly IConfigurationRoot _configuration;
    private readonly IMapper _mapper;
    private readonly IMainInterface _repository;

    public AuthController(IMainInterface repository, IMapper mapper)
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        _mapper = mapper;
        _repository = repository;

        if (_configuration == null || _mapper == null || _repository == null)
        {
            throw new ArgumentNullException();
        }
    }

    [HttpPost, Route("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDTO userRegisterRequest)
    {
        ProblemDetails registerError = new ProblemDetails
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
        userToAddToDB.Password = PasswordHasher.HashPassword(userRegisterRequest.Password);

        User? responseAddUser = await _repository.RegisterNewUser(userToAddToDB);
        if (responseAddUser != null && _repository.SaveChanges())
        {
            string token = JwtTokens.GenerateToken(_mapper.Map<JWTUserForTokenDTO>(responseAddUser), 1);
            RegisterResponseDTO userToReturn = _mapper.Map<RegisterResponseDTO>(responseAddUser);
            userToReturn.Token = token;
            return Ok(userToReturn);
        }
        return BadRequest(registerError);
    }

    [HttpPost, Route("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO userLoginRequest)
    {
        ProblemDetails loginError = new ProblemDetails
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
        if (userFromDB == null || !PasswordHasher.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
        {
            userFromDB = await _repository.GetUserByMail(userLoginRequest.UsernameOrMail);
            if (userFromDB == null || !PasswordHasher.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
            {
                return Unauthorized("Invalid Username or Password");
            }
        }

        LoginResponseDTO loginResponseDTO = _mapper.Map<LoginResponseDTO>(userFromDB);
        JWTUserForTokenDTO userForTokenDTO = _mapper.Map<JWTUserForTokenDTO>(userFromDB);
        loginResponseDTO.Token = JwtTokens.GenerateToken(userForTokenDTO);
  
        Thread.Sleep(PasswordHasher.GenerateRandomInt(1, 1000));
        return Ok(loginResponseDTO);
    }
}
