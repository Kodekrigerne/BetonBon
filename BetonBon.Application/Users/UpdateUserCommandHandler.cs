using BetonBon.Domain.Users;

namespace BetonBon.Application.Users
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }


        public async Task HandleAsync(UpdateUserCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.Id)
                ?? throw new Exception("User not found");

            user.Update(command.Username, command.Role);

            if (!string.IsNullOrWhiteSpace(command.Password))
            {
                user.SetPassword(command.Password, _passwordHasher);
            }


            await _userRepository.SaveChangesAsync();
        }
    }
}
