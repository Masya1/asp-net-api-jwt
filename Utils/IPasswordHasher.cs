namespace Auth.Utils
{
    public interface IPasswordHasher
    {
        string MakeHash(string password);

        bool Validate(string passwordHash, string password);
    }
}