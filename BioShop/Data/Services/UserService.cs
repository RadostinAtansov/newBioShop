namespace BioShop.Data.Services
{
    using BioShop.Data.Models;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class UserService : IUserService
    {
        private readonly BioShopDataContext _dataContext;
        private readonly IConfiguration _configuration;

        public UserService(BioShopDataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<string> Login(UserDTO userRequest)
        {
            var user = await  _dataContext.Users.FirstOrDefaultAsync(n => n.Username == userRequest.Username);

            if (user == null)
            {
                return "User not found";
            }

            bool result = await VerifyPasswordHash(userRequest.Password, user.PasswordHash, user.PasswordSalt, userRequest);

            if (!result)
            {
                return "Wrong Password";
            }

            string token = CreateToken(user);

            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credential);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<UserDTO> Register(UserDTO userRequest)
        {
            CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User()
            {
                Username = userRequest.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = userRequest.Role,
            };

           await _dataContext.Users.AddAsync(newUser);
           await _dataContext.SaveChangesAsync();

            return userRequest;
        }

        private async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt, UserDTO userRequest)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(n => n.Username == userRequest.Username);

            using (HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}