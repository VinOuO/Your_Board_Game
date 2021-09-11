using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map_stuff;

namespace Mob_stuff
{
    public class Mob
    {
        public bool Inis = false;
        public int ID { get; set; }
        public string Name { get; set; }
        public Node Node_Now { get; set; }
        public float Exp { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Wit { get; set; }
        public int Max_Hp;
        public int Max_Mp;
        public int Original_Hp;
        public int Original_Mp;
        public int Original_Strength;
        public int Original_Agility;
        public int Original_Wit;
        public string Description { get; set; }
        public List<string> Attributes = new List<string>();
        public List<Skill> Skills = new List<Skill>();
        public List<Item> Items = new List<Item>();
        public List<Item> Gears = new List<Item>();
        public List<string> Habitats = new List<string>();
        public int Begin_Round { get; set; }
        public int End_Round { get; set; }
        public float Spawn_Odd { get; set; }
        public int Amount { get; set; }

        public Mob()
        {
            //Habitats.Add("Normal");
            Level = 0;
            Begin_Round = 0;
            End_Round = -1;
            Skills.Add(new Skill { Name = "Hit", Hp_Spend = 0, Mp_Spend = 0, Hp_Multiply = 0, Mp_Multiply = 0, Strength_Multiply = 0, Agility_Multiply = 0, Wit_Multiply = 0, Description = "Normal Attack" });
            Skills.Find(_Skill => _Skill.Name == "Hit").Skill_Effects.Add(new Skill_Effect { Apply_Value = "Hp", Value = -5 });
            Skills.Find(_Skill => _Skill.Name == "Hit").Narratives.Add("Y hits Z");
        }

        public Mob(int _Level)
        {
            //Habitats.Add("Normal");
            Begin_Round = 0;
            End_Round = -1;
            Level = _Level;
            Exp = Mathf.Pow(_Level, 1.25f);
            Skills.Add(new Skill { Name = "Hit", Hp_Spend = 0, Mp_Spend = 0, Hp_Multiply = 0, Mp_Multiply = 0, Strength_Multiply = 0, Agility_Multiply = 0, Wit_Multiply = 0, Description = "Normal Attack" });
            Skills.Find(_Skill => _Skill.Name == "Hit").Skill_Effects.Add(new Skill_Effect { Apply_Value = "Hp", Value = -5 });
            Skills.Find(_Skill => _Skill.Name == "Hit").Narratives.Add("Y hits Z");
        }

        public void Get_Exp(float _Exp)
        {
            Exp += _Exp;
            Update_Level();
        }

        public void Consum_Food(Item _Food)
        {
            Hp += _Food.Hp_Plus;
            Mp += _Food.Mp_Plus;
            Strength += _Food.Strength_Plus;
            Agility += _Food.Agility_Plus;
            Wit += _Food.Wit_Plus;
            Hp = (int)(Hp * _Food.Hp_Multiply);
            Mp = (int)(Mp * _Food.Mp_Multiply);
            Strength = (int)(Strength * _Food.Strength_Multiply);
            Agility = (int)(Agility * _Food.Agility_Multiply);
            Wit = (int)(Wit * _Food.Wit_Multiply);
            if(Hp > Original_Hp)
            {
                Hp = Original_Hp;
            }
            if (Mp > Original_Mp)
            {
                Mp = Original_Mp;
            }
            Original_Strength += _Food.Strength_Plus;
            Original_Agility += _Food.Agility_Plus;
            Original_Wit += _Food.Wit_Plus;
            Original_Hp = (int)(Original_Hp * _Food.Hp_Multiply);
            Original_Mp = (int)(Original_Mp * _Food.Mp_Multiply);
            Original_Strength = (int)(Original_Strength * _Food.Strength_Multiply);
            Original_Agility = (int)(Original_Agility * _Food.Agility_Multiply);
            Original_Wit = (int)(Original_Wit * _Food.Wit_Multiply);
            Items.Remove(_Food);
        }

        public void Update_Values()
        {
            if (!Inis)
            {
                Original_Hp = Hp;
                Original_Mp = Mp;
                Original_Strength = Strength;
                Original_Agility = Agility;
                Original_Wit = Wit;
                Inis = true;
            }
            int _temp_Hp, _temp_Mp;
            _temp_Hp = Hp;
            _temp_Mp = Mp;
            Hp = Original_Hp + (Level * 5);
            Mp = Original_Mp + (Level * 5);
            Strength = Original_Strength + (Level * 5);
            Agility = Original_Agility + (Level * 5);
            Wit = Original_Wit + (Level * 5);
            foreach (Item _Item in Items)
            {
                if (_Item.Wearing)
                {
                    Hp += _Item.Hp_Plus;
                    Mp += _Item.Mp_Plus;
                    Strength += _Item.Strength_Plus;
                    Agility += _Item.Agility_Plus;
                    Wit += _Item.Wit_Plus;
                }  
            }
            foreach (Item _Item in Items)
            {
                if (_Item.Wearing)
                {
                    Hp = (int)(Hp * _Item.Hp_Multiply);
                    Mp = (int)(Mp * _Item.Mp_Multiply);
                    Strength = (int)(Strength * _Item.Strength_Multiply);
                    Agility = (int)(Agility * _Item.Agility_Multiply);
                    Wit = (int)(Wit * _Item.Wit_Multiply);
                }     
            }
            Max_Hp = Hp;
            Max_Mp = Mp;
            if (Hp > _temp_Hp)
            {
                Hp = _temp_Hp;
            }
            if (Mp > _temp_Mp)
            {
                Mp = _temp_Mp;
            }
        }

        public void Update_Level()
        {
            Level = (int)Mathf.Pow(Exp, 0.8f);
            Update_Values();
        }

        public string Content(int _Type)
        {
            if (_Type == 1)
            {
                return Name + ", " + Hp + ", " + Mp + ", " + Strength + ", " + Agility + ", " + Wit;
            }
            return ID + ", " + Name + ", " + Exp + ", " + Level + ", " + Hp + ", " + Mp + ", " + Strength + ", " + Agility + ", " + Wit + ", " + Description + ", " + Attributes[0] + ", " + Skills[0].Name + ", " + Items[0].Name + ", " + Habitats[1];
        }
    }

    

    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Hp_Spend { get; set; }
        public int Mp_Spend { get; set; }
        public float Hp_Multiply { get; set; }
        public float Mp_Multiply { get; set; }
        public float Strength_Multiply { get; set; }
        public float Agility_Multiply { get; set; }
        public float Wit_Multiply { get; set; }
        public string Description { get; set; }
        public List<string> Narratives = new List<string>();
        public List<string> Attributes = new List<string>();
        public List<Skill_Effect> Skill_Effects = new List<Skill_Effect>();

