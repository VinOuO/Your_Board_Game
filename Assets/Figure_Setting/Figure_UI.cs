using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Mob_stuff;
using Com.Vincent.Board_Game;

public class Figure_UI : MonoBehaviour
{
    GameObject GS;

    Mob_Manager MobM;
    List<string> temp_Attributes = new List<string>();
    List<string> temp_Skills = new List<string>();
    List<string> temp_Items = new List<string>();
    PhotonView photonView;
    void Start()
    {
        photonView = PhotonView.Get(this);
        GS = GameObject.Find("Game_Setting");
        /*
        Debug.Log(GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>());
        Debug.Log(GS.GetComponent<Game_Setting>());
        Debug.Log(GS.GetComponent<Game_Setting>().temp_Attributes);
        */
        if (PhotonNetwork.IsMasterClient)
        {
            Master_Set_Form();
        }
    }

    private void Update()
    {
        if (GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().options.Count == 0 || GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().options.Count == 0 || GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().options.Count == 0)
        {
            photonView.RPC("Ask_For_Form", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void Ask_For_Form()
    {
        Master_Set_Form();
    }

    [PunRPC]
    void Ask_Set_Form()
    {
        photonView.RPC("Set_Form", RpcTarget.All, MobM.Attribute_Kinds.Count + MobM.Skill_Kinds.Count + MobM.Item_Kinds.Count);
    }

    void Master_Set_Form()
    {
        int _Data_Num = 0;
        MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        _Data_Num = MobM.Attribute_Kinds.Count + MobM.Skill_Kinds.Count + MobM.Item_Kinds.Count;
        foreach (string _Attribute in MobM.Attribute_Kinds)
        {
            photonView.RPC("Send_Form_Info", RpcTarget.All, "Attribute", _Attribute, _Data_Num);
        }
        foreach (Skill _Skill in MobM.Skill_Kinds)
        {
            photonView.RPC("Send_Form_Info", RpcTarget.All, "Skill", _Skill.Name, _Data_Num);
        }
        foreach (Item _Item in MobM.Item_Kinds)
        {
            photonView.RPC("Send_Form_Info", RpcTarget.All, "Item", _Item.Name, _Data_Num);
        }
    }

    List<string> _temp_Attribute_Kinds = new List<string>();
    List<string> _temp_Skill_Kinds = new List<string>();
    List<string> _temp_Item_Kinds = new List<string>();
    [PunRPC]
    void Send_Form_Info(string _Type, string _Info, int _Data_Num)
    {
        switch (_Type)
        {
            case "Attribute":
                if (!_temp_Attribute_Kinds.Exists(_t => _t == _Info))
                {
                    _temp_Attribute_Kinds.Add(_Info);
                }
                break;
            case "Skill":
                if (!_temp_Skill_Kinds.Exists(_t => _t == _Info))
                {
                    _temp_Skill_Kinds.Add(_Info);
                }
                break;
            case "Item":
                if (!_temp_Item_Kinds.Exists(_t => _t == _Info))
                {
                    _temp_Item_Kinds.Add(_Info);
                }
                break;
        }
        if (_Data_Num <= _temp_Attribute_Kinds.Count + _temp_Skill_Kinds.Count + _temp_Item_Kinds.Count)
        {
            photonView.RPC("Ask_Set_Form", RpcTarget.MasterClient);
        }
    }


    bool Setted = false;
    [PunRPC]
    void Set_Form(int _Data_Num)
    {
        if (_Data_Num > _temp_Attribute_Kinds.Count + _temp_Skill_Kinds.Count + _temp_Item_Kinds.Count)
        {
            return;
        }
        if (!Setted)
        {
            GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().AddOptions(_temp_Attribute_Kinds);
            GameObject.Find("Figure_Skill_Drop_down").GetComponent<Dropdown>().AddOptions(_temp_Skill_Kinds);
            GameObject.Find("Figure_Item_Drop_down").GetComponent<Dropdown>().AddOptions(_temp_Item_Kinds);
            Setted = true;
        }

    }

    public void Set_Figure()
    {
        GameObject _temp = GameObject.Find("Figure_Setting");
        Mob _temp_Mob = new Mob { Name = GameObject.Find("Figure_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp = int.Parse(GameObject.Find("Figure_Hp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp = int.Parse(GameObject.Find("Figure_Mp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength = int.Parse(GameObject.Find("Figure_Strength").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility = int.Parse(GameObject.Find("Figure_Agility").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit = int.Parse(GameObject.Find("Figure_Wit").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Level = int.Parse(GameObject.Find("Figure_Level").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Figure_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text };
        _temp_Mob.Attributes.AddRange(temp_Attributes);
        //_temp_Mob.Skills.AddRange(temp_Skills);
        GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Skills.AddRange(temp_Skills);
        //_temp_Mob.Items.AddRange(temp_Items);
        GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().temp_Items.AddRange(temp_Items);
        GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure = _temp_Mob;
        GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>().Figure_Url = GameObject.Find("Figure_Url").transform.GetChild(0).gameObject.GetComponent<InputField>().text;


        _temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        temp_Attributes.Clear();
        _temp.transform.GetChild(2).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(3).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(4).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(5).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(6).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(7).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(10).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(11).GetChild(0).gameObject.GetComponent<InputField>().text = "";

        GameObject.Find("Network_Manager").GetComponent<Network_Manager>().Local_Set_Up();
    }

    public void Add_temp_Attribute()
    {
        if (!temp_Attributes.Exists(_A => _A == GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().captionText.text))
        {
            temp_Attributes.Add(GameObject.Find("Figure_Attribute_Drop_down").GetComponent<Dropdown>().captionText.text);
        }
    }

    public void Add_temp_Skill()
    {
        if (!temp_Skills.Exists(_S => _S == GameObject.Find("Figure_Skill_Drop_down").GetComponent<Dropdown>().captionText.text))
        {
            temp_Skills.Add(GameObject.Find("Figure_Skill_Drop_down").GetComponent<Dropdown>().captionText.text);
        }
    }

    public void Add_temp_Item()
    {
        temp_Items.Add(GameObject.Find("Figure_Item_Drop_down").GetComponent<Dropdown>().captionText.text);
    }

}
