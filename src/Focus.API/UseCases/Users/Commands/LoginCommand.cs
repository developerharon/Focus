using Focus.API.Configuration;
using Focus.API.Entities;
using Focus.API.Services;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Focus.API.UseCases.Users.Commands
{
    public class LoginCommand : IRequest<ResponseDTO<LoginResponseDTO>>
    {
        private readonly LoginRequestDTO DTO;

        public LoginCommand(LoginRequestDTO dto)
        {
            DTO = dto;
        }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseDTO<LoginResponseDTO>>
        {
            private readonly UserManager<UserEntity> _userManager;
            private readonly JwtTokenServices _tokenService;
            private readonly JWTConfiguration _jwtConfig;

            public LoginCommandHandler(UserManager<UserEntity> userManager, JwtTokenServices tokenService, JWTConfiguration jwtConfig)
            {
                _userManager = userManager;
                _tokenService = tokenService;
                _jwtConfig = jwtConfig;
            }

            public async Task<ResponseDTO<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.DTO.Email);

                if (user == null)
                    return ResponseDTO<LoginResponseDTO>.Create(Shared.Enums.ResponseType.Error, null, "Incorrect credentials");

                var isCredentialsCorrect = await _userManager.CheckPasswordAsync(user, request.DTO.Password);
                
                if (isCredentialsCorrect)
                {
                    var signingCredentials = _tokenService.GetSigningCredentials();
                    var claims = await _tokenService.GetClaims(user);
                    var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    user.RefreshToken = _tokenService.GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenDurationInDays ?? 7);

                    await _userManager.UpdateAsync(user);

                    var responseDto = new LoginResponseDTO(token, user.RefreshToken);
                    return ResponseDTO<LoginResponseDTO>.Create(Shared.Enums.ResponseType.Success, responseDto);
                }

                return ResponseDTO<LoginResponseDTO>.Create(Shared.Enums.ResponseType.Error, null, "Incorrect credentials");
            }
        }
    }
}