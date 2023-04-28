﻿using Microsoft.EntityFrameworkCore;
using ModelLibrary;
using ModelLibrary.Enums;
using UserManagement.API.Enums;
using UserManagement.API.Models.Data;
using UserManagement.API.Services;

namespace UserManagement.API.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ICodeGeneratorService _codeGeneratorService;
        private readonly IEmailService _emailService;

        public AccountRepository(ApplicationDbContext context, IConfiguration configuration, IPasswordService passwordService, IJwtService jwtService, ICodeGeneratorService codeGeneratorService, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _codeGeneratorService = codeGeneratorService;
            _emailService = emailService;
        }

        public async Task<Result<Account>> CreateAsync(Account account)
        {
            try
            {
                if (!IsUniqueUser(account.Email!))
                    return new Result<Account>(false, new List<string> { "An account with that email already exists!" });

                account.Password = _passwordService.HashPassword(account.Password!);

                await _context.Accounts!.AddAsync(account);
                await _context.SaveChangesAsync();

                var code = await _codeGeneratorService.GenerateVerificationCode();

                await _context.GeneratedCodes!.AddAsync(new GeneratedCode
                {
                    Code = code,
                    UserEmail = account.Email,
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsync(new EmailRequest
                {
                    To = account.Email,
                    Subject = _configuration["EmailService:ConfirmAccountSubject"],
                    Body = string.Format(_configuration["EmailService:ConfirmAccountBody"], "", code)
                });

                return new Result<Account>(account, new List<string> { "Account created successfully!" });
            }
            catch (Exception ex)
            {
                return new Result<Account>(false,
                    new List<string> { ex.ToString() });
            }
        }

        public Task<Result<bool>> DeleteAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Account>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Account>> GetByIdAsync(int id)
        {
            var account = await _context.Accounts!.SingleOrDefaultAsync(x => x.Id == id);
            if (account == null)
                return new Result<Account>(false, new List<string>() { "User not found" });

            return new Result<Account>(account);
        }

        public Task<Result<Account>> UpdateAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Account>> ConfirmAccountAsync(ConfirmAccountRequest confirmAccount)
        {
            var account = await _context.Accounts.Where(tsuro => tsuro.Email == confirmAccount.Email).FirstOrDefaultAsync();
            if (account == null) return new Result<Account>(false, new List<string>() { "User account not found!" });

            var code = await _context.GeneratedCodes.Where(x => x.UserEmail == confirmAccount.Email && x.Code == confirmAccount.ConfirmationCode).FirstOrDefaultAsync();
            if (code == null) return new Result<Account>(false, new List<string>() { "Invalid code provided!" });

            account.Status = Status.Inactive;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return new Result<Account>(account, new List<string>() { "Account activated successfully!" });
        }

        public async Task<Result<object>> LoginAsync(LoginRequest login)
        {
            var account = await _context.Accounts!.Where(x => x.Email == login.Email).FirstOrDefaultAsync();

            if (account!.Status == Status.Inactive) return new Result<object>(false, account, new List<string> { "Please create your profile to use this account." });
            if (account!.Status == Status.Unverified) return new Result<object>(false, account, new List<string> { "Verify your account with one time password." });

            object? userProfile = account!.AccountType == AccountType.Individual ?
                await _context.Individuals!.Where(x => x.AccountId == account.Id).FirstOrDefaultAsync() :
                await _context.Institutions!.Where(x => x.AccountId == account.Id).FirstOrDefaultAsync();

            if (account == null || _passwordService.VerifyHash(login.Password!, account!.Password!) == false)
                return new Result<object>(false, new List<string>() { "Username or password is incorrect!" });

            account.Token = await _jwtService.GenerateToken(account);
            account.Password = "*************";

            return new Result<object>(userProfile!);
        }

        private bool IsUniqueUser(string email)
        {
            var user = _context.Accounts!.SingleOrDefault(x => x.Email == email);

            if (user == null) return true;
            return false;
        }

        public async Task<Result<Account>> ChangePasswordAsync(ChangePasswordRequest changePassword)
        {
            var account = await GetByIdAsync(changePassword.UserId);
            if (!account.Success) return account;

            if (_passwordService.VerifyHash(changePassword.OldPassword!, account.Data!.Password!) == false)
                return new Result<Account>(false, new List<string>() { "Old password mismatch" });

            account.Data.Password = _passwordService.HashPassword(changePassword.NewPassword!);

            _context.Accounts!.Update(account.Data);
            await _context.SaveChangesAsync();

            return new Result<Account>(account.Data);
        }

        public async Task<Result<string>> ResendOtpAsync(string email)
        {
            var account = await _context.Accounts!.Where(x => x.Email!.Equals(email)).FirstOrDefaultAsync();
            if (account == null) return new Result<string>(false, new List<string>() { "Please ensure you have recently created an account with us!" });

            GeneratedCode? otpCode = await _context.GeneratedCodes!.Where(x => x.UserEmail == email && x.DateCreated.AddMinutes(5) >= DateTime.Now).FirstOrDefaultAsync();

            if (otpCode == null)
            {
                otpCode = new();
                otpCode.Code = await _codeGeneratorService.GenerateVerificationCode();

                await _context.GeneratedCodes!.AddAsync(new GeneratedCode
                {
                    Code = otpCode!.Code,
                    UserEmail = email,
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            var emailResult = await _emailService.SendEmailAsync(new EmailRequest
            {
                Body = string.Format(_configuration["EmailService:ResetCodeBody"], otpCode!.Code),
                Subject = _configuration["EmailService:ResetCodeSubject"],
                To = email
            });

            if (!emailResult.Success) return emailResult;

            return new Result<string>("Verification code has been sent to your email.");
        }

        public async Task<Result<string>> GetResetPasswordCodeAsync(string email)
        {
            var account = await _context.Accounts!.SingleOrDefaultAsync(y => y.Email == email);
            if (account == null) return new Result<string>(false, new List<string> { "User account does not exist." });

            var verificationCode = await _codeGeneratorService.GenerateVerificationCode();

            await _context.GeneratedCodes!.AddAsync(new GeneratedCode
            {
                Code = verificationCode,
                UserEmail = account.Email,
                DateCreated = DateTime.Now
            });

            await _context.SaveChangesAsync();

            var emailResult = await _emailService.SendEmailAsync(new EmailRequest
            {
                Body = string.Format(_configuration["EmailService:ResetCodeBody"], verificationCode),
                Subject = _configuration["EmailService:ResetCodeSubject"],
                To = account.Email
            });

            if (!emailResult.Success) return emailResult;

            return new Result<string>("Verification code has been sent to your email.");
        }

        public async Task<Result<Account>> ResetPasswordAsync(ResetPasswordRequest resetPassword)
        {
            var account = await _context.Accounts!.Where(x => x.Email == resetPassword.UserEmail).FirstOrDefaultAsync();
            var verifyCode = await _context.GeneratedCodes!
                .Where(x => x.UserEmail == resetPassword.UserEmail &&
                x.DateCreated.AddMinutes(5) >= DateTime.Now)
                .FirstOrDefaultAsync();

            if (verifyCode == null) return new Result<Account>(false, new List<string> { "Invalid password reset code provided." });

            account!.Password = _passwordService.HashPassword(resetPassword.NewPassword!);

            _context.Update(account);
            await _context.SaveChangesAsync();

            return new Result<Account>(account, new List<string> { "Your password has been resetted successfully." });
        }

        public async Task<Result<Account>> GetDetailsAsync(string email)
        {
            var account = await _context.Accounts!.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (account == null) return new Result<Account>(false, new List<string> { "Account does not exist" });

            return new Result<Account>(account);
        }
    }
}