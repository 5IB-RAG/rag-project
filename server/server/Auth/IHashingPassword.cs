using server.Model;

namespace server.Auth
{
    public interface IHashingPassword
    {
        public Task<string> CreateUser(UserAuthSignup create);
        public Task<string> UserVerify(UserAuthSignup verify);
    }
}
