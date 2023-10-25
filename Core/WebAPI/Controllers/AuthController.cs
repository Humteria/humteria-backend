﻿using AutoMapper;
using Humteria.Data.DTOs.UserDTO;
using Humteria.Data.DTOs.UserDTO.Request;
using Humteria.Data.DTOs.UserDTO.Response;
using Humteria.Data.Models;
using Humteria.Data.Services;
using Humteria.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Humteria.WebAPI.Controllers;

[Route("api/auth")]
[Authorize]
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
    //private readonly IMainInterface _repository;
    //private readonly IMapper _mapper;

    //public AuthController(IMapper mapper, IMainInterface repository)
    //{
    //    _configuration = new ConfigurationBuilder()
    //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //        .Build();
    //    _mapper = mapper;
    //    _repository = repository;

    //    if(_configuration == null || _repository == null || _mapper == null)
    //    {
    //        throw new ArgumentNullException();
    //    }
    //}



    [AllowAnonymous]
    [HttpPost, Route("test")]
    public IActionResult CreateUser()
    {
        try
        {
            return StatusCode(200, "Successful");
        }
        catch (Exception ex)
        {
            return StatusCode(400, ex.Message);
        }
        
    }

    //[AllowAnonymous]
    //[HttpPost, Route("register")]
    //public async Task<ActionResult<RegisterResponseDTO>> RegisterUser(RegisterRequestDTO userRegisterRequest)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    User? useralreadyExists = await _repository.GetUserByMail(userRegisterRequest.Email);
    //    if (useralreadyExists != null)
    //    {
    //        return BadRequest(new ProblemDetails
    //        {
    //            Status = StatusCodes.Status400BadRequest,
    //            Type = "Bad Reqeuest",
    //            Title = "Email Already in Use",
    //            Detail = "The email address provided is already associated with an existing account."
    //        });
    //    }

    //    User userToAddToDB = _mapper.Map<User>(userRegisterRequest);
    //    userToAddToDB.Password = PasswordHasher.HashPassword(userRegisterRequest.Password);

    //    User? responseAddUser = await _repository.RegisterNewUser(userToAddToDB);
    //    if (responseAddUser != null && _repository.SaveChanges())
    //    {
    //        //TODO Add mail DTO and Service

    //        string token = JwtTokens.GenerateToken(_mapper.Map<JWTUserForTokenDTO>(responseAddUser), 1);
    //        RegisterResponseDTO userToReturn = _mapper.Map<RegisterResponseDTO>(responseAddUser);
    //        userToReturn.Token = token;
    //        return Ok(userToReturn);
    //    }
    //    return BadRequest(new ProblemDetails
    //    {
    //        Status = StatusCodes.Status400BadRequest,
    //        Type = "Bad Request",
    //        Title = "Error while registering new user",
    //        Detail = "There was an error while registering the new user."
    //    });
    //}

    //[AllowAnonymous]
    //[HttpPost, Route("register")]
    //public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDTO userRegisterRequest)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    User? userAlreadyExists = await _repository.GetUserByMail(userRegisterRequest.Email);
    //    if (userAlreadyExists != null)
    //    {
    //        return BadRequest(new ProblemDetails
    //        {
    //            Status = StatusCodes.Status400BadRequest,
    //            Type = "Bad Request",
    //            Title = "Email Already in Use",
    //            Detail = "The email address provided is already associated with an existing account."
    //        });
    //    }

    //    User userToAddToDB = _mapper.Map<User>(userRegisterRequest);
    //    userToAddToDB.Password = PasswordHasher.HashPassword(userRegisterRequest.Password);

    //    User? responseAddUser = await _repository.RegisterNewUser(userToAddToDB);
    //    if (responseAddUser != null && _repository.SaveChanges())
    //    {
    //        string token = JwtTokens.GenerateToken(_mapper.Map<JWTUserForTokenDTO>(responseAddUser), 1);
    //        RegisterResponseDTO userToReturn = _mapper.Map<RegisterResponseDTO>(responseAddUser);
    //        return Ok(userToReturn);
    //    }
    //    return BadRequest(new ProblemDetails
    //    {
    //        Status = StatusCodes.Status400BadRequest,
    //        Type = "Bad Request",
    //        Title = "Error while registering new user",
    //        Detail = "There was an error while registering the new user."
    //    });
    //}
}
