using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Auth;
using Roomly.Users.Infrastructure.Exceptions;
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

    public async Task CreateUserAsync(RegisterViewModel registerViewModel)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerViewModel.Email);
        if (user is not null)
        {
            throw new EntityAlreadyExistsException();
        }
        
        var userEntity = _mapper.Map<User>(registerViewModel);
        
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"User {registerViewModel.Email} has been created");
    }

    public async Task<LoginViewModel> GetUserByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            throw new EntityNotFoundException();
        }
        
        var userViewModel = _mapper.Map<LoginViewModel>(user);

        var token = _jwtProvider.GenerateToken(user);
        
        _logger.LogInformation($"User {email} has been retrieved");
        
        userViewModel.Token = token;
        
        return userViewModel;
    }
}

public interface IUserService
{
    Task CreateUserAsync(RegisterViewModel registerViewModel);
    Task<LoginViewModel> GetUserByEmailAsync(string email);
}
