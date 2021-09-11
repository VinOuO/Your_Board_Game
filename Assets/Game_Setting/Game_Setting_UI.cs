using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Map_stuff;
using Mob_stuff;
using System;
using System.IO;

public class Game_Setting_UI : MonoBehaviour
{
    public bool Want_To_Save = false;
    string File_Path = @"C:\Users\Vincent\Desktop\Save_1\";
    GameObject GS;
    Mob_Manager MobM;
    public FIO_Game_Setting FIO_GS;
    List<string> temp_Narratives = new List<string>();
    List<Skill_Effect> temp_Skill_Effects = new List<Skill_Effect>();
    List<Skill_Effect_A_and_M> temp_Skill_Effect_A_and_Ms = new List<Skill_Effect_A_and_M>();
    List<string> temp_Attributes = new List<string>();
    List<Skill> temp_Skills = new List<Skill>();
    List<Item> temp_Items = new List<Item>();
    List<string> temp_Habitats = new List<string>();
    void Start()
    {
        File_Path = "";
        GS = GameObject.Find("Game_Setting");
        MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();

        if (Want_To_Save)
        {
            Creat_Setting_File();
        }
        else
        {
            StartCoroutine(Use_Setting_File_Check());
        }
    }

    void Update()
    {
    }

    public void Creat_Setting_File()
    {
        int _File_Num = Saved_Files().Count + 1;
        Debug.Log(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num);
        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num);
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Attribute_Rules.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Attributes.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Habitats.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Items.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Mobs.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Nodes.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Round_Titles.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Skills.txt");
        File.Create(Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\" + "Urls.txt");
        File_Path = Directory.GetCurrentDirectory() + @"\Save_" + _File_Num + @"\";
    }

    public void Select_Setting_File()
    {
        File_Path = GameObject.Find("Saved_File").GetComponent<Dropdown>().captionText.text + @"\";
    }

    IEnumerator Use_Setting_File_Check()
    {
        GameObject.Find("Saved_File").GetComponent<Dropdown>().AddOptions(Saved_Files());
        while (File_Path == "")
        {
            yield return new WaitForSeconds(0.1f);
        }
        Import_Setting();
    }

