namespace WpfCvtApp.Infrastructure.Adapters
{
    public interface IPanelInfo
    {
        string GetPanelCaption();
        bool ShowCloseButton { get; set; }
        bool AutoHide { get; set; }
    }
}