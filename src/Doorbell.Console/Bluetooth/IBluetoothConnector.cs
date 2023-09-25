namespace Doorbell.Console.Bluetooth
{
    public interface IBluetoothConnector
    {
        Task<bool> ConnectAsync();
    }
}