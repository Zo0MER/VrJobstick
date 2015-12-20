using System.Collections.Generic;
using JobstickSDK;
using System.Linq;
using UnityEngine;

namespace SpaceShooter
{
    public class Game : MonoBehaviour
    {
        public Score score;
        public Borders borders;
        public Transform pointToStartPlayers;
        public string mainSceneName;
		public MeteorGenerator meteorGenerator;
        public Transform playerPref;

		public bool isShowDebugInfo{ set; get;}

        private List<Player> players = new List<Player>();

        void SetScreenOrientation()
        {
            Screen.orientation = ScreenOrientation.Portrait; ;
        }

        public void OnPressButtonBack()
        {
            Jobstick.controller.StartSearchDevices();
            Jobstick.controller.DisconnectAllPlayers();
            Application.LoadLevel(mainSceneName);
        }

        private Player CreatePlayer(string address)
        {
			Transform[] points =  pointToStartPlayers.Cast<Transform>().ToArray();
			int countPlayers = Jobstick.controller.GetCountPlayers ();
			int index =  (countPlayers + 1) % points.Length;

			Transform obj = Instantiate(playerPref, points[index].position, Quaternion.identity) as Transform;
            Player player = obj.GetComponent<Player>();
            player.addressJobstick = address;

            return player;
        }

        public void AddPlayer(string address)
        {
            Player player = CreatePlayer(address);
            players.Add(player);

			meteorGenerator.StartGenerator ();
        }

        public void RemovePlayer(string address)
        {
            Player player = players.Find(player1 => player1.addressJobstick == address);
            if (player)
            {
                players.Remove(player);

                player.gameObject.SetActive(false);
                Destroy(player.gameObject);
            }

			if (Jobstick.controller.GetCountPlayers () <= 0) 
			{
				meteorGenerator.StopGenerator();
				meteorGenerator.RemoveAllMeteors();
			}
        }

        void Start()
        {
            if (!Jobstick.IsBluetoothOn())
            {
                Jobstick.RequestBluetooch();
            }
            
            SetScreenOrientation();
        }
        void Update()
        {

        }

        #region Singleton
        private static Game instance_ = null;

        public static Game instance
        {
            get { return instance_; }
        }
        Game()
        {
            instance_ = this;
        }
        #endregion
    }
}


