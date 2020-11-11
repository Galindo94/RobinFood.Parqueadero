namespace Common.Utils.Swagger
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class SwaggerConfiguration
    {
        protected SwaggerConfiguration()
        {
        }

        /// <summary>
        /// <para>Foo API v1</para>
        /// </summary>
        public const string EndpointDescription = "Parqueadero API v1";

        /// <summary>
        /// <para>/swagger/v1/swagger.json</para>
        /// </summary>
        public const string EndpointUrl = "/swagger/v1.0/swagger.json";

        /// <summary>
        /// <para>Jorge Serrano</para>
        /// </summary>
        public const string ContactName = "Daniel Sepulveda Galindo";

        /// <summary>
        /// <para>http://geeks.ms/jorge/</para>
        /// </summary>
        public const string ContactUrl = "";

        /// <summary>
        /// <para>v1</para>
        /// </summary>
        public const string DocNameV1 = "1.0";

        /// <summary>
        /// <para>Foo API</para>
        /// </summary>
        public const string DocInfoTitle = "Parqueadero API";

        /// <summary>
        /// <para>v1</para>
        /// </summary>
        public const string DocInfoVersion = "1.0";

        /// <summary>
        /// <para>Foo Api - Sample Web API in ASP.NET Core 2</para>
        /// </summary>
        public const string DocInfoDescription = "Parqueadero Api - Documentacion Servicios Rest";
    }
}