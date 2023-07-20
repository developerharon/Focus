using Focus.API.Entities;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Focus.API.UseCases.Users.Commands
{
    public class CreateUserCommand : IRequest<ResponseDTO<string>>
    {
        private readonly CreateUserDTO DTO;

        public CreateUserCommand(CreateUserDTO dto)
        {
            DTO = dto;
        }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseDTO<string>>
        {
            private readonly UserManager<UserEntity> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public CreateUserCommandHandler(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<ResponseDTO<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.DTO.UserName);

                if (user != null)
                    return ResponseDTO<string>.Create(Shared.Enums.ResponseType.Error, "User already exists");

                user = new UserEntity
                {
                    UserName = request.DTO.UserName,
                    Name = request.DTO.Name,
                    Email = request.DTO.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.DTO.Password);

                if (result.Succeeded)
                    return ResponseDTO<string>.Create(Shared.Enums.ResponseType.Success, user.UserName);
                return ResponseDTO<string>.Create(Shared.Enums.ResponseType.Error, GenerateErrorMessage(result));
            }

            private string GenerateErrorMessage(IdentityResult result)
            {
                var sb = new StringBuilder();

                foreach (var error in  result.Errors)
                {
                    sb.Append($"{error.Code} ,");
                }
                sb.Remove(sb.Length - 3, 1);
                return sb.ToString();
            }
        }
    }
}