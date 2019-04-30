using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWTauthentication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTauthentication.Controllers
{
    //Create an API that validates a user and issues a JWT
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SecurityController (IConfiguration configuration)
        {
            _config = configuration;
        }

        //We need some mechanism that validates a user name and password.We will go with an API that does that for us.So, add a new API controller called SecurityController in the Controllers folder.
        //The SecurityController will have two private helper methods and a public action.

        //The first private helper method is called GenerateJWT() and it generates a JWT token for us.The token is then sent to the client.The GenerateJWT() method is shown below:

        private string GenerateJWT()
        {
            var stringToken = string.Empty;
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer,
                                            audience: audience,
                                            expires: DateTime.Now.AddMinutes(120),
                                            signingCredentials: credentials);
            //The above code retrieves the issuer, audience, and key from the configuration file. It then creates a new SymmetricSecurityKey based on the Key. 
            //SigningCredentials object is then generated based on the SymmetricSecurityKey. Notice that we use HS256 algorithm while generating the digital signature.
            //Now we can move ahead and create a JWT token. This is done using JwtSecurityToken class. We pass the issuer, audience, an expiry DateTime for the token, and the signing credentials in the constructor.
            // We want the JWT in a string form so that it can be easily sent to the client. This is done using JwtSecurityTokenHandler class. The WriteToken() method accepts a JwtSecurityToken created earlier and returns it as a JSON compact serialized format string.
            var tokenHandler = new JwtSecurityTokenHandler();
            stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }


        //Inside, it checks the user name and password. In the above example, we simply check against hard-coded values. But you could use ASP.NET Core Identity or any custom technique to validate a user.
        //If the user credentials are valid we return true, otherwise we return false. Instead of returning true you could have also returned user details such as user name and roles.
        private bool ValidateUser(User loginDetails)
        {
            if (loginDetails.UserName == "admin" &&  loginDetails.Password == "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Finally, we need Login() action that will invoke ValidateUser() and GenerateJWT() helper methods.
        //Note that Login() action of SecurityController is marked with [HttpPost] attribute. It accepts User object as the parameter. The Login() action will be invoked by the client application and the client, 
        //by some means, will supply the User details such as user name and password.

        // Inside, we call ValidateUser() helper method to check whether the user name and password are valid.If user credentials are valid, we call GenerateJWT() to generate a JWT token. 
        //The string token is returned to the client with HTTP status of Ok (status code - 200).

        [HttpPost]
        public IActionResult Login([FromBody]User loginDetails)
        {
            bool result = ValidateUser(loginDetails);
            if (result)
            {
                var tokenstring = GenerateJWT();
                return Ok(new { token = tokenstring });
            }
            else
            {
                return Unauthorized();
            }
        }
    }

   
}