using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_Map_Stuff : MonoBehaviour
{
    GameObject GS;
    void Start()
    {
        GS = GameObject.Find("Game_Setting");
    }

    // Update is called once per frame
    void Update()
    {
        if(GS.GetComponent<Game_Setting>().Stage == "Other_Setting")
        {
            Destroy(gameObject);
        }
    }
}
