using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dotnet7_webapi_jwt.Data;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel userModel)
    {
        var user = await _userManager.FindByEmailAsync(userModel.Email!);
        if (user != null && await _userManager.CheckPasswordAsync(user, userModel.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = CreateToken(claims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email!);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                Status = "Error", Message = "User already exists!" 
            });

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                Status = "Error", Message = "User creation failed! Please check user details and try again." 
            });

        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email!);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                Status = "Error", Message = "User already exists!" 
            });

        IdentityUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await _userManager.CreateAsync(user, model.Password!);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                Status = "Error", Message = "User creation failed! Please check user details and try again." 
            });

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.User);
        }
        return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    private JwtSecurityToken CreateToken(List<Claim> claims)
    {
        var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetValue<string>("JwtSettings:Secret")));    // _configuration.GetSection("JwtSettings:Secret").Value)

        var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(   // 亦可使用　SecurityTokenDescriptor　來産生 Token
            issuer: _configuration.GetValue<string>("JwtSettings:ValidIssuer"),
            audience: _configuration.GetValue<string>("JwtSettings:ValidAudience"),
            expires: DateTime.Now.AddDays(1),
            claims: claims,
            signingCredentials: credentials);

        return token;
    }
}