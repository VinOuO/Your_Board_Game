using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Info : MonoBehaviour
{
    public GameObject CH;
    void Start()
    {
        CH = GameObject.Find("Control_Handler");
    }

    void Update()
    {
        
    }

    public void Click()
    {
        CH.GetComponent<Control_Handler>().Click_Item(this.gameObject.name);
    }
}
