using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mob_stuff;

public class FIO_Game_Setting : MonoBehaviour
{
    Mob_Manager MobM;

    void Start()
    {
        //File_Out();
        //File_In();
        MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
    }

    void Update()
    {
        
    }

    public List<List<string>> File_In(string _File_Path)
    {
        List<string> Lines = new List<string>(System.IO.File.ReadAllLines(_File_Path));
        List<List<string>> _temp = new List<List<string>>();
        foreach (string _Line in Lines)
        {
            _temp.Add(new List<string>(_Line.Split('|')));
        }
        return _temp;
    }

    public void File_Out(string _Line, string _File_Path)
    {
        using (System.IO.StreamWriter File = new System.IO.StreamWriter(_File_Path, true))
        {
            _Line.Replace("||", "|");
            File.WriteLine(_Line);
        }
    }

    public string List_To_Line(List<string> _Lines)
    {
        string _temp = "|";
        foreach (string _Line in _Lines)
        {
            _temp += _Line + "|";
        }
        return _temp;
    }
    /*
    public void Set_BK_URL()
    {
        GameObject.Find("Map_Manager").GetComponent<Map_Manager>().BK_url = GameObject.Find("BK_URL").GetComponent<InputField>().text;
        StartCoroutine(GameObject.Find("Map_Manager").GetComponent<Map_Manager>().Get_BK());
    }

    public void Add_Attribute_Rule()
    {
        MobM.Attribute_Rules.Add(new Attribute_Rule { Attribute_1 = GameObject.Find("Attribute_1").GetComponent<Dropdown>().captionText.text, Multiply = float.Parse(GameObject.Find("Set_Attribute").GetComponent<InputField>().text), Attribute_2 = GameObject.Find("Attribute_2").GetComponent<Dropdown>().captionText.text });
    }

    public void Add_Skill()
    {
        GameObject _temp = GameObject.Find("Add_Skill");
        Skill _temp_Skill = new Skill { Name = GameObject.Find("Skill_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp_Spend = int.Parse(GameObject.Find("Skill_Hp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Spend = int.Parse(GameObject.Find("Skill_Mp_Spend").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Hp_Multiply = float.Parse(GameObject.Find("Skill_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Multiply = float.Parse(GameObject.Find("Skill_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Multiply = float.Parse(GameObject.Find("Skill_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Multiply = float.Parse(GameObject.Find("Skill_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Multiply = float.Parse(GameObject.Find("Skill_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Skill_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text };
        _temp_Skill.Narratives.AddRange(temp_Narratives);
        _temp_Skill.Attributes.AddRange(temp_Attributes);
        _temp_Skill.Skill_Effects.AddRange(temp_Skill_Effects);
        MobM.Skill_Kinds.Add(_temp_Skill);
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

    public void Add_Item()
    {
        GameObject _temp = GameObject.Find("Add_Item");
        MobM.Item_Kinds.Add(new Item { Name = GameObject.Find("Item_Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Type = GameObject.Find("Item_Type").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp_Plus = int.Parse(GameObject.Find("Item_Hp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Plus = int.Parse(GameObject.Find("Item_Mp_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Plus = int.Parse(GameObject.Find("Item_Strength_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Plus = int.Parse(GameObject.Find("Item_Agility_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Plus = int.Parse(GameObject.Find("Item_Wit_Plus").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Hp_Multiply = float.Parse(GameObject.Find("Item_Hp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp_Multiply = float.Parse(GameObject.Find("Item_Mp_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength_Multiply = float.Parse(GameObject.Find("Item_Strength_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility_Multiply = float.Parse(GameObject.Find("Item_Agility_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit_Multiply = float.Parse(GameObject.Find("Item_Wit_Multiply").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Drop_Rate = float.Parse(GameObject.Find("Item_Drop_Rate").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Item_Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text });
    }

    public void Add_Mob()
    {
        GameObject _temp = GameObject.Find("Add_Mob");
        Mob _temp_Mob = new Mob { Name = GameObject.Find("Name").transform.GetChild(0).gameObject.GetComponent<InputField>().text, Hp = int.Parse(GameObject.Find("Hp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Mp = int.Parse(GameObject.Find("Mp").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Strength = int.Parse(GameObject.Find("Strength").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Agility = int.Parse(GameObject.Find("Agility").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Wit = int.Parse(GameObject.Find("Wit").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Level = int.Parse(GameObject.Find("Level").transform.GetChild(0).gameObject.GetComponent<InputField>().text), Description = GameObject.Find("Description").transform.GetChild(0).gameObject.GetComponent<InputField>().text };
        _temp_Mob.Attributes.AddRange(temp_Attributes);
        _temp_Mob.Skills.AddRange(temp_Skills);
        _temp_Mob.Items.AddRange(temp_Items);
        _temp_Mob.Habitats.AddRange(temp_Habitats);
        MobM.Mob_Kinds.Add(_temp_Mob);
    }
    */
}
