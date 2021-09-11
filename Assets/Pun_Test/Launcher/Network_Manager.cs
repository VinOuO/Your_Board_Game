using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.Vincent.Board_Game
{
    public class Network_Manager : MonoBehaviourPunCallbacks
    {
        [Tooltip("遊戲室玩家人數上限. 當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 10;
        // 遊戲版本的編碼, 可讓 Photon Server 做同款遊戲不同版本的區隔.
        string gameVersion = "1";
        int Set_Up_Player_Num = 0;
        bool Is_Master = false;
        //-----------------
        GameObject GM;
        //-----------------
        void Awake()
        {
            // 確保所有連線的玩家均載入相同的遊戲場景
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        void Start()
        {
            
            DontDestroyOnLoad(this.gameObject);
        }

        bool Setting_Up = false;
        bool Ready_To_Set_Up = false;
        private void Update()
        {
            if (!Setting_Up && Ready_To_Set_Up && PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(Check_Set_Up());
            }
        }

        IEnumerator Check_Set_Up()
        {
            bool _Set = false;
            Setting_Up = true;
            while (!_Set)
            {
                Debug.Log("Set_Player_Num: " + Set_Up_Player_Num);
                Debug.Log("Player_Num: " + PhotonNetwork.PlayerList.Length);
                
                if (Set_Up_Player_Num == PhotonNetwork.PlayerList.Length)
                {
                    Change_All_Player_Scene("Game");
                    _Set = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        // 與 Photon Cloud 連線
        public void Connect()
        {
            /*
            // 檢查是否與 Photon Cloud 連線
            if (PhotonNetwork.IsConnected)
            {
                // 已連線, 嚐試隨機加入一個遊戲室
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // 未連線, 嚐試與 Photon Cloud 連線
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            */
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");
            /*
            // 確認已連上 Photon Cloud
            // 隨機加入一個遊戲室
            PhotonNetwork.JoinRandomRoom();
            */
            if (Is_Master)
            {
                PhotonNetwork.CreateRoom("Room1", new RoomOptions { MaxPlayers = maxPlayersPerRoom });//連線失敗別自己開遊戲，用排成每隔一段時間嘗試加入房間
            }
            else
            {
                PhotonNetwork.JoinRoom("Room1");
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN 呼叫 OnDisconnected() {0}.", cause);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN 呼叫 OnJoinRandomFailed(), 隨機加入遊戲室失敗.");

            // 隨機加入遊戲室失敗. 可能原因是 1. 沒有遊戲室 或 2. 有的都滿了.    
            // 好吧, 我們自己開一個遊戲室.
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});//連線失敗別自己開遊戲，用排成每隔一段時間嘗試加入房間
        }

        //------------------------------------------------
        
        //------------------------------------------------

        public override void OnJoinedRoom()
        {
            //------------------------------------------------
            if (!PhotonNetwork.IsMasterClient)
            {
                
            }
            //------------------------------------------------
            Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");
        }
        void OnApplicationQuit()
        {
            PhotonNetwork.Disconnect();
        }

        public void Change_All_Player_Scene(string _Scene_Name)
        {
            photonView.RPC("Change_Scene", RpcTarget.All, _Scene_Name);
        }

        [PunRPC]
        public void Change_Scene(string _Scene_Name)
        {
            
            switch (_Scene_Name)
            {
                case "Game_Setting":
                    Is_Master = true;
                    Connect();
                    PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    SceneManager.LoadScene(_Scene_Name, LoadSceneMode.Single);
                    break;
                case "Game":
                    Destroy(GameObject.Find("Game_Setting"));
                    SceneManager.LoadScene(_Scene_Name, LoadSceneMode.Single);
                    break;
                case "Waiting_Room":
                    Connect();
                    SceneManager.LoadScene(_Scene_Name, LoadSceneMode.Single);
                    break;
                case "Figure_Setting":
                    if (Is_Master)
                    {
                        Ready_To_Set_Up = true;
                    }
                    SceneManager.LoadScene(_Scene_Name, LoadSceneMode.Single);
                    break;
            }
        }

        public void Local_Set_Up()
        {
            photonView.RPC("Set_Up", PhotonNetwork.MasterClient);
        }

        [PunRPC]
        public void Set_Up()
        {
            Set_Up_Player_Num++;
        }

        [PunRPC]
        void GM_CreatPlayerPrefab()
        {
            //GM = GameObject.Find("Game_Manager");
            //GM.GetComponent<Game_Manager>().CreatPlayerPrefab();
        }
    }
}