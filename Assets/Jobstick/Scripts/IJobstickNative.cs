namespace JobstickSDK
{
    public interface IJobstickNative
    {
        void StartSearchingDevices();
        void StopSearchingDevices();
        void DisconnectAllPlayers();
        void DisconnectPlayerAtAddress(string address);
        JobstickAngle GetAngles();
        void BluetoothOn();
        void BluetoothOff();
        bool IsBluetoothOn();
        void SetMaxPlayers(int max);
        void RequestBluetooch();
    }
}
