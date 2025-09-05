using ContactManagment.DTOs;
using ContactManagment.DTOs.GlobaResponse;
using ContactManagment.Helper;
using ContactManagment.Models;
using ContactManagment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly JWTTokenService tokenService;

        public AccountController(UserManager<User> userManager, IConfiguration configuration, JWTTokenService tokenService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if(ModelState.IsValid)
            {
                User user = new User()
                {
                    Email = register.Email,
                    UserName = register.FullName
                };
                IdentityResult created = await userManager.CreateAsync(user, register.Password);
                if(created.Succeeded)
                    return Ok(ApiResponse<RegisterDTO>.SuccessResponse(register, "Registered Successfully"));

                foreach (var er in created.Errors)
                {
                    ModelState.AddModelError("Password", er.Description);
                }
            }
            return BadRequest(ApiResponse<string>.FailedResponse("Validaton Error", ModelState.GetErrors()));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var targetUser = await userManager.FindByEmailAsync(loginDto.Email);
                if (targetUser is null)
                    return BadRequest(ApiResponse<string>.FailedResponse("Invalid Email", ModelState.GetErrors()));
                bool IsMatched = await userManager.CheckPasswordAsync(targetUser, loginDto.Password);

                if (IsMatched)
                {
                    // generate token, in DTO 
                    JwtTokenDTO jwtTokenDTO = await tokenService.GenerateToken(targetUser);

                    return Ok(
                        ApiResponse<object>.SuccessResponse(jwtTokenDTO, "Loggedin Succefully")
                        );
                }
                return BadRequest(ApiResponse<string>.FailedResponse("Invalid Credentials", ModelState.GetErrors()));

            }
            return BadRequest(ApiResponse<string>.FailedResponse("Validation Error", ModelState.GetErrors()));
        }
    }
}
