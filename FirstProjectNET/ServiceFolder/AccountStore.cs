using FirstProjectNET.Data;
using FirstProjectNET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FirstProjectNET.ServiceFolder
{
	public class AccountStore : IUserStore<Account>, IUserPasswordStore<Account>, IUserEmailStore<Account>,IUserLoginStore<Account>
	{
		private readonly HotelDbContext _hotelDbContext;
		private readonly IPasswordHasher<Account> _passwordHasher;

		public AccountStore(HotelDbContext hotelDbContext,IPasswordHasher<Account> passwordHasher)
		{
			_hotelDbContext = hotelDbContext;
			_passwordHasher = passwordHasher;
		}

        public async Task AddLoginAsync(Account user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var externalLogin = new AccountLogin
            {
                AccountID = user.AccountID,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            _hotelDbContext.AccountLogins.Add(externalLogin);
            await _hotelDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(Account user, CancellationToken cancellationToken)
		{
			// Kiểm tra nếu mật khẩu chưa được hash, thực hiện hash
			//if (string.IsNullOrEmpty(user.Password) || !user.Password.StartsWith("$2a$") && !user.Password.StartsWith("$2b$"))
			//{
			//	user.Password = _passwordHasher.HashPassword(user, user.Password);
			//}

			_hotelDbContext.Accounts.Add(user);
			await _hotelDbContext.SaveChangesAsync(cancellationToken);
			return IdentityResult.Success;
		}


		public async Task<IdentityResult> DeleteAsync(Account user, CancellationToken cancellationToken)
		{
			_hotelDbContext.Accounts.Remove(user);
			await _hotelDbContext.SaveChangesAsync(cancellationToken);
			return IdentityResult.Success;
		}

		public void Dispose()
		{
			// Không cần dispose vì DbContext đã được quản lý bởi DI
		}

		public async Task<Account?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			return await _hotelDbContext.Accounts
				.FirstOrDefaultAsync(a => a.Email.ToUpper() == normalizedEmail, cancellationToken);
		}

		public async Task<Account?> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			int id = int.Parse(userId);
			return await _hotelDbContext.Accounts.FindAsync(new object[] { id }, cancellationToken);
		}

        public async Task<Account?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var login = await _hotelDbContext.AccountLogins
                .FirstOrDefaultAsync(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);

            if (login == null)
                return null;

            return await _hotelDbContext.Accounts.FindAsync(new object[] { login.AccountID }, cancellationToken);
        }

        public async Task<Account?> FindByNameAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			return await _hotelDbContext.Accounts
				.FirstOrDefaultAsync(a => a.Email.ToUpper() == normalizedEmail, cancellationToken);
		}

		public Task<string?> GetEmailAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(Account user, CancellationToken cancellationToken)
		{
			// Nếu không có xác thực email, mặc định trả về true
			return Task.FromResult(true);
		}

        public Task<IList<UserLoginInfo>> GetLoginsAsync(Account user, CancellationToken cancellationToken)
        {
            var logins = _hotelDbContext.AccountLogins
                .Where(l => l.AccountID == user.AccountID)
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.LoginProvider))
                .ToList();

            return Task.FromResult<IList<UserLoginInfo>>(logins);
        }

        public Task<string?> GetNormalizedEmailAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email?.ToUpper());
		}

		public Task<string?> GetNormalizedUserNameAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email?.ToUpper());
		}

		public Task<string?> GetPasswordHashAsync(Account user, CancellationToken cancellationToken)
		{ 
			return Task.FromResult(user.Password);  
		}

		public Task<string> GetUserIdAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.AccountID.ToString());
		}

		public Task<string?> GetUserNameAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email);
		}

		public Task<bool> HasPasswordAsync(Account user, CancellationToken cancellationToken)
		{
			return Task.FromResult(!string.IsNullOrEmpty(user.Password));
		}

        public async Task RemoveLoginAsync(Account user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var login = await _hotelDbContext.AccountLogins
                .FirstOrDefaultAsync(l => l.AccountID == user.AccountID && l.LoginProvider == loginProvider && l.ProviderKey == providerKey, cancellationToken);

            if (login != null)
            {
                _hotelDbContext.AccountLogins.Remove(login);
                await _hotelDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task SetEmailAsync(Account user, string? email, CancellationToken cancellationToken)
		{
			user.Email = email;
			return Task.CompletedTask;
		}

		public Task SetEmailConfirmedAsync(Account user, bool confirmed, CancellationToken cancellationToken)
		{
			// Nếu không có cột xác nhận email, không cần xử lý gì cả
			return Task.CompletedTask;
		}

		public Task SetNormalizedEmailAsync(Account user, string? normalizedEmail, CancellationToken cancellationToken)
		{
			// Không cần xử lý gì nếu không sử dụng normalized email
			return Task.CompletedTask;
		}

		public Task SetNormalizedUserNameAsync(Account user, string? normalizedEmail, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public Task SetPasswordHashAsync(Account user, string? passwordHash, CancellationToken cancellationToken)
		{
			user.Password = passwordHash;
			return Task.CompletedTask;
		}

		public Task SetUserNameAsync(Account user, string? email, CancellationToken cancellationToken)
		{
			user.Email = email;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(Account user, CancellationToken cancellationToken)
		{
			_hotelDbContext.Accounts.Update(user);
			await _hotelDbContext.SaveChangesAsync(cancellationToken);
			return IdentityResult.Success;
		}
	}
}
