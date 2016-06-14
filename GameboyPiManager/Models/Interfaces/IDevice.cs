namespace GameboyPiManager.Models.Interfaces
{
    public interface IDevice
    {
        string Name { get; set; }
        bool IsConnected { get; set; }
    }
}