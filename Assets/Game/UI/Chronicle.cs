using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronicle : MonoBehaviour
{

    public Text Content;
    public GameObject Control_Handler;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Control_Handler_Show_Chronicle_Trigger();
        }
    }

    public void Control_Handler_Show_Chronicle_Trigger()
    {
        Control_Handler.GetComponent<Control_Handler>().Show_Chronicle_Trigger();
    }

    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Content.text = Content.text.Replace("\\n", "\n");
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
