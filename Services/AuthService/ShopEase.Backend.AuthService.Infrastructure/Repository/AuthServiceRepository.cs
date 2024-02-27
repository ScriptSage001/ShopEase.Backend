using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Core.Entities;

namespace ShopEase.Backend.AuthService.Infrastructure
{
    /// <summary>
    /// AuthService Repository
    /// </summary>
    public class AuthServiceRepository : IAuthServiceRepository
    {
        #region Variables

        /// <summary>
        /// Instance of AuthDbContext
        /// </summary>
        private readonly AuthDbContext _authDbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AuthServiceRepository
        /// </summary>
        /// <param name="authDbContext"></param>
        public AuthServiceRepository(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To Create New User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CreateUser(User user)
        {
            var existingUser = _authDbContext.User.Any(u => u.Email == user.Email);
            
            if (!existingUser)
            {
                _authDbContext.User.Add(user);
                _authDbContext.SaveChanges();
                return 1;
            }
            return -1;           
        }

        /// <summary>
        /// To Create New User Credentials
        /// </summary>
        /// <param name="userCredentials"></param>
        public void CreateUserCredentials(UserCredentials userCredentials)
        {
            _authDbContext.UserCredentials.Add(userCredentials);
            _authDbContext.SaveChanges();
        }

        /// <summary>
        /// To fetch UserCredentials
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserCredentials? GetUserCredentials(string email)
        {
            return _authDbContext.UserCredentials.FirstOrDefault(x => x.Email == email);
        }

        /// <summary>
        /// To fetch UserCredentials
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserCredentials? GetUserCredentials(Guid userId)
        {
            return _authDbContext.UserCredentials.FirstOrDefault(x => x.UserId == userId);
        }

        /// <summary>
        /// To Update UserCredentials
        /// </summary>
        /// <param name="userCredentials"></param>
        public void UpdateUserCredentials(UserCredentials userCredentials)
        {
            _authDbContext.UserCredentials.Update(userCredentials);
            _authDbContext.SaveChanges();
        }

        #endregion
    }

}