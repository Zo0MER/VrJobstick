using System;
using UnityEngine;
using System.Collections;
using JobstickSDK;
using UnityEngine.UI;
using Jobstick = JobstickSDK.Jobstick;

public class JobstickCharacterController : MonoBehaviour
{

    Character character;
    private int maxPlayers = 1;
    public Text scanningLabel;

    void Start()
    {
        character = GetComponent<Character>();

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

        character.addressJobstick = s;
    }

    void WaitPlayers()
    {
        Time.timeScale = 0.0f;
    }
}

