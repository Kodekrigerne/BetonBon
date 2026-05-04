using BetonBon.Application.RepositoryInterfaces;
using BetonBon.Domain.Users;

namespace BetonBon.Application.Users
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserFactory _userFactory;

        public CreateUserCommandHandler(IUserRepository userRepository, UserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<Guid> HandleAsync(CreateUserCommand command)
        {
            if (await _userRepository.UsernameExistsAsync(command.Username))
            {
                throw new ArgumentException("Username already exists.", nameof(command.Username));
            }

            var user = _userFactory.Create(command.Username, command.Password, command.Role);

            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.Id;
        }
    }
}