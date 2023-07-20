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
    public class RefreshTokenCommand : IRequest<ResponseDTO<LoginResponseDTO>>
    {
        private readonly RefreshTokenDTO DTO;

        public RefreshTokenCommand(RefreshTokenDTO dto)
        {
            DTO = dto;
        }

        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseDTO<LoginResponseDTO>>
        {
            private readonly UserManager<UserEntity> _userManager;
            private readonly JwtTokenServices _tokenServices;
            private readonly JWTConfiguration _jwtConfig;
            public RefreshTokenCommandHandler(UserManager<UserEntity> userManager, JwtTokenServices jwtTokenServices, JWTConfiguration jwtConfig)
            {
                _userManager = userManager;
                _tokenServices = jwtTokenServices;
                _jwtConfig = jwtConfig;
            }

            public async Task<ResponseDTO<LoginResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                var principal = _tokenServices.GetPrincipalFromExpiredToken(request.DTO.AccessToken);
                var username = principal?.Identity?.Name?? "";

                var user = await _userManager.FindByNameAsync(username);

                if (user == null
                        || user.RefreshToken != request.DTO.RefreshToken
                        // It should be true if the value on the left is earlier than or equal to the current time
                        || user.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
                    return ResponseDTO<LoginResponseDTO>.Create(Shared.Enums.ResponseType.Error, null, "Invalid credentials");

                var signingCredentials = _tokenServices.GetSigningCredentials();
                var claims = await _tokenServices.GetClaims(user);
                var tokenOptions = _tokenServices.GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                user.RefreshToken = _tokenServices.GenerateRefreshToken();
                // TOD0: Remove magic number same to LoginCommand
                user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddDays(_jwtConfig.RefreshTokenDurationInDays ?? 7);

                await _userManager.UpdateAsync(user);

                var responseDto = new LoginResponseDTO(token, user.RefreshToken);
                return ResponseDTO<LoginResponseDTO>.Create(Shared.Enums.ResponseType.Success, responseDto);
            }
        }
    }
}