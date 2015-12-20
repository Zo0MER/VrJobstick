using System.Collections.Generic;
using JobstickSDK;
using UnityEngine;

namespace Table
{
    public class Main : MonoBehaviour
    {
        private readonly List<DeviceInfoItem> deviceInfoItems = new List<DeviceInfoItem>();
        public ButtonStartScan buttonStartScan;
        public Transform itemDeviceInfaPrefab;
        public Transform rootTable;
        public Transform scanningLabel;

        private void SetScreenOrientation()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            ;
        }

        private void RequestBluetooth()
        {
            //Включен ли bluetooth на устройстве?
            if (!Jobstick.IsBluetoothOn())
            {
                //Выводим окно с запросом включения bluetooth
                Jobstick.RequestBluetooch();
            }
        }

        public void OnPressButtonStartScan()
        {
            if (!Jobstick.controller.GetIsSearchDevicesNow())
            {
                if (Jobstick.IsBluetoothOn())
                {
                    Jobstick.controller.StartSearchDevices(true);
                }
                else
                {
                    RequestBluetooth();
                }
            }
            else
            {
                Jobstick.controller.StopSearchDevices();
            }
        }

        private DeviceInfoItem AddItem(string address)
        {
            var itemInfo = Instantiate(itemDeviceInfaPrefab) as Transform;
            itemInfo.parent = rootTable;
            itemInfo.localScale = new Vector3(1, 1, 1);

            var item = itemInfo.GetComponent<DeviceInfoItem>();
            item.deviceAddress = address;

            item.OnPressButtonDisconnect =
                delegate(DeviceInfoItem infoItem) { Jobstick.controller.DisconnectPlayerFromAddress(infoItem.deviceAddress); };

            return item;
        }

        private void OnConnectPlayerEvent(string address)
        {
            if (deviceInfoItems.Find(item => item.deviceAddress == address) == null)
            {
                var item = AddItem(address);
                deviceInfoItems.Add(item);
            }
        }

        private void OnStopSearchDevice()
        {
            scanningLabel.gameObject.SetActive(false);
            buttonStartScan.SetStartScan();
        }

        private void OnStartSearchDevice()
        {
            scanningLabel.gameObject.SetActive(true);
            buttonStartScan.SetStopScan();
        }

        private void OnDisconnectPlayerEvent(string address)
        {
            var deviceInfo = deviceInfoItems.Find(item => item.deviceAddress == address);

            if (deviceInfo != null)
            {
                deviceInfoItems.Remove(deviceInfo);
                deviceInfo.gameObject.SetActive(false);
                Destroy(deviceInfo.gameObject);
            }
        }

        private void ConnectEvents()
        {
            Jobstick.controller.OnConnectPlayerEvent += OnConnectPlayerEvent;
            Jobstick.controller.OnDisconnectPlayerEvent += OnDisconnectPlayerEvent;
            Jobstick.controller.OnStartSearchDevice += OnStartSearchDevice;
            Jobstick.controller.OnStopSearchDevice += OnStopSearchDevice;
        }

        private void DisconnectEvents()
        {
            Jobstick.controller.OnConnectPlayerEvent -= OnConnectPlayerEvent;
            Jobstick.controller.OnDisconnectPlayerEvent -= OnDisconnectPlayerEvent;
            Jobstick.controller.OnStartSearchDevice -= OnStartSearchDevice;
            Jobstick.controller.OnStopSearchDevice -= OnStopSearchDevice;
        }

        private void Start()
        {
            SetScreenOrientation();
            ConnectEvents();
            RequestBluetooth();
        }

        public void OnPressButtonBack()
        {
            DisconnectEvents();
            Jobstick.controller.DisconnectAllPlayers();
            Jobstick.controller.StopSearchDevices();
        }

        private void Update()
        {
        }
    }
}