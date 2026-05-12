using BetonBon.Application.RepositoryInterfaces;

namespace BetonBon.Application.Users
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(DeleteUserCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.Id);

            if (user != null)
            {
                _userRepository.Delete(user);

                await _userRepository.SaveChangesAsync();
            }
        }
    }
}
