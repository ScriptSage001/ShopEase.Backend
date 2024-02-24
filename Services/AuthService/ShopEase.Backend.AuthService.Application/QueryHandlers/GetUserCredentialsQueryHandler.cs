﻿using ShopEase.Backend.AuthService.Application.Abstractions;
using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Application.Queries;
using ShopEase.Backend.AuthService.Core.Primitives;
using static ShopEase.Backend.AuthService.Core.CustomErrors.CustomErrors;

namespace ShopEase.Backend.AuthService.Application.QueryHandlers
{
    internal sealed class GetUserCredentialsQueryHandler : IQueryHandler<GetUserCredentialsQuery, Result<UserCredentialsDto>>
    {
        #region Variables

        private readonly IAuthServiceRepository _authServiceRepository;

        #endregion

        #region Constructor

        public GetUserCredentialsQueryHandler(IAuthServiceRepository authServiceRepository)
        {
            _authServiceRepository = authServiceRepository;
        }

        #endregion

        #region Handle Method

        public Task<Result<UserCredentialsDto>> Handle(GetUserCredentialsQuery query, CancellationToken cancellationToken)
        {
            var userCred = _authServiceRepository.GetUserCredentials(query.Email);

            if (userCred == null)
            {
                return Task.FromResult(
                    Result.Failure<UserCredentialsDto>(new(LoginUserErrors.UserDoesntExists.Code, LoginUserErrors.UserDoesntExists.Message)));
            }
            else if (!userCred.RowStatus)
            {
                return Task.FromResult(
                   Result.Failure<UserCredentialsDto>(new(LoginUserErrors.UserInactive.Code, LoginUserErrors.UserInactive.Message)));
            }

            var response = Result.Success(new UserCredentialsDto()
            {
                Email = userCred.Email,
                PasswordHash = userCred.PasswordHash,
                PasswordSalt = userCred.PasswordSalt
            });

            return Task.FromResult(response);
        }

        #endregion
    }
}
