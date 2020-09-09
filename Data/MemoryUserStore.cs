using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using dcoreidentity.Models;


namespace dcoreidentity.Data
{
    internal class MemoryUserStore : IUserStore<CustomUser>, IUserPasswordStore<CustomUser>
    {
        private static List<CustomUser> users;
        private bool disposedValue;

        static MemoryUserStore(){
            users = new List<CustomUser>();    
        }

        public Task<IdentityResult> CreateAsync(CustomUser user, CancellationToken cancellationToken) {
            users.Add(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(CustomUser user, CancellationToken cancellationToken) {
            users.Remove(user);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<CustomUser> FindByIdAsync(string userId, CancellationToken cancellationToken) {
            return Task.FromResult(users.Find(u => u.Id == userId));            
        }

        public Task<CustomUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
            return Task.FromResult(users.Find(u => u.NormalizedUserName == normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(CustomUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(CustomUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(CustomUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(CustomUser user, string normalizedName, CancellationToken cancellationToken) {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(CustomUser user, string userName, CancellationToken cancellationToken) {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(CustomUser user, CancellationToken cancellationToken) {
            var storeUser = await FindByIdAsync(user.Id, cancellationToken);
            storeUser.UserName = user.UserName;
            storeUser.NormalizedUserName = user.NormalizedUserName;
            storeUser.PasswordHash = user.PasswordHash;
            return IdentityResult.Success;
        }

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(CustomUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(CustomUser user, CancellationToken cancellationToken) {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(CustomUser user, string passwordHash, CancellationToken cancellationToken) {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        #endregion

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~CustomUserStore()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }        
    }
}