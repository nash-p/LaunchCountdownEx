namespace LaunchCountDown.Common
{
    public interface IPersistentItem
    {
        bool Load();

        void Save();
    }
}