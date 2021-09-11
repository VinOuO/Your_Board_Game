using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Map_stuff;
using Mob_stuff;

public class Figure : MonoBehaviourPun
{
    public string Belong_Player_Name;
    public int Round_Step = 10;
    public bool Can_Be_Control = true;
    GameObject MM;
    GameObject GM;
    CameraWork CW;
    public Image Hp_UI, Mp_UI;
    public Text Strength_UI, Agility_UI, Wit_UI, Level_UI;
    bool Value_Setted_Up = false;
    int Strength_Value, Agility_Value, Wit_Value, Level_Value;
    Mob_Manager MobM;
    Figure_Manager FM;
    public Mob This_Mob;
    Text Chronicle_Content;
    public BroadCast BC;

    /// <summary>
    /// 0:Is moving 1:Is playing player 2:Rolled_Dice 3: Is not dead
    /// </summary>
    public bool[] Condition_List, Can_Move_Condition;
    //
    void Start()
    {
        CW = gameObject.GetComponent<CameraWork>();
        MM = GameObject.Find("Map_Manager");
        GM = GameObject.Find("Game_Manager");
        if (PhotonNetwork.IsMasterClient)
        {
            Chronicle_Content = GameObject.Find("Chronicle").GetComponent<Chronicle>().Content;
        }
        FM = GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>();

        StartCoroutine(Set_UIs());

        if (PhotonNetwork.IsMasterClient)
        {
            BC = GameObject.Find("BroadCast").GetComponent<BroadCast>();
            MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        }

        Can_Move_Condition[0] = false;
        Can_Move_Condition[1] = true;
        Can_Move_Condition[2] = true;
        Can_Move_Condition[3] = true;
        //StartCoroutine(Set_Pic("https://i.imgur.com/TQfsERQ.png"));

    }

    public IEnumerator Set_UIs()
    {
        while (GameObject.Find("Hp_UI") == null || GameObject.Find("Mp_UI") == null || GameObject.Find("Strength_UI") == null || GameObject.Find("Wit_UI") == null || GameObject.Find("Agility_UI") == null || GameObject.Find("Level_UI").GetComponent<Text>() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Value_Setted_Up = true;
        Hp_UI = GameObject.Find("Hp_UI").GetComponent<Image>();
        Mp_UI = GameObject.Find("Mp_UI").GetComponent<Image>();
        Strength_UI = GameObject.Find("Strength_UI").GetComponent<Text>();
        Agility_UI = GameObject.Find("Agility_UI").GetComponent<Text>();
        Wit_UI = GameObject.Find("Wit_UI").GetComponent<Text>();
        Level_UI = GameObject.Find("Level_UI").GetComponent<Text>();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            BC.Master_BroadCast_Type("Vincent uses Fire Ball.");
            BC.Master_BroadCast_Type("Flam-like orange ball fall from the sky and then hit the ground."); 
            BC.Master_BroadCast_Type("Raging fire rise around Slime and Slime can barely defence itself.");
            BC.Master_BroadCast_Type("Slime is severely injured.");
        }
        */
        if (PhotonNetwork.IsMasterClient)
        {
            SAW_Value_Check();
            Can_Be_Control = true;
            for (int i = 0; i < Condition_List.Length; i++)
            {
                if (Condition_List[i] != Can_Move_Condition[i])
                {
                    Can_Be_Control = false;
                    break;
                }
            }
        }
    }
    [PunRPC]
    void Ask_For_Value_Update(string _UI_Name)
    {
        switch (_UI_Name)
        {
            case "Hp":
                photonView.RPC("UI_Update", Find_Player_By_Name(Belong_Player_Name), "Hp", (float)This_Mob.Hp / (float)This_Mob.Max_Hp);
                break;
            case "Mp":
                photonView.RPC("UI_Update", Find_Player_By_Name(Belong_Player_Name), "Mp", (float)This_Mob.Mp / (float)This_Mob.Max_Mp);
                break;
            case "Strength":
                photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Strength", Strength_Value);
                break;
            case "Agility":
                photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Agility", Agility_Value);
                break;
            case "Wit":
                photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Wit", Wit_Value);
                break;
            case "Level":
                photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Level", Level_Value);
                break;
        }
    }

