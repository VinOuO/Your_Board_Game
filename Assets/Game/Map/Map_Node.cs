using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Map_stuff;
using Photon.Pun;

public class Map_Node : MonoBehaviourPunCallbacks
{
    public GameObject Info_UI;
    public Node This_Node;
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void Show_Info(string _Info)
    {
        Info_UI.GetComponent<CanvasGroup>().alpha = 1;
        Info_UI.GetComponent<CanvasGroup>().interactable = true;
        Info_UI.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Info_UI.transform.GetChild(1).GetComponent<Text>().text = _Info;
    }

    public void Hide_Info()
    {
        Info_UI.GetComponent<CanvasGroup>().alpha = 0;
        Info_UI.GetComponent<CanvasGroup>().interactable = false;
        Info_UI.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    [PunRPC]
    void SetAll(string tempString, int number)
    {
        Debug.Log(tempString + " " + number);
    }
}
