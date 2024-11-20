using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTTokenAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private User[] _users = [
            //Можно только в бухгалтерию
            new User{
                Id = 1,
                Login = "buchgalter",
                Password = "12345",
                Role = "buchgalter"
            },
            //Можно в бухгалтерию и в маркетологию
            new User{
                Id = 2,
                Login = "marketolog",
                Password = "12345",
                Role = "marketolog"
            },
            //Можно везде
            new User{
                Id = 3,
                Login = "admin",
                Password = "12345",
                Role = "admin"
            },
            //Можно только публичную точку входа
            new User{
                Id = 4,
                Login = "user",
                Password = "12345",
                Role = "user"
            },
        ];


        public AuthController(){}

        [HttpGet]
        public ActionResult<ResponseTokenAndRole> Login(string login, string password){
            
            // Ищем пользователя и вытягиваем всё, что необходимо положить в полезную нагрузку для токена
            User? user = _users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if(user is null)
                return Unauthorized();

            string role = user.Role;
            int id = user.Id;

            // Создаём полезную нагрузку для токена
            var claims = new List<Claim> {
                //Кладём Id (если нужно)
                new Claim(ClaimValueTypes.Integer32, id.ToString()),
                //Кладём роль
                new Claim(ClaimTypes.Role, role)
            };
            
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    //кладём полезную нагрузку
                    claims: claims,
                    //устанавливаем время жизни токена 2 минуты
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new  ResponseTokenAndRole{
                Token = token,
                Role = role
            });
        }
    }

    public class ResponseTokenAndRole{
        public string Token { get; set;}
        public string Role { get; set;}
    }

    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }

    public class User {
        public int Id { get; set; }
        public string Login { get; set;}
        public string Password { get; set;}
        public string Role {get; set;} //Представим, что это объект роли, из которой ты вытянешь название
    }
}