using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using SERVICE.Logic;
using SERVICE.Dao.Contracts;
using SERVICE.Dao.Helpers;

namespace Services.Facade
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILoggerService _loggerService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ILoggerService loggerService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _loggerService = loggerService;
        }

        public bool Login(string username, string password)
        {
            var user = _userRepository.FindByUsername(username);
            if (user == null)
            {
                _loggerService.Log($"Intento de login fallido: Usuario {username} no encontrado.");
                return false;
            }

            // Usar PasswordHelper para validar la contraseña
            var isValidPassword = PasswordHelper.ValidatePassword(password, user.Password);
            if (!isValidPassword)
            {
                _loggerService.Log($"Intento de login fallido: Contraseña incorrecta para el usuario {username}.");
                return false;
            }

            _loggerService.Log($"Usuario {username} ha iniciado sesión con éxito.");
            return true;
        }

        public void Register(string username, string password, int roleId)
        {
            var passwordHash = PasswordHelper.EncryptPassword(password);
            var newUser = new Usuario
            {
                UserName = username,
                Password = passwordHash,
                timestamp = DateTime.Now
            };

            _userRepository.Add(newUser);
            _loggerService.Log($"Nuevo usuario {username} registrado con el rol {roleId}.");
        }

        public IEnumerable<Usuario> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public Usuario GetUserById(Guid userId)
        {
            return _userRepository.FindById(userId);
        }
    }
}
