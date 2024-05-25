using server.Db;
using server.Model;
using System.Text;
using server.Helpers;

namespace server.Auth
{
    public class HashingPassword : IHashingPassword
    {
        private readonly PgVectorContext _dbContext;
        public HashingPassword(PgVectorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateUser(UserAuthSignup create)
        {
            string password = create.ConfirmPassword;

            byte[] saltBytes = PasswordEncryption.GenerateSalt();
            // Hash the password with the salt
            string hashedPassword = PasswordEncryption.HashPassword(password, saltBytes);
            string base64Salt = Convert.ToBase64String(saltBytes);

            byte[] retrievedSaltBytes = Convert.FromBase64String(base64Salt);

            var user = new User
            {
                ConfirmPassword = hashedPassword,
                Email = "",
                IsActive = true,
                LastActiondatetime = DateTime.Now,
                Mobile = create.MobileNo,
                Password = base64Salt,
                UserName = create.UserName,
                Salt = retrievedSaltBytes
            };
            _dbContext.Usertests.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return "User added successfully";
        }

        public async Task<string> UserVerify(UserAuthSignup verify)
        {
            // In a real scenario, you would retrieve these values from your database
            var user = _dbContext.Usertests.Where(x => x.Mobile == verify.MobileNo).Select(x => x).FirstOrDefault();

            string storedHashedPassword = user.ConfirmPassword;// "hashed_password_from_database";
            //string storedSalt = user.Salt; //"salt_from_database";
            byte[] storedSaltBytes = user.Salt;
            string enteredPassword = verify.ConfirmPassword; //"user_entered_password";

            // Convert the stored salt and entered password to byte arrays
            // byte[] storedSaltBytes = Convert.FromBase64String(user.Salt);
            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);

            // Concatenate entered password and stored salt
            byte[] saltedPassword = new byte[enteredPasswordBytes.Length + storedSaltBytes.Length];
            Buffer.BlockCopy(enteredPasswordBytes, 0, saltedPassword, 0, enteredPasswordBytes.Length);
            Buffer.BlockCopy(storedSaltBytes, 0, saltedPassword, enteredPasswordBytes.Length, storedSaltBytes.Length);

            // Hash the concatenated value
            string enteredPasswordHash = PasswordEncryption.HashPassword(enteredPassword, storedSaltBytes);

            // Compare the entered password hash with the stored hash
            if (enteredPasswordHash == storedHashedPassword)
            {
                return "Password is correct.";
            }
            else
            {
                return "Password is incorrect.";
            }
        }
    }
}
