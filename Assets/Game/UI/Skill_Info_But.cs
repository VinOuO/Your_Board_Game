using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Info_But : MonoBehaviour
{
    public GameObject CH;
    GameObject UI;
    void Start()
    {
        CH = GameObject.Find("Control_Handler");
        UI = GameObject.Find("UI");
    }

    void Update()
    {

    }

    public void Cast_This_Skill()
    {
        CH.GetComponent<Control_Handler>().Cast(transform.GetChild(0).GetComponent<Text>().text, UI.GetComponent<UI>().Target_Type, UI.GetComponent<UI>().Target_Index);
        UI.GetComponent<UI>().Target_Type = "None";
        UI.GetComponent<UI>().Show_Or_Hide_Skill_Info("Hide");
    }
}
