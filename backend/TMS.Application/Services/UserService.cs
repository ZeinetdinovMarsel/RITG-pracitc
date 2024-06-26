
using TMS.Core.Abstractions;
using TMS.Core.Models;

namespace TMS.Application.Services;

public class UsersService : IUsersService
{

    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;
    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)

    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;

    }


    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(
            Guid.NewGuid(),
            userName,
            hashedPassword,
            email);

        await _usersRepository.Add(user);

    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result)
        {
            throw new Exception("Failed to Login");
        }

        var token = _jwtProvider.GenerateToken(user);
        return token;
    }
}