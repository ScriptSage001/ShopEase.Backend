using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Queries;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.QueryHandlers
{
    /// <summary>
    /// Query Handler for IsUserExistQuery
    /// </summary>
    internal sealed class IsUserExistQueryHandler : IQueryHandler<IsUserExistQuery, bool>
    {
        #region Variables

        /// <summary>
        /// Instance of IAuthServiceRepository
        /// </summary>
        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for IsUserExistQueryHandler
        /// </summary>
        /// <param name="authServiceRepository"></param>
        public IsUserExistQueryHandler(IAuthServiceRepository authServiceRepository)
        {
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handle Mehod for IsUserExistQuery
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> Handle(IsUserExistQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var isUserExists = _authServiceRepository.IsUserExists(query.Email);
                return Result.Success(isUserExists);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(new("IsUserExistsError", ex.Message));
            }
        }

        #endregion
    }
}
