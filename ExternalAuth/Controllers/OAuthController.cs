using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
//server for external auth, simulating an OAuth provider
namespace ExternalAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        private static readonly Dictionary<string, string> AuthCodes = new();

        // Simulate the authorization endpoint
        [HttpGet("authorize")]
        public IActionResult Authorize(string response_type, string client_id, string redirect_uri, string state)
        {
            var authCode = Guid.NewGuid().ToString();
            AuthCodes[authCode] = client_id;

            return Redirect($"{redirect_uri}?code={authCode}&state={state}");//redirect back to the client with the auth code and state for validation
        }
        // Simulate the token endpoint
         [HttpPost("token")]
        public IActionResult Token([FromForm] string code, [FromForm] string client_id)//code is the auth code received from the authorize endpoint(thired party), client_id is used to validate the request
        {
            if (!AuthCodes.ContainsKey(code) || AuthCodes[code] != client_id)
                return BadRequest("Invalid code or client_id.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.MRvO3fE0o9C-pZfd3pI0hMDDXihJfQa1XPQ-UAelzaI"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ExternalAuth",//server generstes token. 
                audience: client_id,
                claims: new List<Claim> { new Claim("sub", "12345"), new Claim("name", "John Doe") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                token_type = "Bearer",
                expires_in = 1800
            });
        }
    }
}