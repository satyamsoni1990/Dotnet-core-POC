using webapi1.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace webapi1.Data {
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;
        public AuthRepository (DataContext context) {
            _context = context;
        }

        public async Task<User> Register (User user, string password) {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash (password, out PasswordHash, out PasswordSalt);
            user.PasswordSalt = PasswordSalt;
            user.PasswordHash = PasswordHash;

            await _context.Users.AddAsync (user);
            await _context.SaveChangesAsync ();
            return user;
        }

        public async Task<User> Login (string username, string password) {
            var user = await _context.Users.FirstOrDefaultAsync (x => x.Username == username);
            if (user == null) {
                return null;
            }
            if (!VerifyPasswordHash (password, user.PasswordHash, user.PasswordSalt)) {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash (string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes (password));
                for (int i = 0; i < computeHash.Length; i++) {
                    if (computeHash[i] != passwordHash[i]) {
                        return false;
                    }
                }
            }
            return true;
        }
        private void CreatePasswordHash (string password, out byte[] PasswordHash, out byte[] PasswordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        public async Task<bool> UserExists (string username) {
            if (await _context.Users.AnyAsync(x => x.Username == username)) {
                return true;
            }
            return false;
        }
    }
}