    List<string> Saved_Files()
    {
        List<string> _temp = new List<string>();

        string[] files = Directory.GetDirectories(Directory.GetCurrentDirectory(), "Save_*");
        Debug.Log(Directory.GetCurrentDirectory());
        for(int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(Directory.GetCurrentDirectory() + @"\", "");
            Debug.Log(files[i]);
            _temp.Add(files[i]);
        }

        return _temp;
    }

    public void Import_Setting()
    {
        GameObject.Find("Map_Manager").GetComponent<Map_Manager>().BK_url = FIO_GS.File_In(File_Path + "Urls.txt")[0][0];

        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Round_Titles.txt"))
        {
            GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Round_Titles.Add(new Round_Title { Num = int.Parse(_Data[0]), Title = _Data[1] });
        }

        List<List<string>> __Data = FIO_GS.File_In(File_Path + "Nodes.txt");
        for (int _i = 0; _i < FIO_GS.File_In(File_Path + "Nodes.txt").Count; _i++)
        {
            Node _temp = new Node { ID = int.Parse(__Data[_i][0]), Pos = new Vector3(float.Parse(__Data[_i][1]), float.Parse(__Data[_i][2]), float.Parse(__Data[_i][3])), Type = __Data[_i][4] };
            int i = 5;
            while (__Data[_i][i] != "Sttop")
            {
                if(_temp.Ins.Exists(_I => _I == int.Parse(__Data[_i][i])))
                {
                    Debug.LogError("Line_In: " + _i);
                }
                _temp.Ins.Add(int.Parse(__Data[_i][i]));
                i++;
            }
            i++;
            while (__Data[_i][i] != "Sttop")
            {
                if (_temp.Outs.Exists(_I => _I == int.Parse(__Data[_i][i])))
                {
                    Debug.LogError("Line_Out: " + _i);
                }
                _temp.Outs.Add(int.Parse(__Data[_i][i]));
                i++;
            }
            GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes.Add(_temp);
        }


        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Attributes.txt"))
        {
            MobM.Attribute_Kinds.Add(_Data[0]);
        }
        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Habitats.txt"))
        {
            MobM.Habitat_Kinds.Add(_Data[0]);
        }
        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Attribute_Rules.txt"))
        {
            MobM.Attribute_Rules.Add(new Attribute_Rule {Attribute_1 = _Data[0], Multiply = float.Parse(_Data[1]), Attribute_2 = _Data[2] });
        }

        Skill _temp_Hit = new Skill { Name = "Hit", Hp_Spend = 0, Mp_Spend = 0, Hp_Multiply = 0, Mp_Multiply = 0, Strength_Multiply = 0, Agility_Multiply = 0, Wit_Multiply = 0, Description = "Normal Attack" };
        _temp_Hit.Skill_Effects.Add(new Skill_Effect { Apply_Value = "Hp", Value = -5 });
        _temp_Hit.Narratives.Add("A hits B");
        MobM.Skill_Kinds.Add(_temp_Hit);
        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Skills.txt"))
        {
            string _Flag = "N";
            Skill _temp_Skill = new Skill { Name = _Data[0], Hp_Spend = int.Parse(_Data[1]), Mp_Spend = int.Parse(_Data[2]), Hp_Multiply = float.Parse(_Data[3]), Mp_Multiply = float.Parse(_Data[4]), Strength_Multiply = float.Parse(_Data[5]), Agility_Multiply = float.Parse(_Data[6]), Wit_Multiply = float.Parse(_Data[7]), Description = _Data[8] };
            for (int _Index = 9; _Index < _Data.Count; _Index++)
            {
                if(_Data[_Index] == "Sttop")
                {
                    if(_Flag == "N")
                    {
                        
                        _Flag = "A";
                    }
                    else if (_Flag == "A")
                    {
                        _Flag = "S";
                    }
                    continue;
                }
                if (_Flag == "N")
                {
                    _temp_Skill.Narratives.Add(_Data[_Index]);
                }
                else if (_Flag == "A")
                {
                    _temp_Skill.Attributes.Add(_Data[_Index]);
                }
                else if (_Flag == "S")
                {
                    Skill_Effect _temp_SE;
                    _temp_SE = new Skill_Effect { Apply_Target = _Data[_Index], Apply_Value = _Data[_Index + 1], Value = int.Parse(_Data[_Index + 2]) };
                    _Index += 3;
                    while (_Data[_Index] != "Sttop1")
                    {
                        _temp_SE.Apply_Attribute_and_Multiply.Add(new Skill_Effect_A_and_M { Apply_Attribute = _Data[_Index], Multiply = float.Parse(_Data[_Index + 1]) });
                        _Index += 2;
                    }
                    _temp_Skill.Skill_Effects.Add(_temp_SE);
                }
            }

            MobM.Skill_Kinds.Add(_temp_Skill);
        }

        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Items.txt"))
        {
            MobM.Item_Kinds.Add( new Item { Name = _Data[0], Type = _Data[1], Hp_Plus = int.Parse(_Data[2]), Mp_Plus = int.Parse(_Data[3]), Strength_Plus = int.Parse(_Data[4]), Agility_Plus = int.Parse(_Data[5]), Wit_Plus = int.Parse(_Data[6]), Hp_Multiply = float.Parse(_Data[7]), Mp_Multiply = float.Parse(_Data[8]), Strength_Multiply = float.Parse(_Data[9]), Agility_Multiply = float.Parse(_Data[10]), Wit_Multiply = float.Parse(_Data[11]), Drop_Rate = float.Parse(_Data[12]), Description = _Data[13] } );
        }

        foreach (List<string> _Data in FIO_GS.File_In(File_Path + "Mobs.txt"))
        {
            Mob _temp_Mob = new Mob { Name = _Data[0], Hp = int.Parse(_Data[1]), Mp = int.Parse(_Data[2]), Strength = int.Parse(_Data[3]), Agility = int.Parse(_Data[4]), Wit = int.Parse(_Data[5]), Level = int.Parse(_Data[6]), Begin_Round = int.Parse(_Data[7]), End_Round = int.Parse(_Data[8]), Description = _Data[9] };
            int i = 10;
            while (_Data[i]!= "Sttop")
            {
                _temp_Mob.Attributes.Add(_Data[i]);
                i++;
            }
            i++;
            while (_Data[i]!= "Sttop")
            {
                _temp_Mob.Skills.Add(MobM.Skill_Kinds.Find(_S => _S.Name == _Data[i]));
                i++;
            }
            i++;
            while (_Data[i] != "Sttop")
            {
                _temp_Mob.Attributes.Add(_Data[i]);
                _temp_Mob.Items.Add(MobM.Item_Kinds.Find(_I => _I.Name == _Data[i]));
                i++;
            }
            i++;
            while (_Data[i] != "Sttop")
            {
                _temp_Mob.Habitats.Add(_Data[i]);
                i++;
            }

            MobM.Mob_Kinds.Add(_temp_Mob);

        }

    }

    public void Set_Stage_Text()
    {
        Text _temp = GameObject.Find("Stage_Text").GetComponent<Text>();
        if (_temp.text == "Adding Habitats")
        {
            _temp.text = "Node Setting";

            GameObject.Find("Setting_File").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Setting_File").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Setting_File").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Add_Habitat").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Add_Habitat").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Add_Habitat").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Node_Habitat_Drop_down").GetComponent<Dropdown>().AddOptions(MobM.Habitat_Kinds);
            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (_temp.text == "Node Setting")
        {
            _temp.text = "Link Setting";

            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Node_Habitat").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else if (_temp.text == "Link Setting")
        {
            _temp.text = "Adding Attributes";

            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameObject.Find("Setting_BK").GetComponent<CanvasGroup>().alpha = 1;
        }
        else if (_temp.text == "Adding Attributes")
        {
            if (Want_To_Save)
            {
                Save_Node();
            }
            _temp.text = "Setting Attributes";

            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Add_Attribute").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameObject.Find("Attribute_1").GetComponent<Dropdown>().AddOptions(MobM.Attribute_Kinds);
            GameObject.Find("Attribute_2").GetComponent<Dropdown>().AddOptions(MobM.Attribute_Kinds);
        }
        else if (_temp.text == "Setting Attributes")
        {
            _temp.text = "Adding Skills";

            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Set_Attribute").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameObject.Find("Add_Skill_Attribute_Drop_down").GetComponent<Dropdown>().AddOptions(MobM.Attribute_Kinds);
            GameObject.Find("Skill_Effect_Drop_down").GetComponent<Dropdown>().AddOptions(MobM.Attribute_Kinds);
        }
        else if (_temp.text == "Adding Skills")
        {
            _temp.text = "Adding Items";

            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Add_Skill").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (_temp.text == "Adding Items")
        {
            _temp.text = "Adding Mobs";

            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Add_Item").GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().blocksRaycasts = true;

            GameObject.Find("Habitat_Drop_down").GetComponent<Dropdown>().AddOptions(MobM.Habitat_Kinds);
            GameObject.Find("Add_Mob_Attribute_Drop_down").GetComponent<Dropdown>().AddOptions(MobM.Attribute_Kinds);

            List<string> _temp_Skill_Kinds = new List<string>();
            foreach(Skill _Skill_Kind in MobM.Skill_Kinds)
            {
                _temp_Skill_Kinds.Add(_Skill_Kind.Name);
            }
            GameObject.Find("Skill_Drop_down").GetComponent<Dropdown>().AddOptions(_temp_Skill_Kinds);

            List<string> _temp_Item_Kinds = new List<string>();
            foreach (Item _Item_Kind in MobM.Item_Kinds)
            {
                _temp_Item_Kinds.Add(_Item_Kind.Name);
            }
            GameObject.Find("Item_Drop_down").GetComponent<Dropdown>().AddOptions(_temp_Item_Kinds);
        }
        else if (_temp.text == "Adding Mobs")
        {
            _temp.text = "Adding Titles";

            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("Add_Mob").GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameObject.Find("Add_Titles").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("Add_Titles").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("Add_Titles").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (_temp.text == "Adding Titles")
        {
            GS.GetComponent<Game_Setting>().Stage = "End";
        }
        

    }

    public void Use_Saved_File()
    {
        GS.GetComponent<Game_Setting>().Stage = "End";
    }

    public void Set_BK_URL()
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("BK_URL").GetComponent<InputField>().text, File_Path + "Urls.txt");
        }
        GameObject.Find("Map_Manager").GetComponent<Map_Manager>().BK_url = GameObject.Find("BK_URL").GetComponent<InputField>().text;
        StartCoroutine(GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Get_BK());
    }

    public void Save_Node()
    {
        string _temp = "";
        foreach (Node _N in GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Nodes)
        {
            string _temp1 = "";
            foreach(int _In in _N.Ins)
            {
                _temp1 += _In + "|";
            }
            _temp1 += "Sttop";
            string _temp2 = "";
            foreach (int _Out in _N.Outs)
            {
                _temp2 += _Out + "|";
            }
            _temp2 += "Sttop";
            _temp = _N.ID + "|" + _N.Pos.x + "|" + _N.Pos.y + "|" + _N.Pos.z + "|" + _N.Type + "|" + _temp1 + "|" + _temp2;
            FIO_GS.File_Out(_temp, File_Path + "Nodes.txt");
        }
        
    }

    public void Add_Habitat()
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("Add_Habitat").GetComponent<InputField>().text, File_Path + "Habitats.txt");
        }
        MobM.Habitat_Kinds.Add(GameObject.Find("Add_Habitat").GetComponent<InputField>().text);
        GameObject.Find("Add_Habitat").GetComponent<InputField>().text = "";
    }

    public void Add_Attribute()
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("Add_Attribute").GetComponent<InputField>().text, File_Path + "Attributes.txt");
        }
        MobM.Attribute_Kinds.Add(GameObject.Find("Add_Attribute").GetComponent<InputField>().text);
        GameObject.Find("Add_Attribute").GetComponent<InputField>().text = "";
    }

    public void Add_Attribute_Rule()//
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("Attribute_1").GetComponent<Dropdown>().captionText.text + "|" + GameObject.Find("Set_Attribute").GetComponent<InputField>().text + "|" + GameObject.Find("Attribute_2").GetComponent<Dropdown>().captionText.text, File_Path + "Attribute_Rules.txt");
        }
        MobM.Attribute_Rules.Add(new Attribute_Rule { Attribute_1 = GameObject.Find("Attribute_1").GetComponent<Dropdown>().captionText.text, Multiply = float.Parse(GameObject.Find("Set_Attribute").GetComponent<InputField>().text), Attribute_2 = GameObject.Find("Attribute_2").GetComponent<Dropdown>().captionText.text });
        GameObject.Find("Set_Attribute").GetComponent<InputField>().text = "";
    }

    public void Add_Skill()//
    {
        if (Want_To_Save)
        {
            List<string> _temp_Lines = new List<string>();
            foreach (Skill_Effect _S in temp_Skill_Effects)
            {
                string _temp_S = "";
                _temp_S = _S.Apply_Target+"|" + _S.Apply_Value + "|" + _S.Value;
                foreach (Skill_Effect_A_and_M _AM in _S.Apply_Attribute_and_Multiply)
                {
                    _temp_S += "|" + _AM.Apply_Attribute + "|" + _AM.Multiply;
                }
                _temp_S += "|Sttop1";
                _temp_Lines.Add(_temp_S);
            }
            FIO_GS.File_Out(GameObject.Find("Skill_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Hp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Mp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Skill_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text + FIO_GS.List_To_Line(temp_Narratives) +"Sttop"+ FIO_GS.List_To_Line(temp_Attributes) + "Sttop" + FIO_GS.List_To_Line(_temp_Lines) + "Sttop" , File_Path + "Skills.txt");
        }
        GameObject _temp = GameObject.Find("Add_Skill");
        Skill _temp_Skill = new Skill { Name = GameObject.Find("Skill_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp_Spend = int.Parse(GameObject.Find("Skill_Hp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Spend = int.Parse(GameObject.Find("Skill_Mp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Hp_Multiply = float.Parse(GameObject.Find("Skill_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Multiply = float.Parse(GameObject.Find("Skill_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Multiply = float.Parse(GameObject.Find("Skill_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Multiply = float.Parse(GameObject.Find("Skill_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Multiply = float.Parse(GameObject.Find("Skill_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Skill_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text };
        _temp_Skill.Narratives.AddRange(temp_Narratives);
        _temp_Skill.Attributes.AddRange(temp_Attributes);
        _temp_Skill.Skill_Effects.AddRange(temp_Skill_Effects);
        MobM.Skill_Kinds.Add(_temp_Skill);

        _temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(1).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(2).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(3).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(4).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(5).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(6).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(7).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        temp_Narratives.Clear();
        temp_Attributes.Clear();
        temp_Skill_Effects.Clear();
        _temp.transform.GetChild(10).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(10).GetChild(1).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(10).GetChild(3).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(10).GetChild(4).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(11).GetChild(0).gameObject.GetComponent<InputField>().text = "";
    }

    public void Add_temp_Narrative()
    {
        temp_Narratives.Add(GameObject.Find("Narrative").transform.GetChild(0).gameObject.GetComponent<InputField>().text);
    }

    public void Add_temp_Skill_Effects()
    {
        temp_Skill_Effects.Add(new Skill_Effect { Apply_Target = GameObject.Find("Apply_Target").GetComponent<InputField>().text, Apply_Value = GameObject.Find("Apply_Value").GetComponent<InputField>().text, Apply_Attribute_and_Multiply = temp_Skill_Effect_A_and_Ms, Value = int.Parse(GameObject.Find("Value").GetComponent<InputField>().text) });
    }

    public void Add_temp_Skill_Effects_A_and_M()
    {
        temp_Skill_Effect_A_and_Ms.Add(new Skill_Effect_A_and_M { Apply_Attribute = GameObject.Find("Skill_Effect_Drop_down").GetComponent<Dropdown>().captionText.text, Multiply = float.Parse(GameObject.Find("Multiply").GetComponent<InputField>().text) });
    }

    public void Add_Item()//
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("Item_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Type").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Hp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Mp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Strength_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Agility_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Wit_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Drop_Rate").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Item_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text, File_Path + "Items.txt");
        }
        GameObject _temp = GameObject.Find("Add_Item");
        MobM.Item_Kinds.Add(new Item { Name = GameObject.Find("Item_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Type = GameObject.Find("Item_Type").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp_Plus = int.Parse(GameObject.Find("Item_Hp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Plus = int.Parse(GameObject.Find("Item_Mp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Plus = int.Parse(GameObject.Find("Item_Strength_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Plus = int.Parse(GameObject.Find("Item_Agility_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Plus= int.Parse(GameObject.Find("Item_Wit_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Hp_Multiply = float.Parse(GameObject.Find("Item_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Multiply = float.Parse(GameObject.Find("Item_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Multiply = float.Parse(GameObject.Find("Item_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Multiply = float.Parse(GameObject.Find("Item_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Multiply = float.Parse(GameObject.Find("Item_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Drop_Rate = float.Parse(GameObject.Find("Item_Drop_Rate").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Item_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text });

        _temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(1).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(2).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(3).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(4).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(5).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(6).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(7).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(8).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(9).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(10).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(11).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(12).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(13).GetChild(0).gameObject.GetComponent<InputField>().text = "";
    }


    public void Add_Title()//
    {
        if (Want_To_Save)
        {
            FIO_GS.File_Out(GameObject.Find("Round").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Round_Title").transform.GetChild(0).gameObject.GetComponent<InputField>().text, File_Path + "Round_Titles.txt");
        }
        GameObject _temp = GameObject.Find("Add_Titles");
        GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Round_Titles.Add(new Round_Title {Num = int.Parse(GameObject.Find("Round").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Title = GameObject.Find("Round_Title").transform.GetChild(0).gameObject.GetComponent<InputField>().text });
        _temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(1).GetChild(0).gameObject.GetComponent<InputField>().text = "";
    }
    public void Add_Mob()//
    {
        if (Want_To_Save)
        {
            string _temp_S = "";
            foreach (Item _I in temp_Items)
            {
                _temp_S += "|" + _I.Name;
            }
            string _temp_S2 = "";
            foreach (Skill _S in temp_Skills)
            {
                _temp_S2 += "|" + _S.Name;
            }
            FIO_GS.File_Out(GameObject.Find("Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Hp").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Mp").transform.GetChild(0).gameObject.GetComponent<InputField>().text +"|" + GameObject.Find("Strength").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Agility").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Wit").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" + GameObject.Find("Level").transform.GetChild(0).gameObject.GetComponent<InputField>().text + "|" +int.Parse(GameObject.Find("Begin").transform.GetChild(0).gameObject.GetComponent<InputField>().text)+"|"+ int.Parse(GameObject.Find("End").transform.GetChild(0).gameObject.GetComponent<InputField>().text) + "|" + GameObject.Find("Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text + FIO_GS.List_To_Line(temp_Attributes) + "Sttop" + _temp_S2 + "|" + "Sttop" +_temp_S + "|" + "Sttop" + FIO_GS.List_To_Line(temp_Habitats) + "Sttop", File_Path + "Mobs.txt");
        }
        GameObject _temp = GameObject.Find("Add_Mob");
        Mob _temp_Mob = new Mob { Name = GameObject.Find("Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp = int.Parse(GameObject.Find("Hp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp = int.Parse(GameObject.Find("Mp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength = int.Parse(GameObject.Find("Strength").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility = int.Parse(GameObject.Find("Agility").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit = int.Parse(GameObject.Find("Wit").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Level = int.Parse(GameObject.Find("Level").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Begin_Round = int.Parse(GameObject.Find("Begin").transform.GetChild(0).gameObject.GetComponent<InputField>().text), End_Round = int.Parse(GameObject.Find("End").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text };
        _temp_Mob.Attributes.AddRange(temp_Attributes);
        _temp_Mob.Skills.AddRange(temp_Skills);
        _temp_Mob.Items.AddRange(temp_Items);
        _temp_Mob.Habitats.AddRange(temp_Habitats);
        MobM.Mob_Kinds.Add(_temp_Mob);

        _temp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        temp_Attributes.Clear();
        _temp.transform.GetChild(2).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(3).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(4).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(5).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(6).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(7).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(11).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(12).GetChild(0).gameObject.GetComponent<InputField>().text = "";
        _temp.transform.GetChild(13).GetChild(0).gameObject.GetComponent<InputField>().text = "";
    }

    public void Add_temp_Attribute(string _Drop_down)
    {
        if (!temp_Attributes.Exists(_A => _A == GameObject.Find(_Drop_down).GetComponent<Dropdown>().captionText.text))
        {
            temp_Attributes.Add(GameObject.Find(_Drop_down).GetComponent<Dropdown>().captionText.text);
        }
    }

    public void Add_temp_Skill(string _Drop_down)
    {
        if (!temp_Skills.Exists(_S => _S.Name == GameObject.Find(_Drop_down).GetComponent<Dropdown>().captionText.text))
        {
            temp_Skills.Add(MobM.Skill_Kinds.Find(_S => _S.Name == GameObject.Find(_Drop_down).GetComponent<Dropdown>().captionText.text));
        }
    }

    public void Add_temp_Item(string _Drop_down)
    {
        temp_Items.Add(MobM.Item_Kinds.Find(_S => _S.Name == GameObject.Find(_Drop_down).GetComponent<Dropdown>().captionText.text));
    }

    public void Add_temp_Habitat()
    {
        if (!temp_Habitats.Exists(_H => _H == GameObject.Find("Habitat_Drop_down").GetComponent<Dropdown>().captionText.text))
        {
            temp_Habitats.Add(GameObject.Find("Habitat_Drop_down").GetComponent<Dropdown>().captionText.text);
        }
    }
}
