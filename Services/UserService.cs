using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BisHub.Models;

namespace BisHub.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {

        private UserRepository userRepo = new UserRepository();
        public User Authenticate(string username, string password)
        {
            User user = userRepo.getUserByEmailOrUsername(username);

            // return null if user not found OR password NOT Valid
            if (user == null || !user.isPasswordValid(password))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.accessToken = tokenHandler.WriteToken(token);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return userRepo.GetUsers();
        }

        public User GetById(int id)
        {
            return userRepo.GetUserById(id);
        }
    }
}