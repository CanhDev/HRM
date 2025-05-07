using ERP.Entities;
using ERP.Entities._1_Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ERP.Base_sys.jwtService
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JwtHelper(IConfiguration configuration, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public async Task<TokenModel> GenerateToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim("role", role));
            }
            //
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenDes = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDes);
            var refreshToken = GenerateRefreshToken();

            //save on Database
            var refreshTokenEntity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                JwtId = tokenDes.Id,
                UserId = user.Id,
                Token = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(30)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            return new TokenModel
            {
                accessToken = accessToken,
                refreshToken = refreshToken
            };
        }
        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public async Task<ApiRespone_basic> RefreshToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var authenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = authenKey,
            };
            //checkToken
            try
            {
                var tokenInverification = jwtTokenHandler.ValidateToken(
                    model.accessToken, tokenValidateParam, out var validatedToken);
                //1. check thuat toan
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (!result) return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "invalid SecurityAlgorithms token"
                    };
                }
                //2. check accessToken expire
                //var utcExpireDate = long.Parse(tokenInverification.Claims
                //    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                //var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                //if (expireDate > DateTime.UtcNow) return new ApiRespone_basic
                //{
                //    Success = false,
                //    Message = "AccessToken has not yet expired"
                //};
                //3. check refreshToken exist in DB
                var storedToken = await _context.RefreshTokenEntities.FirstOrDefaultAsync(
                    x => x.Token == model.refreshToken);
                if (storedToken is null) return new ApiRespone_basic
                {
                    Success = false,
                    Message = "RefreshToken does not exist"
                };
                //4. check accessTokenId match with jwtid in refreshToken?
                var jti = tokenInverification.Claims.FirstOrDefault(
                    x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (storedToken.JwtId != jti) return new ApiRespone_basic
                {
                    Success = false,
                    Message = "token dose not match"
                };
                //end check & delete old token in db
                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                _context.RefreshTokenEntities.Remove(storedToken);
                await _context.SaveChangesAsync();
                //create new token
                if (user != null)
                {
                    var tokenModel = await GenerateToken(user);
                    return new ApiRespone_basic
                    {
                        Success = true,
                        Message = "renew token Successful",
                        Data = tokenModel
                    };
                }
                else
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "invalid token: user not exist"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "invalidToken: " + ex.Message,
                };
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExprireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExprireDate).ToUniversalTime();
        }
    }
}
