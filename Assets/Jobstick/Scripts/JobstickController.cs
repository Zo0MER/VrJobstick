using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JobstickSDK
{
    public class JobstickController : MonoBehaviour
    {
        public bool deviceIsConnected = false;
        public float delayBetweenScans = 5.0f;
        public float timeKickOfflinePlayers = 1.5f;

        private bool isScanning = false;
        private bool isReturnPauseSearchDevices = false;
        private float oldTime;

        public event Action<string> OnConnectPlayerEvent;
        public event Action<string> OnDisconnectPlayerEvent;
        public event Action OnStartSearchDevice;
        public event Action OnStopSearchDevice;
        public event Action OnDisconnectAllPlayers;

        public class Player
        {
            public string address;
            public JobstickAngle angless;
            public float latestUpdates = 0;
        }

        private Dictionary<string, Player> connectedPlayers = new Dictionary<string, Player>();

        private float time;
        private float timeAfterStartPlay = 1.0f;

        public bool GetIsSearchDevicesNow()
        {
            return isScanning;
        }
        
        /**
         * disconnect player at address
         * @param address
         */
        public void DisconnectPlayerFromAddress(string adress)
        {
            JobstickManager.jobstick.DisconnectPlayerAtAddress(adress);
            if (OnDisconnectPlayerEvent != null)
                OnDisconnectPlayerEvent(adress);
        }

        /**
         * disconnect all connected devices
         * @param if true - then send event OnDisconnectPlayerEvent
         */
        public void DisconnectAllPlayers(bool isSendEvent = true)
        {
            StopSearchDevices();

            while (connectedPlayers.Count > 0)
            {
                var p = connectedPlayers.First(pair => true);

                Debug.Log(p.Value.address);

                JobstickManager.jobstick.DisconnectPlayerAtAddress(p.Value.address);
                OnDisconnectPlayer(p.Value, isSendEvent);
            }

            time = Time.unscaledTime + timeAfterStartPlay;

            connectedPlayers.Clear();
        }

        /**
         * @return angles from device at address
         */
        public JobstickAngle GetAnglesToPlayerFromAddress(string address)
        {
            if (address != null && connectedPlayers.ContainsKey(address))
            {
                var player = connectedPlayers[address];
                return player.angless;
            }

            return null;
        }


        /**
         * @return angles from the first device
         */
        public JobstickAngle GetAnglesFirstPlayer()
        {
            if (connectedPlayers.Count > 0)
            {
                var player = connectedPlayers.First();
                return player.Value.angless;
            }

            return null;
        }

        /**
         * @return count of connected devices
        */
        public int GetCountPlayers()
        {
            return connectedPlayers.Count;
        }

        /**
        * here come the addresses and angles with all devices
        */
        void OnAnglesFromPlugin(string address, JobstickAngle angles)
        {
            //After call method DisconnectAllPlayers 
            //for some time come from the corners of devices 
            //to do this, a slight delay
            if (Time.unscaledTime > time)
            {
                if (angles != null)
                {
                    RefreshAnglesToPlayer(address, angles);
                }

                if (!deviceIsConnected)
                {
                    deviceIsConnected = true;
                    StartWatchToDisconnectPlayers();
                }
            }
        }

        /**
         * start search devices
         * @param if true - through time set in delayBetweenScans restart scanning
         */
        //запуск поиска устройств 
        public void StartSearchDevices(bool loop = true)
        {
            if (OnStartSearchDevice != null && !isScanning)
            {
                OnStartSearchDevice();
            }

            if (loop)
            {
                oldTime = Time.unscaledTime - delayBetweenScans;
                isScanning = true;
            }
            else
            {
                StopSearchDevices();
                JobstickManager.jobstick.StartSearchingDevices();
            }
        }

        /**
         * stop search devices
         */
        public void StopSearchDevices()
        {
            isScanning = false;
            JobstickManager.jobstick.StopSearchingDevices();

            if (OnStopSearchDevice != null)
            {
                OnStopSearchDevice();
            }
        }

        /**
         * set max players
         */
        public void SetMaxPlayers(int setMaxPlayers)
        {
            JobstickManager.jobstick.SetMaxPlayers(setMaxPlayers);
        }

        // Use this for initialization
        void Awake()
        {
            ConnectEvents();
        }

        void ConnectEvents()
        {
            Jobstick.events.jobstickOnBluetoothLENoFind += OnBluetoothLeNoFind;
            Jobstick.events.jobstickOnDisconnectToDevice += OnDisconnectToDevice;
            Jobstick.events.jobstickOnNoDataFromDevice += OnNoDataFromDevice;
            Jobstick.events.jobstickOnAnglesFromPlugin += OnAnglesFromPlugin;
        }

        void DisconnectEvents()
        {
            Jobstick.events.jobstickOnBluetoothLENoFind -= OnBluetoothLeNoFind;
            Jobstick.events.jobstickOnDisconnectToDevice -= OnDisconnectToDevice;
            Jobstick.events.jobstickOnNoDataFromDevice -= OnNoDataFromDevice;
            Jobstick.events.jobstickOnAnglesFromPlugin -= OnAnglesFromPlugin;
        }
        
        void OnDisable()
        {
            //DisconnectEvents();
        }

        void RefreshAnglesToPlayer(string address, JobstickAngle angles)
        {
            Player player = null;

            if (!connectedPlayers.ContainsKey(address))
            {
                //connected new player
                player = new Player();
                player.address = address;
                connectedPlayers.Add(address, player);
                OnConnectNewPlayer(player); 
            }
            else
            {
                player = connectedPlayers[address];
            }

            JobstickAngle angle = angles;
            angle.SetLastAngle(player.angless);

            player.angless = angle;
            player.latestUpdates = Time.fixedTime;
        }

        void OnConnectNewPlayer(Player player)
        {
            Debug.Log("Connect player - " + player.address.ToString());
            if (OnConnectPlayerEvent != null) 
                OnConnectPlayerEvent(player.address);
        }

        void OnDisconnectPlayer(Player player, bool isSendEvent = true)
        {
            OnDisconnectPlayer(player.address , isSendEvent);
        }

        void OnDisconnectPlayer(string adress, bool isSendEvent = true)
        {
            connectedPlayers.Remove(adress);
            JobstickManager.jobstick.DisconnectPlayerAtAddress(adress);
            if (OnDisconnectPlayerEvent != null && isSendEvent)
                OnDisconnectPlayerEvent(adress);
        }

        void StartWatchToDisconnectPlayers()
        {
            StopAllCoroutines();
            StartCoroutine(RefreshDisconnectedPlayers());

            Time.timeScale = 1f;
        }

        IEnumerator RefreshDisconnectedPlayers()
        {
            while (deviceIsConnected)
            {
                yield return StartCoroutine(WaitForSeconds(0.2f));
                RemoveDisconnectedPlayers();
            }
        }

        void RemoveDisconnectedPlayers()
        {
            if (KickOfflinePlayers())
            {
                if (connectedPlayers.Count == 0)
                {
                    if (OnDisconnectAllPlayers != null)
                    {
                        OnDisconnectAllPlayers();
                    }

                    deviceIsConnected = false;
                }
            }
        }

        bool KickOfflinePlayers()
        {
            foreach (var pair in connectedPlayers)
            {
                var player = pair.Value;
                if (Time.fixedTime - player.latestUpdates > timeKickOfflinePlayers)
                {
                    OnDisconnectPlayer(player);
                    return true;
                }
            }

            return false;
        }

 
        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                isReturnPauseSearchDevices = isScanning;

                StopSearchDevices();
                DisconnectAllPlayers();
            }
            else
            {
                RemoveDisconnectedPlayers();

                if (isReturnPauseSearchDevices)
                {
                    StartSearchDevices();
                }
            }
        }

        void OnApplicationQuit()
        {
            DisconnectAllPlayers();
        }

        IEnumerator WaitForSeconds(float time)
        {
            float delay = 0;
            while (delay < time)
            {
                delay += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        void Update()
        {
            if (isScanning && Time.unscaledTime - oldTime > delayBetweenScans)
            {
                JobstickManager.jobstick.StartSearchingDevices();
                oldTime = Time.unscaledTime;
            }
        }

        #region Native callbacks
        private void OnNoDataFromDevice()
        {

        }
        //don't use
        private void OnDisconnectToDevice()
        {
        }

        private void OnBluetoothLeNoFind()
        {
        }
        #endregion
    }
}
