using WpfCvtApp.Infrastructure.Data;

namespace WpfCvtApp.Infrastructure.DataServices
{
    public class SettingsDataService : ISettingsDataService
    {
        public VoronoiSettings VoronoiSettings { get; }

        public SettingsDataService()
        {
            VoronoiSettings = new VoronoiSettings();
        }
    }
}