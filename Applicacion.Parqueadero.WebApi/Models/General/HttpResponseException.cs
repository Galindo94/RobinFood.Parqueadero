namespace Applicacion.Transferencias.WebApi.Models.General
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class HttpResponseException : Exception
    {
        public int Status { get; set; }

        public object Value { get; set; }
    }
}