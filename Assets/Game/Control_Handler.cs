using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Mob_stuff;

public class Control_Handler : MonoBehaviour
{

    GameObject Local_Figure;
    //public int Belong_Player_ID;
    PhotonView photonView;
    int layerMask;
    GameObject Showing_Info_Node;
    GameObject UI;
    Map_Manager MM;
    Mob_Manager MobM;
    public GameObject Chronicle;


    //------------------------------------------------------------------------------Fairy_AI
    Item Gear_Average, Food_Average;
    GameObject Fairy;

    void Count_Item_Average()
    {
        Gear_Average = new Item();
        foreach (Item _Item in MobM.Item_Kinds)
        {
            if (_Item.Type == "Gear")
            {
                Gear_Average.Hp_Plus += _Item.Hp_Plus;
                Gear_Average.Mp_Plus += _Item.Mp_Plus;
                Gear_Average.Strength_Plus += _Item.Strength_Plus;
                Gear_Average.Agility_Plus += _Item.Agility_Plus;
                Gear_Average.Wit_Plus += _Item.Wit_Plus;
                Gear_Average.Hp_Multiply += _Item.Hp_Multiply;
                Gear_Average.Mp_Multiply += _Item.Mp_Multiply;
                Gear_Average.Strength_Multiply += _Item.Strength_Multiply;
                Gear_Average.Agility_Multiply += _Item.Agility_Multiply;
                Gear_Average.Wit_Multiply += _Item.Wit_Multiply;
            }
        }
        Gear_Average.Hp_Plus /= MobM.Item_Kinds.Count;
        Gear_Average.Mp_Plus /= MobM.Item_Kinds.Count;
        Gear_Average.Strength_Plus /= MobM.Item_Kinds.Count;
        Gear_Average.Agility_Plus /= MobM.Item_Kinds.Count;
        Gear_Average.Wit_Plus /= MobM.Item_Kinds.Count;
        Gear_Average.Hp_Multiply /= MobM.Item_Kinds.Count;
        Gear_Average.Mp_Multiply /= MobM.Item_Kinds.Count;
        Gear_Average.Strength_Multiply /= MobM.Item_Kinds.Count;
        Gear_Average.Agility_Multiply /= MobM.Item_Kinds.Count;
        Gear_Average.Wit_Multiply /= MobM.Item_Kinds.Count;

        Food_Average = new Item();
        foreach (Item _Item in MobM.Item_Kinds)
        {
            if (_Item.Type == "Food")
            {
                Food_Average.Hp_Plus += _Item.Hp_Plus;
                Food_Average.Mp_Plus += _Item.Mp_Plus;
                Food_Average.Strength_Plus += _Item.Strength_Plus;
                Food_Average.Agility_Plus += _Item.Agility_Plus;
                Food_Average.Wit_Plus += _Item.Wit_Plus;
                Food_Average.Hp_Multiply += _Item.Hp_Multiply;
                Food_Average.Mp_Multiply += _Item.Mp_Multiply;
                Food_Average.Strength_Multiply += _Item.Strength_Multiply;
                Food_Average.Agility_Multiply += _Item.Agility_Multiply;
                Food_Average.Wit_Multiply += _Item.Wit_Multiply;
            }
        }
        Food_Average.Hp_Plus /= MobM.Item_Kinds.Count;
        Food_Average.Mp_Plus /= MobM.Item_Kinds.Count;
        Food_Average.Strength_Plus /= MobM.Item_Kinds.Count;
        Food_Average.Agility_Plus /= MobM.Item_Kinds.Count;
        Food_Average.Wit_Plus /= MobM.Item_Kinds.Count;
        Food_Average.Hp_Multiply /= MobM.Item_Kinds.Count;
        Food_Average.Mp_Multiply /= MobM.Item_Kinds.Count;
        Food_Average.Strength_Multiply /= MobM.Item_Kinds.Count;
        Food_Average.Agility_Multiply /= MobM.Item_Kinds.Count;
        Food_Average.Wit_Multiply /= MobM.Item_Kinds.Count;
    }

    string Value_To_Word(string _Value_Type, string _Target)
    {
        switch (_Target)
        {
            case "Gear":
                switch (_Value_Type)
                {
                    case "Hp":
                        return "makes you tankier";
                    case "Mp":
                        return "makes you cast more";
                    case "Strength":
                        return "makes you stronger";
                    case "Agility":
                        return "makes you faster";
                    case "Wit":
                        return "makes you smarter";
                }
                break;
            case "Food":
                switch (_Value_Type)
                {
                    case "Hp":
                        return "is good for your health";
                    case "Mp":
                        return "looks magical";
                    case "Strength":
                        return "smells so manly";
                    case "Agility":
                        return "makes you faster";
                    case "Wit":
                        return "makes you smarter";
                }
                break;
        }

        return "";
    }

