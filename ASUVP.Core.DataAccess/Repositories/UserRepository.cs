using System;
using System.Data.Entity;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.DataAccess.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task CreateAsync(User user)
        {
            Contract.RequiresParameterNotNull(user);
            Add(user);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            Contract.RequiresParameterNotNull(user);
            Context.Entry(user).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            Contract.RequiresParameterNotNull(user);
            Remove(user);
            await SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            return await GetByIdAsync<User>(userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Context.Set<User>().FirstOrDefaultAsync(e => e.UserName.ToUpper() == userName.ToUpper());
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            Contract.RequiresParameterNotNull(user);
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            Contract.RequiresParameterNotNull(user);
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            Contract.RequiresParameterNotNull(user);
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            Contract.RequiresParameterNotNull(user);
            return Task.FromResult(user.SecurityStamp);
        }
    }
}