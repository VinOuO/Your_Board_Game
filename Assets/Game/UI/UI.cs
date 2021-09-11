using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public GameObject Things_On_Node, BackPack_Info, Skill_Info;
    public GameObject Control_Handler;
    public GameObject Back_Pack_Info_Prefab, Skill_Info_Prefab;
    public GameObject Fairy;
    UI_RayCast UI_RC;
    void Start()
    {
        BackPack_Ins(BackPack_Ins_Num);
        Skill_Info_Ins(Skill_Info_Ins_Num);
        UI_RC = GetComponent<UI_RayCast>();
        Fairy = GameObject.Find("Fairy");
    }

    void Update()
    {
        Point_On_Manage(UI_RC.Point_On_UIs());
    }

    public void Point_On_Manage(List<RaycastResult> _Point_On_UIs)
    {
        foreach (RaycastResult _Point_On_UI in _Point_On_UIs)
        {
            //---------------------------------------------------------------------------------------------------------------------
            switch (_Point_On_UI.gameObject.name)
            {
                case "Players":
                    Roll_On_Node_Info("Players");
                    break;
                case "Mobs":
                    Roll_On_Node_Info("Mobs");
                    break;
                case "Stuffs":
                    Roll_On_Node_Info("Stuffs");
                    break;
                case "BackPack_Info":
                    if (Input.mouseScrollDelta.y < 0 && Item_Names.Count - BackPack_Index > 3)
                    {
                        BackPack_Index += 3;
                        Roll_BackPack_Item();
                    }
                    else if (Input.mouseScrollDelta.y > 0 && BackPack_Index > 3)
                    {
                        BackPack_Index -= 3;
                        Roll_BackPack_Item();
                    }
                    break;
                case "Skill_Info":
                    if (Input.mouseScrollDelta.y < 0 && Skill_Names.Count - Skill_Info_Index > 0)
                    {
                        Skill_Info_Index += 1;
                        Roll_Skill_Info();
                    }
                    else if (Input.mouseScrollDelta.y > 0 && Skill_Info_Index > 1)
                    {
                        Skill_Info_Index -= 1;
                        Roll_Skill_Info();
                    }
                    break;
            }
            //---------------------------------------------------------------------------------------------------------------------
            switch (_Point_On_UI.gameObject.tag)
            {
                case "Item":
                    Debug.Log("Hit Item!!!");
                    if (Input.GetKey(KeyCode.Q))
                    {
                        Control_Handler.GetComponent<Control_Handler>().Ask_Question("Item", _Point_On_UI.gameObject.name, "Consult");
                    }
                    else
                    {
                        Control_Handler.GetComponent<Control_Handler>().Ask_Question("Item", _Point_On_UI.gameObject.name, "Description");
                    }
                    break;
                case "Skill":
                    if (Input.GetKey(KeyCode.Q))
                    {
                        Control_Handler.GetComponent<Control_Handler>().Ask_Question("Skill", _Point_On_UI.gameObject.name, "Consult");
                    }
                    else
                    {
                        Control_Handler.GetComponent<Control_Handler>().Ask_Question("Skill", _Point_On_UI.gameObject.name, "Description");
                    }
                    break;
                case "Mob":
                    Control_Handler.GetComponent<Control_Handler>().Ask_Question("Mob", _Point_On_UI.gameObject.name, "Description");
                    break;

            }
            //---------------------------------------------------------------------------------------------------------------------
        }
    }

   



    int Players_Index = 0, Mob_Index = 0, Stuffs_Index = 0;
    public void Control_Handler_Search()
    {
        Players_Index = 0;
        Mob_Index = 0;
        Stuffs_Index = 0;
        Control_Handler.GetComponent<Control_Handler>().Search();
    }

    public void Roll_On_Node_Info(string _Name)
    {
        switch (_Name)
        {
            case "Players":
                if (Input.mouseScrollDelta.y < 0 && Things_On_Node.transform.GetChild(0).GetChild(Players_Index + 1).transform.localPosition.x > -5000)
                {
                    Players_Index++;
                    Roll_All_On_Node_Info("Players");
                }
                else if (Input.mouseScrollDelta.y > 0 && Players_Index > 0)
                {
                    Players_Index--;
                    Roll_All_On_Node_Info("Players");
                }
                break;
            case "Mobs":
                if (Input.mouseScrollDelta.y < 0 && Things_On_Node.transform.GetChild(1).GetChild(Mob_Index + 1).transform.localPosition.x > -5000)
                {
                    Mob_Index++;
                    Roll_All_On_Node_Info("Mobs");
                }
                else if (Input.mouseScrollDelta.y > 0 && Mob_Index > 0)
                {
                    Mob_Index--;
                    Roll_All_On_Node_Info("Mobs");
                }
                break;
            case "Stuffs":
                if (Input.mouseScrollDelta.y < 0 && Things_On_Node.transform.GetChild(2).GetChild(Stuffs_Index + 1).transform.localPosition.x > -5000)
                {
                    Stuffs_Index++;
                    Roll_All_On_Node_Info("Stuffs");
                }
                else if (Input.mouseScrollDelta.y > 0 && Stuffs_Index > 0)
                {
                    Stuffs_Index--;
                    Roll_All_On_Node_Info("Stuffs");
                }
                break;
        }

    }

    public void Roll_All_On_Node_Info(string _Type)
    {
        int _temp = 0;
        switch (_Type)
        {
            case "Players":
                while (Things_On_Node.transform.GetChild(0).GetChild(_temp).transform.localPosition.x > -5000)
                {
                    Things_On_Node.transform.GetChild(0).GetChild(_temp).GetComponent<On_Node_Thing>().Move_To_Pos(Players_Index);
                    _temp++;
                }
                break;
            case "Mobs":
                while (Things_On_Node.transform.GetChild(1).GetChild(_temp).transform.localPosition.x > -5000)
                {
                    Things_On_Node.transform.GetChild(1).GetChild(_temp).GetComponent<On_Node_Thing>().Move_To_Pos(Mob_Index);
                    _temp++;
                }
                break;
            case "Stuffs":
                while (Things_On_Node.transform.GetChild(2).GetChild(_temp).transform.localPosition.x > -5000)
                {
                    Things_On_Node.transform.GetChild(2).GetChild(_temp).GetComponent<On_Node_Thing>().Move_To_Pos(Stuffs_Index);
                    _temp++;
                }
                break;
        }

    }

    public void Control_Handler_BackPack()
    {
        Control_Handler.GetComponent<Control_Handler>().Open_BackPack();
    }

    int BackPack_Ins_Num = 100;
    public void BackPack_Ins(int _Num)
    {
        for (int i = 0; i < _Num; i++)
        {
            Instantiate(Back_Pack_Info_Prefab, new Vector3(-10000, -40, 0), Back_Pack_Info_Prefab.transform.rotation, BackPack_Info.transform);
        }
        BackPack_Ins_Num += _Num;
    }
    
    public List<string> Item_Names = new List<string>();
    public List<int> Item_Amounts = new List<int>();
    int BackPack_Index = 1;
    public void Add_BackPack_Info(string _Name, int _Amount)
    {
        Item_Names.Add(_Name);
        Item_Amounts.Add(_Amount);
    }

    public void Update_BackPack_Info()
    {
        if (Item_Names.Count > BackPack_Ins_Num)
        {
            BackPack_Ins(Item_Names.Count - BackPack_Ins_Num);
        }
        if (Item_Names.Count - BackPack_Index < 0)
        {
            BackPack_Index -= 3;
        }
        for (int i = 1; i <= Item_Names.Count; i++)
        {
            BackPack_Info.transform.GetChild(i).name = Item_Names[i - 1];
            BackPack_Info.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Item_Names[i - 1] + " X " + Item_Amounts[i - 1];
        }
        Roll_BackPack_Item();
    }

    public void Roll_BackPack_Item()
    {
        for (int i = 1, j = 0; i <= Item_Names.Count + 1; i++)
        {
            if (i >= BackPack_Index && j < 27 && i <= Item_Names.Count)
            {
                BackPack_Info.transform.GetChild(BackPack_Index + j).localPosition = new Vector3(-200 + ((j % 3) * 200), 300 - ((j / 3) * 80), 0);
                j++;
            }
            else
            {
                BackPack_Info.transform.GetChild(i).localPosition = new Vector3(-10000, -40, 0);
            }
        }
    }

    public void Control_Handler_Skill_Info()
    {
        Control_Handler.GetComponent<Control_Handler>().Show_Skill_Info();
    }

    public string Target_Type = "None";
    public int Target_Index;
    public void Control_Handler_Skill_Info(string _Apply_Type, int _Apply_Index)
    {
        Target_Type = _Apply_Type;
        Target_Index = _Apply_Index;
        Show_Or_Hide_Things_On_Node("Hide");
        Control_Handler.GetComponent<Control_Handler>().Show_Skill_Info();
    }

    bool Skill_Info_Showing = false;
    public void Show_Or_Hide_Skill_Info(string _Show_Or_Hide)
    {
        if (_Show_Or_Hide == "Show" && !Skill_Info_Showing)
        {
            Skill_Info.GetComponent<CanvasGroup>().alpha = 1;
            Skill_Info.GetComponent<CanvasGroup>().interactable = true;
            Skill_Info.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Skill_Info_Showing = true;
        }
        else if (_Show_Or_Hide == "Hide" && Skill_Info_Showing)
        {
            Skill_Info.GetComponent<CanvasGroup>().alpha = 0;
            Skill_Info.GetComponent<CanvasGroup>().interactable = false;
            Skill_Info.GetComponent<CanvasGroup>().blocksRaycasts = false;
            Skill_Info_Showing = false;
        }
    }

    int Skill_Info_Ins_Num = 100;
    public void Skill_Info_Ins(int _Num)
    {
        for (int i = 0; i < _Num; i++)
        {
            Instantiate(Skill_Info_Prefab, new Vector3(-10000, -40, 0), Skill_Info_Prefab.transform.rotation, Skill_Info.transform);
        }
        Skill_Info_Ins_Num += _Num;
    }

    public List<string> Skill_Names = new List<string>();
    public List<string> Skill_Descriptions = new List<string>();
    int Skill_Info_Index = 1;
    public void Add_Skill_Info(string _Name, string _Description)
    {
        Skill_Names.Add(_Name);
        Skill_Descriptions.Add(_Description);
    }

    public void Update_Skill_Info()
    {
        if (Skill_Names.Count > Skill_Info_Ins_Num)
        {
            Skill_Info_Ins(Skill_Names.Count - Skill_Info_Ins_Num);
        }
        if (Skill_Names.Count - Skill_Info_Index < 0)
        {
            Skill_Info_Index -= 1;
        }
        for (int i = 1; i <= Skill_Names.Count; i++)
        {
            Skill_Info.transform.GetChild(i).name = Skill_Names[i - 1];
            Skill_Info.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = Skill_Names[i-1];
            Skill_Info.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = " - " + Skill_Descriptions[i - 1];
        }
        Roll_Skill_Info();
    }

    public void Roll_Skill_Info()
    {
        for (int i = 1, j = 0; i <= Skill_Names.Count + 1; i++)
        {
            if (i >= Skill_Info_Index && j < 9 && i <= Skill_Names.Count)
            {
                Skill_Info.transform.GetChild(Skill_Info_Index + j).localPosition = new Vector3(-200 + ((j % 1) * 200), 300 - ((j / 1) * 80), 0);
                if (Target_Type == "None")
                {
                    Skill_Info.transform.GetChild(Skill_Info_Index + j).GetComponent<Button>().interactable = false;
                }
                else
                {
                    Skill_Info.transform.GetChild(Skill_Info_Index + j).GetComponent<Button>().interactable = true;
                }
                j++;
            }
            else
            {
                Skill_Info.transform.GetChild(i).localPosition = new Vector3(-10000, -40, 0);
            }
        }
    }

    bool BackPack_Showing = false;
    public void Show_Or_Hide_BackPack(string _Show_Or_Hide)
    {
        if (_Show_Or_Hide == "Show" && !BackPack_Showing)
        {
            BackPack_Info.GetComponent<CanvasGroup>().alpha = 1;
            BackPack_Info.GetComponent<CanvasGroup>().interactable = true;
            BackPack_Info.GetComponent<CanvasGroup>().blocksRaycasts = true;
            BackPack_Showing = true;
        }
        else if (_Show_Or_Hide == "Hide" && BackPack_Showing)
        {
            BackPack_Info.GetComponent<CanvasGroup>().alpha = 0;
            BackPack_Info.GetComponent<CanvasGroup>().interactable = false;
            BackPack_Info.GetComponent<CanvasGroup>().blocksRaycasts = false;
            BackPack_Showing = false;
        }
    }

    public void Show_Or_Hide_Things_On_Node(string _Show_Or_Hide)
    {
        if (_Show_Or_Hide == "Show")
        {
            Things_On_Node.GetComponent<CanvasGroup>().alpha = 1;
            Things_On_Node.GetComponent<CanvasGroup>().interactable = true;
            Things_On_Node.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if(_Show_Or_Hide == "Hide")
        {
            Things_On_Node.GetComponent<CanvasGroup>().alpha = 0;
            Things_On_Node.GetComponent<CanvasGroup>().interactable = false;
            Things_On_Node.GetComponent<CanvasGroup>().blocksRaycasts = false;

            for (int i = 0; i < Things_On_Node.transform.GetChild(0).childCount; i++)
            {
                Things_On_Node.transform.GetChild(0).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }
            for (int i = 0; i < Things_On_Node.transform.GetChild(1).childCount; i++)
            {
                Things_On_Node.transform.GetChild(1).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }
            for (int i = 0; i < Things_On_Node.transform.GetChild(2).childCount; i++)
            {
                Things_On_Node.transform.GetChild(2).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }

        }
        else
        {
            for (int i = 0; i < Things_On_Node.transform.GetChild(0).childCount; i++)
            {
                Things_On_Node.transform.GetChild(0).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }
            for (int i = 0; i < Things_On_Node.transform.GetChild(1).childCount; i++)
            {
                Things_On_Node.transform.GetChild(1).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }
            for (int i = 0; i < Things_On_Node.transform.GetChild(2).childCount; i++)
            {
                Things_On_Node.transform.GetChild(2).GetChild(i).GetComponent<On_Node_Thing>().Move_Away();
            }
            Control_Handler_Search();
        }
    }
}
