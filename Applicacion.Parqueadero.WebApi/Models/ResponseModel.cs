namespace Applicacion.Transferencias.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class ResponseModel<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }
        public T Result { get; set; }
    }
}