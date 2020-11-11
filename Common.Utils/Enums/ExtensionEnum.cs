namespace Common.Utils.Enums
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    [ExcludeFromCodeCoverage]
    public static class ExtensionEnum
    {
        public static string GetDisplayName(this Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }

        /// <summary>
        /// Get display name from enum value
        /// </summary>
        /// <param name="emun"></param>
        /// <param name="valueEnum"></param>
        /// <returns></returns>
        public static string GetDisplayName(Type emun, int valueEnum)
        {
            return emun.GetMember(emun.GetEnumName(valueEnum)).FirstOrDefault().GetCustomAttribute<DisplayAttribute>(false).Name;
        }
    }
}