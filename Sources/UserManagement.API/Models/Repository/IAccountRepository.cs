﻿using ModelLibrary;
using UserManagement.API.Models.Data;

namespace UserManagement.API.Models.Repository
{
    public interface IAccountRepository
    {
        Task<Result<Account>> CreateAsync(Account account);
        Task<Result<Account>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Account>>> GetAllAsync();
        Task<Result<Account>> UpdateAsync(Account account);
        Task<Result<bool>> DeleteAsync(Account account);
        Task<Result<object>> LoginAsync(LoginRequest login);
        Task<Result<Account>> ChangePasswordAsync(ChangePasswordRequest changePassword);
        Task<Result<string>> GetResetPasswordCodeAsync(string email);
        Task<Result<Account>> ResetPasswordAsync(ResetPasswordRequest resetPassword);
        Task<Result<Account>> ConfirmAccountAsync(ConfirmAccountRequest confirmAccount);
        Task<Result<string>> ResendOtpAsync(string email);
        Task<Result<Account>> GetDetailsAsync(string email);
    }
}