    void SAW_Value_Check()
    {
        if (This_Mob == null)
        {
            return;
        }

        if (Strength_Value != This_Mob.Strength)
        {
            Strength_Value = This_Mob.Strength;
            photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Strength", Strength_Value);
        }
        if (Agility_Value != This_Mob.Agility)
        {
            Agility_Value = This_Mob.Agility;
            photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Agility", Agility_Value);
        }
        if (Wit_Value != This_Mob.Wit)
        {
            Wit_Value = This_Mob.Wit;
            photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Wit", Wit_Value);
        }
        if (Level_Value != This_Mob.Level)
        {
            Level_Value = This_Mob.Level;
            photonView.RPC("Value_UI_Update", Find_Player_By_Name(Belong_Player_Name), "Level", Level_Value);
        }
    }

    public IEnumerator Set_Pic(string _Pic_url)
    {
        Texture2D Pic_Image;
        Pic_Image = new Texture2D(4, 4, TextureFormat.RGB24, false);
        WWW www = new WWW(_Pic_url);
        yield return www;
        www.LoadImageIntoTexture(Pic_Image);
        GameObject.Find("Player_Pic").GetComponent<Image>().sprite = Sprite.Create(Pic_Image, new Rect(0, 0, Pic_Image.width, Pic_Image.height), new Vector2(0.5f, 0.5f));
        photonView.RPC("Global_Set_My_Pic", RpcTarget.All, this.GetComponent<PhotonView>().ViewID, _Pic_url);
    }

    public IEnumerator Set_My_Pic(int _My_Figure_ID, string _Pic_url)
    {
        Texture2D Pic_Image;
        Pic_Image = new Texture2D(4, 4, TextureFormat.RGB24, false);
        WWW www = new WWW(_Pic_url);
        yield return www;
        www.LoadImageIntoTexture(Pic_Image);
        PhotonView.Find(_My_Figure_ID).GetComponent<SpriteRenderer>().sprite = Sprite.Create(Pic_Image, new Rect(0, 0, Pic_Image.width, Pic_Image.height), new Vector2(0.5f, 0.2f));
    }

    [PunRPC]
    void Global_Set_My_Pic(int _My_Figure_ID, string _Pic_url)
    {
        StartCoroutine(Set_My_Pic(_My_Figure_ID, _Pic_url));
    }

    public IEnumerator Move(int _Out_ID)
    {
        if (Can_Be_Control)
        {
            Debug.Log("I Can Move!!!");
            Condition_List[0] = true;
            This_Mob.Node_Now.Players.Remove(this.gameObject);
            for (int i = 0; i <= This_Mob.Node_Now.Out_Links[_Out_ID].Pos.Count; i++)
            {
                if (i == This_Mob.Node_Now.Out_Links[_Out_ID].Pos.Count)
                {
                    while (Vector3.Distance(transform.position, MM.GetComponent<Map_Manager>().Nodes[This_Mob.Node_Now.Outs[_Out_ID]].Pos) >= 0.05f)
                    {
                        transform.Translate((MM.GetComponent<Map_Manager>().Nodes[This_Mob.Node_Now.Outs[_Out_ID]].Pos - transform.position).normalized * 5f * Time.deltaTime, Space.World);
                        yield return new WaitForSeconds(0.001f);
                    }
                }
                else
                {
                    while (Vector3.Distance(transform.position, This_Mob.Node_Now.Out_Links[_Out_ID].Pos[i]) >= 0.05f)
                    {
                        transform.Translate((This_Mob.Node_Now.Out_Links[_Out_ID].Pos[i] - transform.position).normalized * 5f * Time.deltaTime, Space.World);
                        yield return new WaitForSeconds(0.001f);
                    }
                }
            }
            This_Mob.Node_Now = MM.GetComponent<Map_Manager>().Nodes[This_Mob.Node_Now.Outs[_Out_ID]];
            This_Mob.Node_Now.Players.Add(this.gameObject);
            Condition_List[0] = false;
            Round_Step_Consum();
        }
    }