    public void Ask_Question(string _Tag, string _Name, string _Ask_Type)
    {
        photonView.RPC("Master_Ask_Question", PhotonNetwork.MasterClient, _Tag, _Name, _Ask_Type, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    public void Master_Ask_Question(string _Tag, string _Name, string _Ask_Type, string _Order_Player)
    {
        string _Ans = "";

        switch (_Ask_Type)
        {
            case "Description":
                switch (_Tag)
                {
                    case "Item":
                        Item _Item = MobM.Item_Kinds.Find(_I => _I.Name == _Name);
                        _Ans = _Item.Description;
                        break;
                    case "Skill":
                        Skill _Skill = MobM.Skill_Kinds.Find(_S => _S.Name == _Name);
                        _Ans = _Skill.Description;
                        break;
                    case "Mob":
                        Mob _Mob;
                        if (MobM.Mob_Kinds.Exists(_M => _M.Name == _Name))
                        {
                            _Mob = MobM.Mob_Kinds.Find(_M => _M.Name == _Name);
                        }
                        else
                        {
                            _Mob = MobM.Player_Mob_Kinds.Find(_M => _M.Name == _Name);
                        }
                        _Ans = _Mob.Description;
                        break;
                }
                break;
            case "Consult":
                List<string> _Pro = new List<string>();
                List<string> _Con = new List<string>();
                switch (_Tag)
                {
                    case "Item":
                        Item _Item = MobM.Item_Kinds.Find(_I => _I.Name == _Name);
                        if (_Item.Hp_Plus > Gear_Average.Hp_Plus || _Item.Hp_Multiply > Gear_Average.Hp_Multiply)
                        {
                            _Pro.Add("Hp");
                        }
                        else
                        {
                            _Con.Add("Hp");
                        }
                        if (_Item.Mp_Plus > Gear_Average.Mp_Plus || _Item.Mp_Multiply > Gear_Average.Mp_Multiply)
                        {
                            _Pro.Add("Mp");
                        }
                        else
                        {
                            _Con.Add("Mp");
                        }
                        if (_Item.Strength_Plus > Gear_Average.Strength_Plus || _Item.Strength_Multiply > Gear_Average.Strength_Multiply)
                        {
                            _Pro.Add("Strength");
                        }
                        else
                        {
                            _Con.Add("Strength");
                        }
                        if (_Item.Agility_Plus > Gear_Average.Agility_Plus || _Item.Agility_Multiply > Gear_Average.Agility_Multiply)
                        {
                            _Pro.Add("Agility");
                        }
                        else
                        {
                            _Con.Add("Agility");
                        }
                        if (_Item.Wit_Plus > Gear_Average.Wit_Plus || _Item.Wit_Multiply > Gear_Average.Wit_Multiply)
                        {
                            _Pro.Add("Wit");
                        }
                        else
                        {
                            _Con.Add("Wit");
                        }

                        switch (_Item.Type)
                        {
                            case "Gear":
                                switch (_Pro.Count)
                                {
                                    case 0:
                                        _Ans = "It's not impressive at all";
                                        break;
                                    case 1:
                                        _Ans = "Hmm~ I think it " + Value_To_Word(_Pro[0], "Gear") + ".";
                                        break;
                                    case 2:
                                        _Ans = "Oh~ This one " + Value_To_Word(_Pro[0], "Gear") + " and " + Value_To_Word(_Pro[1], "Gear") + ".";
                                        break;
                                    case 3:
                                        _Ans = "Oh~ This one " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + " and " + Value_To_Word(_Pro[2], "Gear") + ", not quit bad~ ";
                                        break;
                                    case 4:
                                        _Ans = "Wow~ It " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + ", " + Value_To_Word(_Pro[2], "Gear") + " and " + Value_To_Word(_Pro[3], "Gear") + ", very decent one~ ";
                                        break;
                                    case 5:
                                        _Ans = "Oh Wow~ Not only " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + ", " + Value_To_Word(_Pro[2], "Gear") + ", " + Value_To_Word(_Pro[3], "Gear") + " but also " + Value_To_Word(_Pro[4], "Gear") + ", this is what I call a masterpiece~ ";
                                        break;
                                }

                                break;
                            case "Food":
                                switch (_Pro.Count)
                                {
                                    case 0:
                                        _Ans = "Are you sure you are going to eat this?";
                                        break;
                                    case 1:
                                        _Ans = "Hmm~ Something " + Value_To_Word(_Pro[0], "Gear") + ".";
                                        break;
                                    case 2:
                                        _Ans = "Oh~ This one " + Value_To_Word(_Pro[0], "Gear") + " and " + Value_To_Word(_Pro[1], "Gear") + ".";
                                        break;
                                    case 3:
                                        _Ans = "Oh~ This one " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + " and " + Value_To_Word(_Pro[2], "Gear") + ", Samlls good~ ";
                                        break;
                                    case 4:
                                        _Ans = "Wow~ It " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + ", " + Value_To_Word(_Pro[2], "Gear") + " and " + Value_To_Word(_Pro[3], "Gear") + ", look delicious~ ";
                                        break;
                                    case 5:
                                        _Ans = "Oh Wow~ Not only " + Value_To_Word(_Pro[0], "Gear") + ", " + Value_To_Word(_Pro[1], "Gear") + ", " + Value_To_Word(_Pro[2], "Gear") + ", " + Value_To_Word(_Pro[3], "Gear") + " but also " + Value_To_Word(_Pro[4], "Gear") + ", bon appetit~ ";
                                        break;
                                }
                                break;
                        }
                        break;
                    case "Skill":
                        Skill _Skill = MobM.Skill_Kinds.Find(_S => _S.Name == _Name);
                        _Ans = _Skill.Description;
                        break;
                }
                break;
        }

        photonView.RPC("Fairy_Answer_Question", Find_Player_By_Name(_Order_Player), _Ans);
    }

    [PunRPC]
    void Fairy_Answer_Question(string _Ans)
    {
        Fairy.GetComponent<Fairy>().Ask_To_Speak(_Ans);
    }
    //------------------------------------------------------------------------------Fairy_AI


    void Start()
    {
        MM = GameObject.Find("Map_Manager").GetComponent<Map_Manager>();
        if (PhotonNetwork.IsMasterClient)
        {
            MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
            MM = GameObject.Find("Map_Manager").GetComponent<Map_Manager>();
            Count_Item_Average();
        }
        else
        {
            StartCoroutine(MM.GetComponent<Map_Manager>().Inis_Point(300));
        }

        Fairy = GameObject.Find("Fairy");
        UI = GameObject.Find("UI");
        photonView = PhotonView.Get(this);
        Ask_For_BK();
        PhotonNetwork.LocalPlayer.NickName = GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Name;
        Local_Creat_My_Figure();
        layerMask = 1 << 8;

        //Belong_Player_ID = PhotonNetwork.LocalPlayer.ActorNumber - 1;
    }

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.NickName.Contains("Figure"))
        {
            PhotonNetwork.LocalPlayer.NickName = GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Name;
        }
        Send_Show_Node_Route();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Pick_Next_Node();
        }
    }

    public void Pick_Next_Node()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
        if (hit.collider != null)
        {
            Debug.Log("In1");
            if (hit.collider.GetComponent<Map_Node>() != null)
            {
                Debug.Log("In2");
                photonView.RPC("Figure_Move", PhotonNetwork.MasterClient, hit.collider.GetComponent<PhotonView>().ViewID, PhotonNetwork.LocalPlayer.NickName);
            }
        }
    }

    public void Send_Show_Node_Route()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<Map_Node>() != null)
            {
                photonView.RPC("Master_Show_Node_Route", PhotonNetwork.MasterClient, hit.collider.GetComponent<PhotonView>().ViewID, PhotonNetwork.LocalPlayer.NickName);
            }
        }
    }
    /*
    GameObject _Node;
    float _Timer = 0.1f;
    IEnumerator Current_Show_Node()
    {
        while (_Timer > 0)
        {
            yield return new WaitForSeconds(0.001f);
            _Timer -= 0.001f;
        }
        _Node = null;
    }
    */
    [PunRPC]
    public void Master_Show_Node_Route(int _Node_ID, string _Order_Player)
    {
        bool _Is_Last = false;
        string _All_Link_Name = "";
        GameObject _temp = PhotonView.Find(_Node_ID).gameObject;
        for (int i = 0; i < _temp.GetComponent<Map_Node>().This_Node.Outs.Count; i++)
        {
            if (i == _temp.GetComponent<Map_Node>().This_Node.Outs.Count - 1)
            {
                _All_Link_Name += _temp.GetComponent<Map_Node>().This_Node.Out_Links_Game_Object[i].name;
            }
            else
            {
                _All_Link_Name += _temp.GetComponent<Map_Node>().This_Node.Out_Links_Game_Object[i].name + "|";
            }
        }

        for (int i = 0; i < _temp.GetComponent<Map_Node>().This_Node.Outs.Count; i++)
        {
            if (i == _temp.GetComponent<Map_Node>().This_Node.Outs.Count - 1)
            {
                _Is_Last = true;
            }

            Vector3 _Form_Pos = _temp.transform.position;
            Vector3 _To_Pos = MM.GetComponent<Map_Manager>().Nodes[_temp.GetComponent<Map_Node>().This_Node.Outs[i]].Pos;
            photonView.RPC("Local_Move_Node_Route_Point", Find_Player_By_Name(_Order_Player), _temp.GetComponent<Map_Node>().This_Node.Out_Links_Game_Object[i].name, _Form_Pos.x, _Form_Pos.y, _Form_Pos.z, _To_Pos.x, _To_Pos.y, _To_Pos.z, _All_Link_Name);
        }
    }

    /*
    [PunRPC]
    public void Master_Show_Node_Route(int _Node_ID, string _Order_Player)
    {
        foreach (GameObject _Out_Link in PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Out_Links_Game_Object)
        {
            photonView.RPC("Local_Show_Node_Route", Find_Player_By_Name(_Order_Player), _Out_Link.name);
        }
    }
    */

    float Hight_of_Link_Point(Vector3 _A, Vector3 _B, float _X)
    {
        float _Dis = Vector3.Distance(_B, _A) / 2, a;
        a = 2 / (_Dis * _Dis);
        return a * (_X * _X) - 2;
    }

    [PunRPC]
    public void Local_Move_Node_Route_Point(string _Link_Name, float _From_x, float _From_y, float _From_z, float _To_x, float _To_y, float _To_z, string _All_List_Name)
    {
        /*
        foreach (string _S in _All_List_Name.Split('|'))
        {
            if (GameObject.Find(_Link_Name).GetComponent<Map_Link>().Is_Showing)
            {
                return;
            }
        }
        */
        if (GameObject.Find(_Link_Name).GetComponent<Map_Link>().Is_Preparing == 0)
        {
            GameObject.Find(_Link_Name).GetComponent<Map_Link>().Is_Preparing = 1;
        }
        else
        {
            return;
        }

        Vector3 _Form_Pos = new Vector3(_From_x, _From_y, _From_z);
        Vector3 _To_Pos = new Vector3(_To_x, _To_y, _To_z);

        Vector3 _temp = (_To_Pos - _Form_Pos).normalized;
        Vector3 Mid = (_Form_Pos + _To_Pos) / 2;

        GameObject _temp3;
        Vector3 _temp_Vector;
        GameObject _temp_Point;
        for (int j = 0; Vector3.Distance(Mid + _temp * j, _To_Pos) >= 1; j++)
        {
            Vector3 _temp2 = Quaternion.Euler(0, 0, -90) * _temp;

            _temp_Vector = Mid + _temp * j + Hight_of_Link_Point(_Form_Pos, _To_Pos, j) * _temp2;

            _temp_Point = MM.Take_Point();
            _temp_Point.transform.position = _temp_Vector;
            _temp_Point.transform.SetParent(GameObject.Find(_Link_Name).transform);
            _temp_Point.transform.SetAsLastSibling();
            if (j != 0)
            {
                _temp_Vector = Mid - _temp * j + Hight_of_Link_Point(_Form_Pos, _To_Pos, j) * _temp2;

                _temp_Point = MM.Take_Point();
                _temp_Point.transform.position = _temp_Vector;
                _temp_Point.transform.SetParent(GameObject.Find(_Link_Name).transform);
                _temp_Point.transform.SetAsFirstSibling();
            }

        }
        GameObject.Find(_Link_Name).GetComponent<Map_Link>().Is_Preparing = 2;
        foreach (string _S in _All_List_Name.Split('|'))
        {
            if (GameObject.Find(_S).GetComponent<Map_Link>().Is_Preparing != 2)
            {
                return;
            }
        }
        foreach (string _S in _All_List_Name.Split('|'))
        {
            GameObject.Find(_S).GetComponent<Map_Link>().Ask_Show_Route();
        }
    }
    /*
    [PunRPC]
    public void Local_Show_Node_Route(string _Link_Name)
    {
        GameObject.Find(_Link_Name).GetComponent<Map_Link>().Ask_Show_Route();
    }
    */
    public void Search()
    {
        photonView.RPC("Master_Things_On_Node_Show_Info", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    public void Open_BackPack()
    {
        UI.GetComponent<UI>().Item_Names.Clear();
        UI.GetComponent<UI>().Item_Amounts.Clear();
        photonView.RPC("Master_BackPack_Show_Info", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    public void Show_Skill_Info()
    {
        UI.GetComponent<UI>().Skill_Names.Clear();
        UI.GetComponent<UI>().Skill_Descriptions.Clear();
        photonView.RPC("Master_Skill_Show_Info", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    public void Click_Item(string _Item_Name)
    {
        UI.GetComponent<UI>().Item_Names.Clear();
        UI.GetComponent<UI>().Item_Amounts.Clear();
        photonView.RPC("Figure_Click_Item", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName, _Item_Name);
        photonView.RPC("Master_BackPack_Show_Info", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }
    /*
    public void Cast_Show_Skill(string _Reply_Type, int _Reply_Index)
    {
        photonView.RPC("Master_Cast_Show_Skill", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName, _Reply_Type, _Reply_Index);
    }
    */
    public void Cast(string _Skill_Name, string _Apply_Mob_Type, int _Apply_Mob)
    {
        Debug.Log("_Skill_Name: " + _Skill_Name);
        photonView.RPC("Figure_Cast_Skill", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName, _Skill_Name, _Apply_Mob_Type, _Apply_Mob);
    }

    public void Pick(int _Item)
    {
        photonView.RPC("Figure_Pick_Up_Item", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName, _Item);
    }

    public void Show_Chronicle_Trigger()
    {
        photonView.RPC("Master_Show_Chronicle", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    public void End()
    {
        photonView.RPC("End_Turn", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    public void End_Turn(string _Figure_Name)
    {
        GameObject.Find(_Figure_Name).GetComponent<Figure>().Round_Step = 0;
        GameObject.Find(_Figure_Name).GetComponent<Figure>().End_Turn();
    }

    [PunRPC]
    void Figure_Click_Item(string _Figure_Name, string _Item_Name)
    {
        if (GameObject.Find(_Figure_Name).GetComponent<Figure>().Can_Be_Control)
        {
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Round_Step_Consum();
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Click_Item(_Item_Name);
        }
    }
    [PunRPC]
    void Figure_Move(int _Node_View_ID, string _Figure_Name)
    {
        int _Node_ID = PhotonView.Find(_Node_View_ID).GetComponent<Map_Node>().This_Node.ID;
        Debug.Log("Can he move? " + GameObject.Find(_Figure_Name).GetComponent<Figure>().Can_Be_Control);
        if (GameObject.Find(_Figure_Name).GetComponent<Figure>().Can_Be_Control)
        {
            int _Out_ID = GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Outs.FindIndex(_N => _N == _Node_ID);
            if (_Out_ID != -1)
            {
                Debug.Log("He moved? ");
                StartCoroutine(GameObject.Find(_Figure_Name).GetComponent<Figure>().Move(_Out_ID));
            }
        }
    }
    /*
    //-------------------------------------------------
    [PunRPC]
    void Master_Cast_Show_Skill(string _Order_Player, string _Reply_Type, int _Reply_Index)
    {
        List<string> _Skill_Names = new List<string>();
        foreach (Skill _Skill in GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Skills)
        {
            _Skill_Names.Add(_Skill.Name);
        }
        photonView.RPC("Cast_Show_Skill", Find_Player_By_Name(_Order_Player), _Skill_Names, _Reply_Type, _Reply_Index);
    }
    [PunRPC]
    void Cast_Show_Skill(List<string> _Skill_Names, string _Reply_Type, int _Reply_Index)
    {
        switch (_Reply_Type)
        {
            case "Player":
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(0).GetChild(_Reply_Index).GetComponent<On_Node_Thing>().Show_Cast_Skill(_Skill_Names);
                break;
            case "Mob":
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(1).GetChild(_Reply_Index).GetComponent<On_Node_Thing>().Show_Cast_Skill(_Skill_Names);
                break;
        }    
    }
    //-------------------------------------------------
    */
    [PunRPC]
    void Figure_Cast_Skill(string _Figure_Name, string _Skill_Name, string _Apply_Mob_Type, int _Apply_Mob)
    {
        if (GameObject.Find(_Figure_Name).GetComponent<Figure>().Can_Be_Control)
        {
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Round_Step_Consum();
            switch (_Apply_Mob_Type)
            {
                case "Player":
                    GameObject.Find(_Figure_Name).GetComponent<Figure>().Cast_Skill(GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Players[_Apply_Mob], GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Skills.Find(_S => _S.Name == _Skill_Name));
                    break;
                case "Mob":
                    Debug.Log("1:" + GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.ID);
                    Debug.Log("2:" + GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Mobs[_Apply_Mob].GetComponent<Mob_Behavior>().This_Mob.Node_Now.ID);
                    //Debug.Log("3:" + GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Mobs[_Apply_Mob]);
                    GameObject.Find(_Figure_Name).GetComponent<Figure>().Cast_Skill(GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Mobs[_Apply_Mob], GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Skills.Find(_S => _S.Name == _Skill_Name));
                    break;
            }
        }
    }
    [PunRPC]
    void Figure_Pick_Up_Item(string _Figure_Name, int _Item)
    {
        if (GameObject.Find(_Figure_Name).GetComponent<Figure>().Can_Be_Control)
        {
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Round_Step_Consum();
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Pick_Up_Item(GameObject.Find(_Figure_Name).GetComponent<Figure>().This_Mob.Node_Now.Stuffs[_Item]);
            photonView.RPC("Things_On_Node_Update_Info", Find_Player_By_Name(_Figure_Name));
        }
    }

    //-------------------------------------------------
    [PunRPC]
    void Master_Map_Node_Show_Info(int _Node_ID, string _Order_Player)
    {
        string _temp = "";

        for (int i = 0; i < PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Players.Count; i++)
        {
            _temp += PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Players[i].name + " ";
        }

        for (int i = 0; i < PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Mobs.Count; i++)
        {
            _temp += PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Mobs[i].name + " ";
        }

        photonView.RPC("Map_Node_Show_Info", Find_Player_By_Name(_Order_Player), _Node_ID, _temp);
    }
    [PunRPC]
    void Map_Node_Show_Info(int _Node_ID, string _Info)
    {
        PhotonView.Find(_Node_ID).GetComponent<Map_Node>().Show_Info(_Info);
    }
    //-------------------------------------------------
    [PunRPC]
    void Master_Map_Node_Hide_Info(int _Node_ID, string _Order_Player)
    {
        photonView.RPC("Map_Node_Hide_Info", Find_Player_By_Name(_Order_Player), _Node_ID);
    }
    [PunRPC]
    void Map_Node_Hide_Info(int _Node_ID)
    {
        PhotonView.Find(_Node_ID).GetComponent<Map_Node>().Hide_Info();
    }
    //-------------------------------------------------
    //-------------------------------------------------
    List<string> Already_Ins_Players = new List<string>();
    [PunRPC]
    void Master_Things_On_Node_Show_Info(string _Order_Player)
    {
        GameObject _Figure = GameObject.Find(_Order_Player);

        if (_Figure.GetComponent<Figure>().Can_Be_Control)
        {
            /*
            Debug.Log(GameObject.Find(_Order_Player));
            Debug.Log(GameObject.Find(_Order_Player).GetComponent<Figure>());
            Debug.Log(GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob);
            Debug.Log(GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Node_Now);
            Debug.Log(GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Node_Now.Game_Object);
            Debug.Log(GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Node_Now.Game_Object.GetComponent<PhotonView>());
            */
            _Figure.GetComponent<Figure>().Round_Step_Consum();
            int _Node_ID = _Figure.GetComponent<Figure>().This_Mob.Node_Now.Game_Object.GetComponent<PhotonView>().ViewID;
            if (!Already_Ins_Players.Exists(_A => _A == _Order_Player))
            {
                Already_Ins_Players.Add(_Order_Player);
                for (int i = 0; i < 50; i++)
                {
                    photonView.RPC("Things_On_Node_Show_Info_Ins", Find_Player_By_Name(_Order_Player), "Player", i);
                }
                for (int i = 0; i < 50; i++)
                {
                    photonView.RPC("Things_On_Node_Show_Info_Ins", Find_Player_By_Name(_Order_Player), "Mob", i);
                }
                for (int i = 0; i < 50; i++)
                {
                    photonView.RPC("Things_On_Node_Show_Info_Ins", Find_Player_By_Name(_Order_Player), "Stuff", i);
                }
            }

            for (int i = 0; i < PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Players.Count; i++)
            {
                photonView.RPC("Things_On_Node_Show_Info_Move", Find_Player_By_Name(_Order_Player), "Player", i, PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Players[i].name);
            }
            for (int i = 0; i < PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Mobs.Count; i++)
            {
                photonView.RPC("Things_On_Node_Show_Info_Move", Find_Player_By_Name(_Order_Player), "Mob", i, PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Mobs[i].name);
            }
            for (int i = 0; i < PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Stuffs.Count; i++)
            {
                photonView.RPC("Things_On_Node_Show_Info_Move", Find_Player_By_Name(_Order_Player), "Stuff", i, PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Stuffs[i].Name);
            }
            photonView.RPC("Things_On_Node_Show_Info", Find_Player_By_Name(_Order_Player), PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.Type + "_" + PhotonView.Find(_Node_ID).GetComponent<Map_Node>().This_Node.ID);
        }
    }
    [PunRPC]
    void Things_On_Node_Show_Info_Ins(string _Type, int _Index)
    {
        GameObject _temp;
        switch (_Type)
        {
            case "Player":
                _temp = Instantiate(UI.GetComponent<UI>().Things_On_Node.GetComponent<Things_On_Node>().On_Node_Player);
                _temp.transform.SetParent(UI.GetComponent<UI>().Things_On_Node.transform.GetChild(0), false);
                _temp.GetComponent<On_Node_Thing>().Type = "Player";
                _temp.GetComponent<On_Node_Thing>().Index = _Index;
                _temp.GetComponent<RectTransform>().localPosition = new Vector3(-10000, -40, 0);

                break;
            case "Mob":
                _temp = Instantiate(UI.GetComponent<UI>().Things_On_Node.GetComponent<Things_On_Node>().On_Node_Mob);
                _temp.transform.SetParent(UI.GetComponent<UI>().Things_On_Node.transform.GetChild(1), false);
                _temp.GetComponent<On_Node_Thing>().Type = "Mob";
                _temp.GetComponent<On_Node_Thing>().Index = _Index;
                _temp.GetComponent<RectTransform>().localPosition = new Vector3(-10000, -40, 0);
                break;
            case "Stuff":
                _temp = Instantiate(UI.GetComponent<UI>().Things_On_Node.GetComponent<Things_On_Node>().On_Node_Stuff);
                _temp.transform.SetParent(UI.GetComponent<UI>().Things_On_Node.transform.GetChild(2), false);
                _temp.GetComponent<On_Node_Thing>().Type = "Stuff";
                _temp.GetComponent<On_Node_Thing>().Index = _Index;
                _temp.GetComponent<RectTransform>().localPosition = new Vector3(-10000, -40, 0);
                break;
        }


    }

    [PunRPC]
    void Things_On_Node_Show_Info_Move(string _Type, int _Index, string _Name)
    {
        switch (_Type)
        {
            case "Player":
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(0).GetChild(_Index).GetComponent<On_Node_Thing>().Move_To_Pos(0);
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(0).GetChild(_Index).GetComponent<Text>().text = _Name;
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(0).GetChild(_Index).name = _Name;
                break;
            case "Mob":
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(1).GetChild(_Index).GetComponent<On_Node_Thing>().Move_To_Pos(0);
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(1).GetChild(_Index).GetComponent<Text>().text = _Name;
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(1).GetChild(_Index).name = _Name;
                break;
            case "Stuff":
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(2).GetChild(_Index).GetComponent<On_Node_Thing>().Move_To_Pos(0);
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(2).GetChild(_Index).GetComponent<Text>().text = _Name;
                UI.GetComponent<UI>().Things_On_Node.transform.GetChild(2).GetChild(_Index).name = _Name;
                break;
        }
    }
    [PunRPC]
    void Things_On_Node_Show_Info(string _Node_Name)
    {
        UI.GetComponent<UI>().Things_On_Node.transform.GetChild(3).GetComponent<Text>().text = _Node_Name;
        UI.GetComponent<UI>().Show_Or_Hide_Things_On_Node("Show");
    }
    [PunRPC]
    void Things_On_Node_Update_Info()
    {
        UI.GetComponent<UI>().Show_Or_Hide_Things_On_Node("Update");
    }
    //-------------------------------------------------

    //-------------------------------------------------
    [PunRPC]
    void Master_BackPack_Show_Info(string _Order_Player)
    {
        List<string> _Item_Names = new List<string>();
        List<int> _Item_Amounts = new List<int>();
        foreach (Item _Item in GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Items)
        {
            int _temp = _Item_Names.FindIndex(_I => _I == _Item.Name);
            if (_temp == -1)
            {
                _Item_Names.Add(_Item.Name);
                _Item_Amounts.Add(1);
            }
            else
            {
                _Item_Amounts[_temp]++;
            }
        }
        for (int i = 0; i < _Item_Names.Count; i++)
        {
            photonView.RPC("BackPack_Add_Info", Find_Player_By_Name(_Order_Player), _Item_Names[i], _Item_Amounts[i]);
        }
        photonView.RPC("BackPack_Show_Info", Find_Player_By_Name(_Order_Player));
    }
    [PunRPC]
    void BackPack_Add_Info(string _Item_Name, int _Item_Amount)
    {
        UI.GetComponent<UI>().Add_BackPack_Info(_Item_Name, _Item_Amount);
    }
    [PunRPC]
    void BackPack_Show_Info()
    {
        UI.GetComponent<UI>().Update_BackPack_Info();
        UI.GetComponent<UI>().Show_Or_Hide_BackPack("Show");
    }
    //-------------------------------------------------

    //-------------------------------------------------
    [PunRPC]
    void Master_Skill_Show_Info(string _Order_Player)
    {
        List<string> _Skill_Names = new List<string>();
        List<string> _Skill_Descriptions = new List<string>();
        foreach (Skill _Skill in GameObject.Find(_Order_Player).GetComponent<Figure>().This_Mob.Skills)
        {
            _Skill_Names.Add(_Skill.Name);
            _Skill_Descriptions.Add(_Skill.Description);
        }
        for (int i = 0; i < _Skill_Names.Count; i++)
        {
            photonView.RPC("Skill_Add_Info", Find_Player_By_Name(_Order_Player), _Skill_Names[i], _Skill_Descriptions[i]);
        }
        photonView.RPC("Skill_Show_Info", Find_Player_By_Name(_Order_Player));
    }
    [PunRPC]
    void Skill_Add_Info(string _Skill_Name, string _Skill_Description)
    {
        UI.GetComponent<UI>().Add_Skill_Info(_Skill_Name, _Skill_Description);
    }
    [PunRPC]
    void Skill_Show_Info()
    {
        UI.GetComponent<UI>().Update_Skill_Info();
        UI.GetComponent<UI>().Show_Or_Hide_Skill_Info("Show");
    }
    //-------------------------------------------------

    //-------------------------------------------------
    [PunRPC]
    void Master_Show_Chronicle(string _Order_Player)
    {
        photonView.RPC("Show_Chronicle", Find_Player_By_Name(_Order_Player), Chronicle.GetComponent<Chronicle>().Content.text);
    }
    [PunRPC]
    void Show_Chronicle(string _Content)
    {
        Chronicle.GetComponent<Chronicle>().Content.text = _Content;
        Chronicle.GetComponent<Chronicle>().Show();
    }
    //-------------------------------------------------

    //-------------------------------------------------
    public void Ask_For_BK()
    {
        photonView.RPC("Master_Set_BK", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName);
    }

    [PunRPC]
    void Master_Set_BK(string _Order_Player)
    {
        photonView.RPC("Set_BK", Find_Player_By_Name(_Order_Player), GameObject.Find("Map_Manager").GetComponent<Map_Manager>().BK_url);
    }

    [PunRPC]
    void Set_BK(string _BK_url)
    {
        StartCoroutine(GameObject.Find("Map_Builder").GetComponent<Map_Bulider>().Get_BK(_BK_url));
    }
    //-------------------------------------------------
    //-------------------------------------------------
    public void Master_Set_Pic(string _Order_Player)
    {
        photonView.RPC("Set_Pic", Find_Player_By_Name(_Order_Player), GameObject.Find(_Order_Player).GetComponent<Figure>().Pic_url, GameObject.Find(_Order_Player).GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void Set_Pic(string _Pic_url, int _Figure_ID)
    {
        StartCoroutine(PhotonView.Find(_Figure_ID).GetComponent<Figure>().Set_Pic(_Pic_url));
    }
    //-------------------------------------------------
    public void Local_Creat_My_Figure()
    {
        string _Temp_A = "", _Temp_S = "", _Temp_I = "";
        for (int i = 0; i < GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Attributes.Count; i++)
        {
            _Temp_A += GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Attributes[i];
            if (i != GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Attributes.Count - 1)
            {
                _Temp_A += "|";
            }
            //photonView.RPC("Recive_My_Figure_Data", PhotonNetwork.MasterClient, "Attribute", GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Attributes[i]);
        }

        for (int i = 0; i < GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Skills.Count; i++)
        {
            if (GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Skills[i] != "Hit")
            {
                _Temp_S += GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Skills[i];
                if (i != GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Skills.Count - 1)
                {
                    _Temp_S += "|";
                }
                //photonView.RPC("Recive_My_Figure_Data", PhotonNetwork.MasterClient, "Skill", _S);
            }
        }
        for (int i = 0; i < GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Items.Count; i++)
        {
            _Temp_I += GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Items[i];
            if (i != GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Items.Count - 1)
            {
                _Temp_I += "|";
            }
            //photonView.RPC("Recive_My_Figure_Data", PhotonNetwork.MasterClient, "Item", _I);
        }

        photonView.RPC("Master_Creat_My_Figure", PhotonNetwork.MasterClient, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Name, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Hp, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Mp, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Strength, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Agility, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Wit, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Level, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure.Description, GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure_Url, _Temp_A, _Temp_S, _Temp_I);//要改成傳所有資料
    }

    /*
    [PunRPC]
    void Recive_My_Figure_Data(string _Type, string _Data)
    {
        switch (_Type)
        {
            case "Attribute":
                _temp_Attributes.Add(_Data);
                break;
            case "Skill":
                _temp_Skills.Add(_Data);
                break;
            case "Item":
                _temp_Items.Add(_Data);
                break;
        }
    }
    */
    [PunRPC]
    void Master_Creat_My_Figure(string _Figure_Name, int _Figure_Hp, int _Figure_Mp, int _Figure_Strength, int _Figure_Agility, int _Figure_Wit, int _Figure_Level, string _Figure_Description, string _Figure_Url, string _Temp_A, string _Temp_S, string _Temp_I)
    {
        List<string> _temp_Attributes = new List<string>(), _temp_Skills = new List<string>(), _temp_Items = new List<string>();
        Debug.LogError(_Figure_Name);
        //_temp_Attributes.Clear();
        _temp_Attributes.AddRange(_Temp_A.Split('|'));
        //_temp_Items.Clear();
        Debug.LogError(_Temp_I);
        _temp_Items.AddRange(_Temp_I.Split('|'));
        Debug.LogError(_temp_Items[0]);
        //_temp_Skills.Clear();
        _temp_Skills.AddRange(_Temp_S.Split('|'));
        //_temp_Attributes = _Temp_A;
        //_temp_Items = _Temp_I;
        //_temp_Skills = _Temp_S;
        
        StartCoroutine(GameObject.Find("Game_Manager").GetComponent<Game_Manager>().CreatPlayerPrefab(_Figure_Name, _Figure_Hp, _Figure_Mp, _Figure_Strength, _Figure_Agility, _Figure_Wit, _Figure_Level, _Figure_Description, _Figure_Url, _temp_Attributes, _temp_Items, _temp_Skills));
    }

    //-------------------------------------------------

    Photon.Realtime.Player Find_Player_By_Name(string _Name)
    {
        foreach (Photon.Realtime.Player _Player in PhotonNetwork.PlayerList)
        {
            if (_Player.NickName == _Name)
            {
                return _Player;
            }
        }
        Debug.Log("Null Player!!!!!!!!!");
        return null;
    }
}
