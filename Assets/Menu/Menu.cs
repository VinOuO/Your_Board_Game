using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change_Scene(string _Scene_Name)
    {
        if(_Scene_Name == "Game_Setting")
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
        SceneManager.LoadScene(_Scene_Name, LoadSceneMode.Single);
    }

}
