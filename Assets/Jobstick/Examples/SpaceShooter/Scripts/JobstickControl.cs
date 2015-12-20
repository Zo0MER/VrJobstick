using System;
using UnityEngine;
using System.Collections;
using JobstickSDK;
using UnityEngine.UI;
using Jobstick = JobstickSDK.Jobstick;

namespace SpaceShooter
{
    public class JobstickControl : MonoBehaviour
    {
        public Transform scanningLabel;
        public Game game;

        private int maxPlayers = 2;

        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            ConnectEvents();
            Jobstick.controller.SetMaxPlayers(maxPlayers);
            Jobstick.controller.StartSearchDevices();
        }

        public void OnExitOnStopSearchDevice()
        {
            DisconnectEvents();
        }

        void ConnectEvents()
        {
            Jobstick.controller.OnConnectPlayerEvent += OnConnectPlayerEvent;
            Jobstick.controller.OnDisconnectPlayerEvent += OnDisconnectPlayerEvent;
            Jobstick.controller.OnStartSearchDevice += OnStartSearchDevice;
            Jobstick.controller.OnStopSearchDevice += OnStopSearchDevice;
        }

        void DisconnectEvents()
        {
            Jobstick.controller.OnConnectPlayerEvent += OnConnectPlayerEvent;
            Jobstick.controller.OnDisconnectPlayerEvent += OnDisconnectPlayerEvent;
            Jobstick.controller.OnStartSearchDevice += OnStartSearchDevice;
            Jobstick.controller.OnStopSearchDevice += OnStopSearchDevice;
        }

        private void OnStopSearchDevice()
        {
            if (scanningLabel)
                scanningLabel.gameObject.SetActive(false);
        }

        private void OnStartSearchDevice()
        {
            if (scanningLabel)
                scanningLabel.gameObject.SetActive(true);
        }

        private void OnDisconnectPlayerEvent(string s)
        {
            game.RemovePlayer(s);

            Jobstick.controller.StartSearchDevices();
        }

        private void OnConnectPlayerEvent(string s)
        {
            if (Jobstick.controller.GetCountPlayers() < maxPlayers)
            {
                Jobstick.controller.StartSearchDevices();
            }

            if (Jobstick.controller.GetCountPlayers() <= 0)
            {
                WaitPlayers();
            }

            game.AddPlayer(s);
        }

        void WaitPlayers()
        {
            //Time.timeScale = 0.0f;
        }

        void Update()
        {

        }
    } 
}

