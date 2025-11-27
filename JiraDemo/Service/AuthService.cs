using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Models;

namespace WebApplication3.Service;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<AuthResultDto> RegistrAsync(RegisterDTO register)
    {
        if (await _context.Users.AnyAsync(x => x.Email == register.email))
        {
            throw new Exception("Email already exists");
        }

        var user = new User
        {
            FullName = register.fullname,
            Email = register.email,
            PasswordHash = hashPassword(register.password),
            Role = "User"
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var token = GenerateToken(user);
        return new AuthResultDto(token);

    }

    public async Task<AuthResultDto> LoginAsync(LoginDTO login)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == login.email);
        if (user == null || !verifyPassword(login.password, user.PasswordHash))
        {
            throw new Exception("Wrong password");
        }
        var token = GenerateToken(user);
        return new AuthResultDto(token);
    }

      public bool verifyPassword(string password, string storedHash)
        {
            // Extract the salt and hash from the stored hash
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            // Hash the input password with the same salt
            var hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the hashes
            return hash == hashedInput;
        }




        public string hashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[16]; 
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // Hash the password with the salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Return the salt and hash concatenated
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }
    
    
    private string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "super_secret_key_12345super_secret_key_12345");
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}