using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace JobstickSDK
{
    public class JobstickIos 
#if UNITY_IOS
    : IJobstickNative
#endif
    {
#if (UNITY_IOS && !UNITY_EDITOR)
    #region NativeMethods
	    [DllImport("__Internal")]
	    private static extern void _Init();

	    [DllImport("__Internal")]
	    private static extern void _StartScan();

	    [DllImport("__Internal")]
	    private static extern void _StopScan();

	    [DllImport("__Internal")]
	    private static extern void _DisconnectAllPlayers();

	    [DllImport("__Internal")]
	    private static extern void _CloseConnectionPlayer(string adress);

	    [DllImport("__Internal")]
	    private static extern void _SetMaxPlayers(int maxPlayers);

	    [DllImport("__Internal")]
	    private static extern void _BluetoothEnable();

	    [DllImport("__Internal")]
	    private static extern void _BluetoothDisable();

	    [DllImport("__Internal")]
	    private static extern bool _IsBluetoothOn();

	    [DllImport("__Internal")]
	    private static extern string _GetAnglesString();

		[DllImport("__Internal")]
		private static extern void _RequestBluetooch();


	    #endregion

	    public JobstickIos()
	    {
	        Init();
	    }

	    public void Init()
	    {
	        _Init();
	    }

	    public void StartSearchingDevices()
	    {
	        _StartScan();
	    }

	    public void StopSearchingDevices()
	    {
	        _StopScan();
	    }

	    public void DisconnectAllPlayers()
	    {
	        _DisconnectAllPlayers();
	    }

	    public void DisconnectPlayerAtAddress(string adress)
	    {
	        _CloseConnectionPlayer(adress);
	    }

	    public void SetMaxPlayers(int max)
	    {
	        _SetMaxPlayers(max);
	    }

	    public void BluetoothOn()
	    {
	        _BluetoothEnable();
	    }

	    public void BluetoothOff()
	    {
	        _BluetoothDisable();
	    }

	    public bool IsBluetoothOn()
	    {
	        return _IsBluetoothOn();
	    }

		public JobstickAngle GetAngles()
	    {
	        return JobstickManager.ParseString(_GetAnglesString());
	    }	

		public void RequestBluetooch()
		{
			_RequestBluetooch();
		}
#else
        public JobstickIos()
        {
            Init();
        }

        public void Init()
        {
        }

        public void StartSearchingDevices()
        {
        }

        public void StopSearchingDevices()
        {   
        }

        public void DisconnectAllPlayers()
        {
        }

        public void DisconnectPlayerAtAddress(string adress)
        {
        }

        public void SetMaxPlayers(int max)
        {
        }

        public void BluetoothOn()
        {
        }

        public void BluetoothOff()
        {
        }

        public bool IsBluetoothOn()
        {
            return true;
        }

        public JobstickAngle GetAngles()
        {
            return new JobstickAngle();
        }


		public void RequestBluetooch()
		{

		}
#endif

    }
}
