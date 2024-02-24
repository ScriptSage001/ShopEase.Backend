﻿using ShopEase.Backend.AuthService.Application.Helper;
using ShopEase.Backend.AuthService.Application.Models;
using ShopEase.Backend.AuthService.Core.Primitives;

namespace ShopEase.Backend.AuthService.Application.Commands
{
    /// <summary>
    /// Command to Register User
    /// </summary>
    /// <param name="userRegister"></param>
    public record RegisterUserCommand(UserRegisterDto userRegister) : ICommandAsync
    {
        /// <summary>
        /// User Register DTO
        /// </summary>
        public UserRegisterDto UserRegister { get; set; } = userRegister;

        /// <summary>
        /// Result
        /// </summary>
        public Result<string> Result { get; set; }
    }
}
