using ShopEase.Backend.AuthService.Application.Commands;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Core.Entities;

namespace ShopEase.Backend.AuthService.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : ICommandHandlerAsync<RegisterUserCommand>
    {
        private readonly IAuthHelper _authHelper;

        public RegisterUserCommandHandler(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User user = new(new Guid(), request.UserRegister.Name, request.UserRegister.Email, request.UserRegister.MobileNumber, request.UserRegister.AltMobileNumber);

            _authHelper.CreatePasswordHash(request.UserRegister.Password, out byte[] passwordHash, out byte[] passwordSalt);

            return Task.CompletedTask;

            // User user = new() 
            // {
            //     Name = request.User.Name,
            //     Email = request.User.Email,
            //     MobileNumer = request.User.MobileNumer,
            //     AltMobileNumber = request.User.AltMobileNumber,
            //     HintNameAltNumer = request.User.HintNameAltNumer
            // };           
            // var userId = _userRepository.InsertUserDetails(user);

            // if (userId > 0)
            // {
            //     AuthUserDetails authUser = new()
            //     {
            //         UserId = userId,
            //         PasswordHash = request.User.PasswordHash,
            //         PasswordSalt = request.User.PasswordSalt
            //     };
            //     _userRepository.InsertUserAuthDetails(authUser);

            //     return Task.CompletedTask;
            // }

            // return Task.FromException(new Exception("User Already Exists!"));
        }
    }
}
