using System;
using System.Linq;
using System.Security.Cryptography;
using Auth.Options;
using Microsoft.Extensions.Options;

namespace Auth.Utils
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private readonly HashingOptions _options;

        public PasswordHasher(IOptions<HashingOptions> options)
        {
            _options = options.Value;
        }

        public string MakeHash(string password)
        {
            using var algotithm = new Rfc2898DeriveBytes(
                password,
                _options.SaltSize,
                _options.IterationsCount,
                HashAlgorithmName.SHA256
            );

            string key = Convert.ToBase64String(algotithm.GetBytes(_options.KeySize));
            string salt = Convert.ToBase64String(algotithm.Salt);

            return $"{_options.IterationsCount}.{salt}.{key}";
        }

        public bool Validate(string passwordHash, string password)
        {
            string[] hashParts = passwordHash.Split('.', 3);
            if (hashParts.Length != 3)
            {
                throw new FormatException("Unexpected password hash format.");
            }

            int iterations = Convert.ToInt32(hashParts[0]);
            byte[] salt = Convert.FromBase64String(hashParts[1]);
            byte[] key = Convert.FromBase64String(hashParts[2]);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256
            );

            byte[] keyToCheck = algorithm.GetBytes(_options.KeySize);
            return keyToCheck.SequenceEqual(key);
        }
    }
}