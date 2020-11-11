namespace Common.Utils.AutoMapper
{
    using global::AutoMapper;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class SettingAutomapper
    {
        protected SettingAutomapper()
        {
        }

        public static void CreateMaps()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.ValidateInlineMaps = false;
            });
        }
    }
}