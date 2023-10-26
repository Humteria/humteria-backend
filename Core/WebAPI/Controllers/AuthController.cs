using AutoMapper;
using Humteria.WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Humteria.WebAPI.Controllers;

[Route("api/auth")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMapper m_mapper;
    private readonly IMainInterface m_repository;
    private readonly IJwtGenerator m_jwtGenerator;

    public AuthController(IJwtGenerator jwtGenerator, IMainInterface repository, IMapper mapper)
    {
        m_jwtGenerator = jwtGenerator;
        m_mapper = mapper;
        m_repository = repository;
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

        User? mailAdreadyExists = await m_repository.GetUserByMail(userRegisterRequest.Email);
        User? usernameAlreadyExists = await m_repository.GetUserByUsername(userRegisterRequest.Username);
        if (usernameAlreadyExists != null || mailAdreadyExists != null)
        {
            return BadRequest(registerError);
        }
           
        User userToAddToDb = m_mapper.Map<User>(userRegisterRequest);
        if (userToAddToDb == null)
        {
            return BadRequest(registerError);
        }
        User userToAddToDB = m_mapper.Map<User>(userRegisterRequest);
        userToAddToDB.Password = PasswordHelper.HashPassword(userRegisterRequest.Password);

        User? responseAddUser = await m_repository.RegisterNewUser(userToAddToDB);
        if (responseAddUser != null && m_repository.SaveChanges())
        {
            string token = m_jwtGenerator.GenerateToken(m_mapper.Map<JWTUserForTokenDTO>(responseAddUser), 1);
            RegisterResponseDTO userToReturn = m_mapper.Map<RegisterResponseDTO>(responseAddUser);
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
        User? userFromDB = await m_repository.GetUserByUsername(userLoginRequest.UsernameOrMail);
        if (userFromDB == null || !PasswordHelper.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
        {
            userFromDB = await m_repository.GetUserByMail(userLoginRequest.UsernameOrMail);
            if (userFromDB == null || !PasswordHelper.CompareHashAndPassword(userFromDB.Password, userLoginRequest.Password))
            {
                return Unauthorized("Invalid Username or Password");
            }
        }

        LoginResponseDTO loginResponseDTO = m_mapper.Map<LoginResponseDTO>(userFromDB);
        JWTUserForTokenDTO userForTokenDTO = m_mapper.Map<JWTUserForTokenDTO>(userFromDB);
        loginResponseDTO.Token = m_jwtGenerator.GenerateToken(userForTokenDTO);
  
        Thread.Sleep(PasswordHelper.GenerateRandomInt(1, 1000));
        return Ok(loginResponseDTO);
    }
}
