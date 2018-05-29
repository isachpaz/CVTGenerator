using WpfCvtApp.Infrastructure.Data;

namespace WpfCvtApp.Infrastructure.DataServices
{
    public interface ISettingsDataService
    {
        VoronoiSettings VoronoiSettings { get; }
    }
}