        public string Content()
        {
            return ID + ", " + Name + ", " + Hp_Spend + ", " + Mp_Spend + ", " + Hp_Multiply + ", " + Mp_Multiply + ", " + Strength_Multiply + ", " + Agility_Multiply + ", " + Wit_Multiply + ", " + Description + ", " + Narratives[0] + ", " + Attributes[0] + ", " + Skill_Effects[0].Apply_Value;
        }
    }

    public class Attribute_Rule
    {
        public string Attribute_1 { get; set; }
        public float Multiply { get; set; }
        public string Attribute_2 { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public Node Node_Now { get; set; }
        public string Type { get; set; }
        public bool Wearing { get; set; }
        public int Hp_Plus { get; set; }
        public int Mp_Plus { get; set; }
        public int Strength_Plus { get; set; }
        public int Agility_Plus { get; set; }
        public int Wit_Plus { get; set; }
        public float Hp_Multiply { get; set; }
        public float Mp_Multiply { get; set; }
        public float Strength_Multiply { get; set; }
        public float Agility_Multiply { get; set; }
        public float Wit_Multiply { get; set; }
        public string Description { get; set; }

        public float Drop_Rate { get; set; }

        public Item()
        {
            Type = "Gear";
            Wearing = false;
        }
    }
}

public class Skill_Effect
{
    public string Apply_Target { get; set; }
    public string Apply_Value { get; set; }
    public List<Skill_Effect_A_and_M> Apply_Attribute_and_Multiply = new List<Skill_Effect_A_and_M>();
    public int Value { get; set; }

    public Skill_Effect()
    {
        Apply_Target = "Target";
    }
}

public class Skill_Effect_A_and_M
{
    public string Apply_Attribute { get; set; }
    public float Multiply { get; set; }
}
