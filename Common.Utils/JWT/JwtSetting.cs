namespace Common.Utils.JWT
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public sealed class JwtSetting
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }
    }
}