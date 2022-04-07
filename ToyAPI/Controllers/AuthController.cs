using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ToyAPI.Classes;
using ToyAPI.Models;

namespace ToyAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //bring in identity user management
        private readonly SignInManager<UserModel> signInmanager;
        private readonly UserManager<IdentityUser> userManager;
        
        //bring in token settings
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;

        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager)
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel userDetails)
        {
            //validate request
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new ErrorResponseModel { Message = "Invalid Registration Details!" });
            }

            //create user
            var identityUser = new UserModel() { UserName = userDetails.Email, Email = userDetails.Email, FirstName = userDetails.FirstName, LastName = userDetails.LastName, Role="customer"};
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);
            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new ErrorResponseModel { Message = "User Registration Failed" });
            }
            //return
            return Ok(new {Message="Reg Successful! Please Login!"});
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequestModel credentials)
        {
            

            //valid request
            if(!ModelState.IsValid || credentials == null)
            {
                return new BadRequestObjectResult(new ErrorResponseModel { Message = "Invalid Login Attempt!" });
            }

            //check user details against user manager
            var identityUser = await userManager.FindByNameAsync(credentials.Email);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);

                var user = result == PasswordVerificationResult.Failed ? null : identityUser;
                //generate token
                if (user != null)
                {
                    var token = GenerateToken(user);
                    //turn identity user into userModel
                    UserModel returnUser = (UserModel)user;
                    
                    return Ok(new AuthenticateResponseModel(returnUser, token.ToString()));
                }
            }

            //if we get here there is no user by that name/email!
            return new BadRequestObjectResult(new ErrorResponseModel { Message = "Invalid email or password!" });
           
        }

        private object GenerateToken(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };
        
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
