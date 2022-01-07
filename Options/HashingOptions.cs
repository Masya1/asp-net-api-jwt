namespace Auth.Options
{
    public sealed class HashingOptions
    {
        public int IterationsCount { get; set; }
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
    }
}