using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos.Authentication;
using BloodBank.Data.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using BCrypt.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BloodBank.Data.Entities;

namespace BloodBank.Service.Utils.Authentication
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "awdkKAaSDsfaDFsdfSawrDKASDsuEgFKAsdaDoAsdFWwAkd129fASDKasfASL";

        private const int JWT_TOKEN_VALIDITY_MINS = 120;
        private readonly BloodBankContext _db;

        public JwtTokenHandler(BloodBankContext db)
        {
            _db = db;
        }
        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password)) return null;
            dynamic userAccount = null;
            //Validation

            if (request.Role == Role.Donor)
            {
                userAccount = _db.Donors.Where(d => d.Username == request.UserName).FirstOrDefault();
            }
            else if (request.Role == Role.Hospital)
            {
                userAccount = _db.Hospitals.Where(d => d.Username == request.UserName).FirstOrDefault();
            }
            //else
            //{
            //    userAccount = _db..Where(us => us.Email == request.UserName
            //                                       && us.Password == request.Password).FirstOrDefault();
            //}
            if (userAccount == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(request.Password, userAccount.Password)) return null; 
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                new Claim("Role", request.Role.ToString())
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserId = userAccount.Id,
                FullName = userAccount.FullName,
                Username = userAccount.Username,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token
            };

        }
        
    }
}
