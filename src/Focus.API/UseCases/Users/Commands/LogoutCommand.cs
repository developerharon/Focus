using Focus.API.Entities;
using Focus.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Focus.API.UseCases.Users.Commands
{
    public class LogoutCommand : IRequest<ResponseDTO<string>>
    {
        private readonly HttpContext HttpContext;

        public LogoutCommand(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseDTO<string>>
        {
            private readonly UserManager<UserEntity> _userManager;

            public LogoutCommandHandler(UserManager<UserEntity> userManager)
            {
                _userManager = userManager;
            }

            public async Task<ResponseDTO<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.HttpContext?.User?.Identity?.Name ?? "");

                if (user == null)
                    return ResponseDTO<string>.Create(Shared.Enums.ResponseType.Error, null, "Errror logging out user");

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
                return ResponseDTO<string>.Create(Shared.Enums.ResponseType.Success, user.UserName);
            }
        }
    }
}