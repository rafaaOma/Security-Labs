using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq; 
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AccountController :  ControllerBase
{
     private readonly UserManager<IdentityUser> _userManager; //for user data managament and lifecycle operations
     private readonly SignInManager<IdentityUser> _signInManager;//for handling user sign-in and authentication processes, such as signing in users, signing them out, and managing their authentication state.

 public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
{
    _userManager = userManager;
    _signInManager = signInManager;
}


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);//The CreateAsync method of the UserManager class is called to create a new user with the specified password. The result of this operation is stored in the result variable, which contains information about whether the user creation was successful and any errors that may have occurred during the process.

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { message = "Registration failed", errors });
        }

        return Ok(new { message = "User registered successfully" });
    }
    // POST: api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _signInManager.PasswordSignInAsync(//The PasswordSignInAsync method of the SignInManager class is called to attempt to sign in the user with the provided email and password. The method takes the email, password, a boolean indicating whether to remember the user (for persistent cookies), and a flag for lockout on failure. The result of this operation is stored in the result variable, which contains information about whether the login was successful and any errors that may have occurred during the process.
            model.Email,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid login attempt." });

        return Ok(new { message = "Login successful" });
    }
}


public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}