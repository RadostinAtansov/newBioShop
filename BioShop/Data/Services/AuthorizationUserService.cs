namespace BioShop.Data.Services
{
    using System.Text;
    using BioShop.Data.Models;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using BioShop.Data.Services.Interfaces;
    using BioShop.Data.ViewModels.UserModels;
    using BioShop.Config;
    using Microsoft.Extensions.Options;

    public class AuthorizationUserService : IAuthorizationUserService
    {
        private readonly BioShopDataContext _dataContext;
        //private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public AuthorizationUserService(BioShopDataContext dataContext, 
                                        IConfiguration configuration,
                                        IOptions<AppSettings> appSettings)
        {
            _dataContext = dataContext;
            //_configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public async Task<UserDTOLogin> Login(UserDTOLogin userRequest)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(n => n.Username == userRequest.Username);

            ArgumentNullException.ThrowIfNull(user, "User not found");

            bool result = await VerifyPasswordHash(userRequest.Password, user.PasswordHash, user.PasswordSalt, userRequest);

            if (!result)
            {
                throw new ArgumentException("Wrong Password");
            }

            string token = CreateToken(user);

            var loggedUser = new UserDTOLogin()
            {
                Username = user.Username,              
                Role = user.Role,
                Token = token,
               
            };

            return loggedUser;
        }

        public async Task<UserDTORegister> Register(UserDTORegister userRequest)
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
        
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Token));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credential);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt, UserDTOLogin userRequest)
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