    public string Pic_url;
    public void Build_This_Mob(string _Name, int _Hp, int _Mp, int _Strength, int _Agility, int _Wit, int _Level, string _Description, List<string> _Attributes, List<Item> _Items, List<Skill> _Skills)
    {
        GameObject.Find("Control_Handler").GetComponent<Control_Handler>().Master_Set_Pic(_Name);
        Mob _temp_Mob = new Mob(_Level) { ID = -1, Name = _Name, Hp = _Hp, Mp = _Mp, Strength = _Strength, Agility = _Agility, Wit = _Wit, Description = _Description };
        _temp_Mob.Attributes.AddRange(_Attributes);
        _temp_Mob.Items.AddRange(_Items);
        _temp_Mob.Skills.AddRange(_Skills);
        This_Mob = _temp_Mob;
        This_Mob.Update_Level();
        This_Mob.Node_Now = Node_Now;



        if (MobM == null)
        {
            MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        }
        Debug.Log(_temp_Mob.Description);
        MobM.Player_Mob_Kinds.Add(_temp_Mob);
        Debug.Log(MobM.Player_Mob_Kinds[0].Description);
        /*
        //============
        //This_Mob = new Mob(0) { ID = -1, Name = "None", Hp = 100, Mp = 100, Strength = 10, Agility = 10, Wit = 10, Description = "???" };
        This_Mob.Update_Values();
        Debug.Log("Figure: " + This_Mob.Content(1));
        This_Mob.Node_Now = Node_Now;
        This_Mob.Skills.Add(new Skill { Name = "Fire_Ball", Hp_Spend = 0, Mp_Spend = 10, Hp_Multiply = 0, Mp_Multiply = 0, Strength_Multiply = 0, Agility_Multiply = 0, Wit_Multiply = 0.5f , Description = "No." + 0 });
        This_Mob.Skills.Find(_Skill => _Skill.Name == "Fire_Ball").Skill_Effects.Add(new Skill_Effect { Apply_Value = "Hp", Value = -10 });

        This_Mob.Skills.Find(_Skill => _Skill.Name == "Fire_Ball").Narratives.Add("Suddenly, raging fire rise around B and B can barely defence himself");
        for (int i = 1; i <= 23; i += 1)
        {
            This_Mob.Skills.Add(new Skill { Name = "Fire_Ball" + "_" + i, Hp_Spend = 0, Mp_Spend = 10, Hp_Multiply = 0, Mp_Multiply = 0, Strength_Multiply = 0, Agility_Multiply = 0, Wit_Multiply = 0.5f, Description = "No." + i });
            This_Mob.Skills.Find(_Skill => _Skill.Name == "Fire_Ball" + "_" + i).Narratives.Add("Nothing happen. 0.0");
        }
        for (int i = 1; i <= 20; i += 2)
        {
            This_Mob.Items.Add(new Item { Type = "Food", Name = "Poop_" + i, Hp_Plus = 10, Mp_Plus = 10, Strength_Plus = 10, Agility_Plus = 10, Wit_Plus = 10, Hp_Multiply = 1, Mp_Multiply = 1, Strength_Multiply = 1, Agility_Multiply = 1, Wit_Multiply = 1, Description = "A poop", Drop_Rate = 1 });
        }
        for (int i = 0; i <= 20; i += 2)
        {
            This_Mob.Items.Add(new Item { Type = "Gear", Name = "Poop_" + i, Hp_Plus = 10, Mp_Plus = 10, Strength_Plus = 10, Agility_Plus = 10, Wit_Plus = 10, Hp_Multiply = 1, Mp_Multiply = 1, Strength_Multiply = 1, Agility_Multiply = 1, Wit_Multiply = 1, Description = "A poop", Drop_Rate = 1 });
        }
        //============
        */
        //This_Mob.Items.Add(GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>().Item_Kinds[0]);
    }

    Node Node_Now;
    public void Inis_Transport()
    {
        Node_Now = GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes[Random.Range(0, GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes.Count)];
        Node_Now.Players.Add(this.gameObject);
        transform.position = Node_Now.Pos;
        Condition_List[3] = true;
        photonView.RPC("Game_Camera_Inis_Pos", Find_Player_By_Name(Belong_Player_Name), transform.position.x, transform.position.y);
    }

    [PunRPC]
    public void Game_Camera_Inis_Pos(float _x, float _y)
    {
        Debug.LogError("Camera Move!!!");
        Camera.main.transform.position = new Vector3(_x, _y, Camera.main.transform.position.z);
        Debug.LogError(Camera.main.transform.position);
    }


    public void Pick_Up_Item(Item _Item)
    {
        This_Mob.Items.Add(_Item);
        This_Mob.Node_Now.Stuffs.Remove(_Item);
        Chronicle_Content.text += "    " + Belong_Player_Name + " get a" + _Item.Name + "\n";
        switch (_Item.Type)
        {
            case "Gear":
                This_Mob.Update_Values();
                Debug.Log("3: " + This_Mob.Content(1));
                break;
        }
    }

