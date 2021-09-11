using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class On_Node_Thing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int PointerOn = 0;
    public int Index = 3, Pos_Index = 3;
    public string Type = "";
    GameObject CH;
    GameObject UI;
    void Start()
    {
        CH = GameObject.Find("Control_Handler");
        UI = GameObject.Find("UI");
    }

    void Update()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show_Operations();
        PointerOn++;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(PointerLeave());
    }

    IEnumerator PointerLeave()
    {
        yield return new WaitForSeconds(0.25f);
        PointerOn--;
        if(PointerOn <= 0)
        {
            Hide_Operations();
        }
    }
    /*
    List<string> Cast_Skills = new List<string>();
    int Cast_Skill_Index = 1;
    public void Show_Cast_Skill(List<string> _Cast_Skills)
    {
        Cast_Skill_Index = 1;
        Cast_Skills = _Cast_Skills;
    }

    public void Roll_Cast_Skill()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Cast_Skills[Cast_Skill_Index];
    }
    */
    void Show_Operations()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<On_Node_Thing_Operation>().Show();
        }
    }

    void Hide_Operations()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<On_Node_Thing_Operation>().Hide();
        }
    }

    public void Do_Operation_Cast()
    {
        UI.GetComponent<UI>().Control_Handler_Skill_Info(Type, Index);
    }
    public void Do_Operation_Pick()
    {
        CH.GetComponent<Control_Handler>().Pick(Index);
    }
    
    /*
    public void Do_Operation(string _Operation)
    {
        
    }
    */
    public void Move_To_Pos(int _Index)
    {
        GetComponent<RectTransform>().localPosition = new Vector3((Index - _Index - 1) * 180, 0, 0);
        if((Index - _Index) > 2 || (Index - _Index) < 0)
        {
            GetComponent<RectTransform>().localPosition = new Vector3(10000, 0, 0);
        }
    }

    public void Move_Away()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(-10000, -40, 0);
    }
}
