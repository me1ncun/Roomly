using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Auth;
using Roomly.Users.Infrastructure.Exceptions;
using Roomly.Users.Infrastructure.Helpers;
using Roomly.Users.ViewModels;

namespace Roomly.Users.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        ApplicationDbContext dbContext,
            IMapper mapper,
        ILogger<UserService> logger,
        JwtProvider jwtProvider)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _jwtProvider = jwtProvider;
    }

    public async Task<User> CreateUserAsync(RegisterViewModel registerViewModel)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerViewModel.Email);
        if (user is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        var userEntity = _mapper.Map<User>(registerViewModel);

        userEntity.Password = HashPasswordHelper.HashPassword(userEntity.Password);
        
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"User {registerViewModel.Email} has been created");
        
        return userEntity;
    }

    public async Task<string> GetUserTokenByCredentialsAsync(LoginViewModel loginViewModel)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginViewModel.Email);
        if (user is null)
            throw new EntityNotFoundException();
        
        var result = HashPasswordHelper.VerifyPassword(loginViewModel.Password, user.Password);
        if (!result)
            throw new LoginException();

        var token = _jwtProvider.GenerateToken(user);
        
        _logger.LogInformation($"User {loginViewModel.Email} has been retrieved");
        
        return token;
    }
}

public interface IUserService
{
    Task<User> CreateUserAsync(RegisterViewModel registerViewModel);
    Task<string> GetUserTokenByCredentialsAsync(LoginViewModel loginViewModel);
}