    public void Click_Item(string _Item_Name)
    {
        Debug.Log("Use Item!");
        int _temp = This_Mob.Items.FindIndex(_I => _I.Name == _Item_Name);
        if (_temp != -1)
        {
            switch (This_Mob.Items[_temp].Type)
            {
                case "Gear":
                    if (!This_Mob.Items[_temp].Wearing)
                    {
                        Wear_Gear(This_Mob.Items[_temp]);
                    }
                    else
                    {
                        Take_Off_Gear(This_Mob.Items[_temp]);
                    }
                    break;
                case "Food":
                    This_Mob.Consum_Food(This_Mob.Items[_temp]);
                    break;
            }
        }
        Debug.Log("Player_Info: " + This_Mob.Content(1));
    }

    public void Wear_Gear(Item _Item)
    {
        _Item.Wearing = true;
        This_Mob.Update_Values();
    }

    public void Take_Off_Gear(Item _Item)
    {
        _Item.Wearing = false;
        This_Mob.Update_Values();
    }

    public void Drop_Item(Item _Item)
    {
        _Item.Node_Now = This_Mob.Node_Now;
        This_Mob.Node_Now.Stuffs.Add(_Item);
        This_Mob.Update_Values();
        This_Mob.Items.Remove(_Item);
        Chronicle_Content.text += "    " + Belong_Player_Name + " drop a" + _Item.Name + "\n";
    }

    void Hightlight_Outs()
    {
        foreach (int _Out in This_Mob.Node_Now.Outs)
        {
            photonView.RPC("Hightlight", Find_Player_By_Name(Belong_Player_Name), GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes[_Out].Game_Object.GetComponent<PhotonView>().ViewID, Color.yellow.r, Color.yellow.g, Color.yellow.b, Color.yellow.a);
        }
    }

    void Dis_Hightlight_Outs()
    {
        foreach (int _Out in This_Mob.Node_Now.Outs)
        {
            photonView.RPC("Hightlight", Find_Player_By_Name(Belong_Player_Name), GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes[_Out].Game_Object.GetComponent<PhotonView>().ViewID, Color.white.r, Color.white.g, Color.white.b, Color.white.a);
        }
    }

    [PunRPC]
    void Hightlight(int _Node_ID, float _R, float _G, float _B, float _A)
    {
        Color _Color = new Color(_R, _G, _B, _A);
        PhotonView.Find(_Node_ID).GetComponent<SpriteRenderer>().color = _Color;
    }

    public void Round_Step_Consum()
    {
        Round_Step--;
        if (Round_Step <= 0)
        {
            End_Turn();
        }
    }

    public void Start_Turn()
    {
        Condition_List[1] = true;
        Condition_List[2] = false;
    }

    public void End_Turn()
    {
        Condition_List[1] = false;
        GM.GetComponent<Game_Manager>().Next_Player();
    }

