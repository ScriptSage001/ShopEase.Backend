﻿using ShopEase.Backend.AuthService.Application.Abstractions.ExplicitMediator;
using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command for User Login
    /// </summary>
    /// <param name="userLogin"></param>
    public sealed record LoginUserCommand(UserCredentialsDto userCredentials) : ICommand
    {
        /// <summary>
        /// UserCredentials DTO
        /// </summary>
        public UserCredentialsDto UserCredentials { get; set; } = userCredentials;
    }
}
