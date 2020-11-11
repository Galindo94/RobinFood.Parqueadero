using Common.Utils.Constant;
using Common.Utils.Utils.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Common.Utils.Utils
{
    [ExcludeFromCodeCoverage]
    public class Utils : IUtils
    {
        public readonly IConfiguration configuration;
        private readonly IMemoryCache cache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="cache"></param>
        public Utils(IConfiguration configuration, IMemoryCache cache)
        {
            this.configuration = configuration;
            this.cache = cache;
        } 
    }
}