    public void Cast_Skill(GameObject _Apply_Mob, Skill _Skill)
    {
        if (Can_Be_Control)
        {

            if (_Skill.Hp_Spend > This_Mob.Hp || _Skill.Mp_Spend > This_Mob.Mp)
            {
                Chronicle_Content.text += "    " + Belong_Player_Name + "is not able to use" + _Skill.Name + "!\n";
                BC.Master_BroadCast_Type(Belong_Player_Name + "is not able to use" + _Skill.Name);
                return;
            }

            Chronicle_Content.text += "    " + Belong_Player_Name + " used " + _Skill.Name + "\n";
            BC.Master_BroadCast_Type(Belong_Player_Name + " used " + _Skill.Name);
            if (_Apply_Mob.GetComponent<Mob_Behavior>() != null)
            {
                Debug.Log("Player_Agility: " + This_Mob.Agility);
                Debug.Log("Slime_Agility: " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Agility);
                if (_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Agility > This_Mob.Agility)
                {
                    int _temp = Random.Range(0, _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Skills.Count);
                    _temp = 0;
                    Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " find out and use " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Skills[_temp].Name + " first!\n";
                    BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " find out and use " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Skills[_temp].Name + " first");
                    _Apply_Mob.GetComponent<Mob_Behavior>().Cast_Skill(this.gameObject, _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Skills[_temp]);
                }
                Chronicle_Content.text += "    " + _Skill.Narratives[Random.Range(0, _Skill.Narratives.Count)].Replace("Z", _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name).Replace("Y", This_Mob.Name) + "\n";
                BC.Master_BroadCast_Type(_Skill.Narratives[Random.Range(0, _Skill.Narratives.Count)].Replace("Z", _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name).Replace("Y", This_Mob.Name));
            }
            else
            {
                /*
                if (_Apply_Mob.GetComponent<Figure>().This_Mob.Agility > This_Mob.Agility)
                {
                    //讓對方玩家先攻擊
                }
                */
                Chronicle_Content.text += "    " + _Skill.Narratives[Random.Range(0, _Skill.Narratives.Count)].Replace("Z", _Apply_Mob.GetComponent<Figure>().This_Mob.Name).Replace("Y", This_Mob.Name) + "\n";
                BC.Master_BroadCast_Type(_Skill.Narratives[Random.Range(0, _Skill.Narratives.Count)].Replace("Z", _Apply_Mob.GetComponent<Figure>().This_Mob.Name).Replace("Y", This_Mob.Name));
            }
            foreach (Skill_Effect _Skill_Effect in _Skill.Skill_Effects)
            {
                float _Value = _Skill_Effect.Value;

                foreach (string _Attribute in This_Mob.Attributes)
                {
                    foreach (string _A in _Skill.Attributes.FindAll(_a => _a == _Attribute))
                    {
                        _Value *= 1.5f;
                    }
                }
                switch (_Skill_Effect.Apply_Target)
                {
                    case "Self":
                        foreach (Skill_Effect_A_and_M _A_and_M in _Skill_Effect.Apply_Attribute_and_Multiply)
                        {
                            if (GetComponent<Figure>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                            {
                                _Value *= _A_and_M.Multiply;
                            }
                        }
                        switch (_Skill_Effect.Apply_Value)
                        {
                            case "Hp":
                                GetComponent<Figure>().Casted((int)_Value, 0);
                                if ((int)_Value < 0)
                                {
                                    Chronicle_Content.text += "    " + GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp" + "\n";
                                    BC.Master_BroadCast_Type(GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp");
                                }
                                else
                                {
                                    Chronicle_Content.text += "    " + GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp" + "\n";
                                    BC.Master_BroadCast_Type(GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp");
                                }
                                break;
                            case "Mp":
                                GetComponent<Figure>().Casted(0, (int)_Value);
                                if ((int)_Value < 0)
                                {
                                    Chronicle_Content.text += "    " + GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp" + "\n";
                                    BC.Master_BroadCast_Type(GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp");
                                }
                                else
                                {
                                    Chronicle_Content.text += "    " + GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp" + "\n";
                                    BC.Master_BroadCast_Type(GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp");
                                }
                                break;
                        }

                        if (_Apply_Mob.GetComponent<Figure>().This_Mob.Hp <= 0)
                        {
                            Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name + "\n";
                            BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name);
                            After_Dead(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name, _Apply_Mob, Belong_Player_Name);
                        }

                        break;
                    case "Target":
                        if (_Apply_Mob.GetComponent<Mob_Behavior>() != null)
                        {
                            Debug.Log("Attack_mob_ID:" + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Node_Now.ID);
                            foreach (string _Attribute in _Skill.Attributes)
                            {
                                foreach (Attribute_Rule _AR in MobM.Attribute_Rules.FindAll(_a => _a.Attribute_1 == _Attribute))
                                {
                                    if (_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Attributes.Exists(_a => _a == _AR.Attribute_2))
                                    {
                                        _Value *= _AR.Multiply;
                                    }
                                }
                            }
                            foreach (Skill_Effect_A_and_M _A_and_M in _Skill_Effect.Apply_Attribute_and_Multiply)
                            {
                                if (_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                                {
                                    _Value *= _A_and_M.Multiply;
                                }
                            }

                            switch (_Skill_Effect.Apply_Value)
                            {
                                case "Hp":
                                    _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Hp += (int)_Value;
                                    if ((int)_Value < 0)
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Hp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Hp");
                                    }
                                    else
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Hp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Hp");
                                    }
                                    break;
                                case "Mp":
                                    _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Mp += (int)_Value;
                                    if ((int)_Value < 0)
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Mp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Mp");
                                    }
                                    else
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Mp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Mp");
                                    }
                                    break;
                            }

                            if (_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Hp <= 0)
                            {
                                This_Mob.Get_Exp(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Exp);
                                Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + Belong_Player_Name + "\n";
                                BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + Belong_Player_Name);
                                _Apply_Mob.GetComponent<Mob_Behavior>().Drop_Items();
                                GM.GetComponent<Game_Manager>().DeletMob(_Apply_Mob);
                            }

                        }
                        else
                        {
                            foreach (string _Attribute in This_Mob.Attributes)
                            {
                                foreach (Attribute_Rule _AR in MobM.Attribute_Rules.FindAll(_a => _a.Attribute_1 == _Attribute))
                                {
                                    if (_Apply_Mob.GetComponent<Figure>().This_Mob.Attributes.Exists(_a => _a == _AR.Attribute_2))
                                    {
                                        _Value *= _AR.Multiply;
                                    }
                                }
                            }
                            foreach (Skill_Effect_A_and_M _A_and_M in _Skill_Effect.Apply_Attribute_and_Multiply)
                            {
                                if (_Apply_Mob.GetComponent<Figure>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                                {
                                    _Value *= _A_and_M.Multiply;
                                }
                            }

                            switch (_Skill_Effect.Apply_Value)
                            {
                                case "Hp":
                                    _Apply_Mob.GetComponent<Figure>().Casted((int)_Value, 0);
                                    if ((int)_Value < 0)
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp");
                                    }
                                    else
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp");
                                    }
                                    break;
                                case "Mp":
                                    _Apply_Mob.GetComponent<Figure>().Casted(0, (int)_Value);
                                    if ((int)_Value < 0)
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp");
                                    }
                                    else
                                    {
                                        Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp" + "\n";
                                        BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp");
                                    }
                                    break;
                            }
                            if (_Apply_Mob.GetComponent<Figure>().This_Mob.Hp <= 0)
                            {
                                Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name + "\n";
                                BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name);
                                After_Dead(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name, _Apply_Mob, Belong_Player_Name);
                            }
                        }
                        break;
                    case "All":
                        foreach (GameObject _Mob in This_Mob.Node_Now.Mobs)
                        {
                            if (_Mob.GetComponent<Mob_Behavior>() != null)
                            {
                                foreach (string _Attribute in _Skill.Attributes)
                                {
                                    foreach (Attribute_Rule _AR in MobM.Attribute_Rules.FindAll(_a => _a.Attribute_1 == _Attribute))
                                    {
                                        if (_Mob.GetComponent<Mob_Behavior>().This_Mob.Attributes.Exists(_a => _a == _AR.Attribute_2))
                                        {
                                            _Value *= _AR.Multiply;
                                        }
                                    }
                                }
                                foreach (Skill_Effect_A_and_M _A_and_M in _Skill_Effect.Apply_Attribute_and_Multiply)
                                {
                                    if (_Mob.GetComponent<Mob_Behavior>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                                    {
                                        _Value *= _A_and_M.Multiply;
                                    }
                                }

                                switch (_Skill_Effect.Apply_Value)
                                {
                                    case "Hp":
                                        _Mob.GetComponent<Mob_Behavior>().This_Mob.Hp += (int)_Value;
                                        if ((int)_Value < 0)
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Hp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Hp");
                                        }
                                        else
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Hp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Hp");
                                        }
                                        break;
                                    case "Mp":
                                        _Mob.GetComponent<Mob_Behavior>().This_Mob.Mp += (int)_Value;
                                        if ((int)_Value < 0)
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Mp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + -(int)_Value + " Mp");
                                        }
                                        else
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Mp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + (int)_Value + " Mp");
                                        }
                                        break;
                                }

                                if (_Mob.GetComponent<Mob_Behavior>().This_Mob.Hp <= 0)
                                {
                                    This_Mob.Get_Exp(_Mob.GetComponent<Mob_Behavior>().This_Mob.Exp);
                                    Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + Belong_Player_Name + "\n";
                                    BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + Belong_Player_Name);
                                    _Apply_Mob.GetComponent<Mob_Behavior>().Drop_Items();
                                    GM.GetComponent<Game_Manager>().DeletMob(_Mob);
                                }

                            }
                            else
                            {
                                foreach (string _Attribute in This_Mob.Attributes)
                                {
                                    foreach (Attribute_Rule _AR in MobM.Attribute_Rules.FindAll(_a => _a.Attribute_1 == _Attribute))
                                    {
                                        if (_Mob.GetComponent<Figure>().This_Mob.Attributes.Exists(_a => _a == _AR.Attribute_2))
                                        {
                                            _Value *= _AR.Multiply;
                                        }
                                    }
                                }
                                foreach (Skill_Effect_A_and_M _A_and_M in _Skill_Effect.Apply_Attribute_and_Multiply)
                                {
                                    if (_Mob.GetComponent<Figure>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                                    {
                                        _Value *= _A_and_M.Multiply;
                                    }
                                }

                                switch (_Skill_Effect.Apply_Value)
                                {
                                    case "Hp":
                                        _Mob.GetComponent<Figure>().Casted((int)_Value, 0);
                                        if ((int)_Value < 0)
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Hp");
                                        }
                                        else
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Hp");
                                        }
                                        break;
                                    case "Mp":
                                        _Mob.GetComponent<Figure>().Casted(0, (int)_Value);
                                        if ((int)_Value < 0)
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Figure>().Belong_Player_Name + " loses " + -(int)_Value + " Mp");
                                        }
                                        else
                                        {
                                            Chronicle_Content.text += "    " + _Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp" + "\n";
                                            BC.Master_BroadCast_Type(_Mob.GetComponent<Figure>().Belong_Player_Name + " gains " + (int)_Value + " Mp");
                                        }
                                        break;
                                }
                                if (_Apply_Mob.GetComponent<Figure>().This_Mob.Hp <= 0)
                                {
                                    Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name + "\n";
                                    BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + Belong_Player_Name);
                                    After_Dead(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name, _Apply_Mob, Belong_Player_Name);
                                }
                            }

                        }
                        break;
                }

            }
        }

    }

    public void After_Dead(string _Player_Name, GameObject _G, string _S)
    {
        _G.GetComponent<Figure>().Condition_List[3] = false;
        GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Player_Dead_Next_Player(_G);
        photonView.RPC("Put_On_Gray_Mask", Find_Player_By_Name(_Player_Name), _S);
    }

    [PunRPC]
    public void Put_On_Gray_Mask(string _S)
    {
        GameObject.Find("Dead_Gray_Mask").transform.GetChild(0).GetComponent<Text>().text += _S;
        GameObject.Find("Dead_Gray_Mask").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("Dead_Gray_Mask").GetComponent<CanvasGroup>().interactable = true;
        GameObject.Find("Dead_Gray_Mask").GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Casted(int _Hp, int _Mp)
    {
        This_Mob.Hp += _Hp;
        This_Mob.Mp += _Mp;


        Debug.Log("HP UI:" + (float)This_Mob.Hp / (float)This_Mob.Max_Hp);


        photonView.RPC("UI_Update", Find_Player_By_Name(Belong_Player_Name), "Hp", (float)This_Mob.Hp / (float)This_Mob.Max_Hp);
        photonView.RPC("UI_Update", Find_Player_By_Name(Belong_Player_Name), "Mp", (float)This_Mob.Mp / (float)This_Mob.Max_Mp);
    }

    //------------------------------------------------------------------------------UI
    [PunRPC]
    public void UI_Update(string _UI_Name, float _Fill_Amount)
    {
        if (!Value_Setted_Up)
        {
            photonView.RPC("Ask_For_Value_Update", RpcTarget.MasterClient, _UI_Name);
            return;
        }
        switch (_UI_Name)
        {
            case "Hp":
                Hp_UI.fillAmount = _Fill_Amount;
                break;
            case "Mp":
                Mp_UI.fillAmount = _Fill_Amount;
                break;
        }

    }

    [PunRPC]
    public void Value_UI_Update(string _UI_Name, int _Value)
    {
        if (!Value_Setted_Up)
        {
            photonView.RPC("Ask_For_Value_Update", RpcTarget.MasterClient, _UI_Name);
            return;
        }
        switch (_UI_Name)
        {
            case "Strength":
                Strength_UI.text = "Strength: " + _Value;
                break;
            case "Agility":
                Agility_UI.text = "Agility: " + _Value;
                break;
            case "Wit":
                Wit_UI.text = "Wit: " + _Value;
                break;
            case "Level":
                Level_UI.text = _Value + "";
                break;
        }

    }

    //------------------------------------------------------------------------------UI
    Photon.Realtime.Player Find_Player_By_Name(string _Name)
    {
        foreach (Photon.Realtime.Player _Player in PhotonNetwork.PlayerList)
        {
            if (_Player.NickName == _Name)
            {
                return _Player;
            }
        }
        return null;
    }
}
