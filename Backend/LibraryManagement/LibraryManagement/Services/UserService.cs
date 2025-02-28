using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;
using LibraryManagement.Interfaces;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Services;
using Newtonsoft.Json;
using LibraryManagement.Interfaces;

public class UserService : IUserService
{
    private readonly ISysUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(ISysUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<(bool IsValid, string Token, string RefreshToken, string Message)> AuthenticateUser(string userName, string password)
    {
        var user = await _userRepository.VerifyLogin(userName, password);

        if (user == null)
        {
            return (false, null, null, "Invalid username or password.");
        }

        // Lấy danh sách các quyền của người dùng từ cơ sở dữ liệu
        var permissions = CustomAuthorizeAttribute.GetAllUserFunctionsAndPermissions(userName);

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Email),
                new Claim("FunctionsAndPermissions", JsonConvert.SerializeObject(permissions)) // Lưu các quyền của từng chức năng vào JWT
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // Tạo Refresh Token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Refresh token có thời hạn 7 ngày

        // Gọi stored procedure để lưu session mới
        await _userRepository.CreateSession(user.UserID, refreshToken, refreshTokenExpiry);
        return (true, token, refreshToken, "Login successful.");
    }

    public async Task<(string Token, string RefreshToken)> RefreshToken(string refreshToken)
    {
        // Lấy session của người dùng dựa trên refresh token
        var session = await _userRepository.GetSessionByRefreshToken(refreshToken);

        // Kiểm tra xem session có tồn tại và refresh token có hết hạn hay không
        if (session == null || session.ExpiryDate <= DateTime.UtcNow || session.IsRevoked)
        {
            return (null, null); // Refresh token không hợp lệ hoặc đã hết hạn
        }

        // Lấy thông tin người dùng từ database
        var user = await _userRepository.GetByID(session.UserID);
        if (user == null)
        {
            return (null, null); // Người dùng không tồn tại
        }

        // Lấy các quyền của người dùng từ hệ thống
        var permissions = CustomAuthorizeAttribute.GetAllUserFunctionsAndPermissions(user.UserName);

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

        // Tạo lại JWT token (Access Token)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Email),
            new Claim("FunctionsAndPermissions", JsonConvert.SerializeObject(permissions)) // Quyền của người dùng
        }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var newToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // Tạo refresh token mới
        var newRefreshToken = GenerateRefreshToken();

        // Cập nhật refresh token và thời gian hết hạn mới vào cơ sở dữ liệu
        await _userRepository.UpdateSessionRefreshToken(session.SessionID, newRefreshToken, DateTime.UtcNow.AddDays(7));

        return (newToken, newRefreshToken); // Trả về access token và refresh token mới
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
