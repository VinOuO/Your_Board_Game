using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mob_stuff;
using Map_stuff;
public class Mob_Behavior : MonoBehaviour
{

    public Mob This_Mob;
    GameObject MM;
    GameObject GM;
    Mob_Manager MobM;
    Text Chronicle_Content;
    public BroadCast BC;

    void Start()
    {
        MM = GameObject.Find("Map_Manager");
        GM = GameObject.Find("Game_Manager");
        MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        BC = GameObject.Find("BroadCast").GetComponent<BroadCast>();
        Chronicle_Content = GameObject.Find("Chronicle").GetComponent<Chronicle>().Content;
    }

    public void Update()
    {
        /*
        Debug.Log("Is Mob Here?" + GetComponent<Mob_Behavior>().This_Mob.Node_Now.ID);
        if (GetComponent<Mob_Behavior>().This_Mob.Node_Now.Mobs.Exists(_M => GetComponent<Mob_Behavior>().This_Mob == _M.GetComponent<Mob_Behavior>().This_Mob))
        {
            Debug.Log("Mob Here!666");
        }
        else
        {
            foreach (Node _N in MM.GetComponent<Map_Manager>().Nodes)
            {
                if (GetComponent<Mob_Behavior>().This_Mob.Node_Now.Mobs.Exists(_M => GetComponent<Mob_Behavior>().This_Mob == _M.GetComponent<Mob_Behavior>().This_Mob))
                {
                    Debug.Log("Mob is acturally Here:" + _N.ID);
                }

            }

        }
        */
    }

    public void Drop_Items()
    {
        while (This_Mob.Items.Count > 0)
        {
            This_Mob.Items[This_Mob.Items.Count - 1].Node_Now = This_Mob.Node_Now;
            This_Mob.Node_Now.Stuffs.Add(This_Mob.Items[This_Mob.Items.Count - 1]);
            This_Mob.Items.Remove(This_Mob.Items[This_Mob.Items.Count - 1]);
        }
    }

    public void Cast_Skill(GameObject _Apply_Mob, Skill _Skill)
    {
        Chronicle_Content.text += "    " + This_Mob.Name + " used " + _Skill.Name + "\n";
        BC.Master_BroadCast_Type(This_Mob.Name + " used " + _Skill.Name);
        if (_Apply_Mob.GetComponent<Mob_Behavior>() != null)
        {
            Debug.Log("Player_Agility: " + This_Mob.Agility);
            Debug.Log("Slime_Agility: " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Agility);
            if (_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Agility > This_Mob.Agility)
            {
                int _temp = Random.Range(0, _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Skills.Count);
                _temp = 0;
                Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " finds out and attacks first!\n";
                BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " finds out and attacks first");
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
                        if (GetComponent<Mob_Behavior>().This_Mob.Attributes.Exists(_A => _A == _A_and_M.Apply_Attribute))
                        {
                            _Value *= _A_and_M.Multiply;
                        }
                    }
                    switch (_Skill_Effect.Apply_Value)
                    {
                        case "Hp":
                            GetComponent<Mob_Behavior>().This_Mob.Hp += (int)_Value;
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
                            GetComponent<Mob_Behavior>().This_Mob.Mp += (int)_Value;
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

                    break;
                case "Target":
                    if (_Apply_Mob.GetComponent<Mob_Behavior>() != null)
                    {
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
                            Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + This_Mob.Name + "\n";
                            BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + This_Mob.Name);
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
                            Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + This_Mob.Name + "\n";
                            BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + This_Mob.Name);
                            _Apply_Mob.GetComponent<Figure>().After_Dead(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name, _Apply_Mob, This_Mob.Name);
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
                                        BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " loses " + (int)_Value + " Hp");
                                    }
                                    else
                                    {
                                        Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " gains " + -(int)_Value + " Hp" + "\n";
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
                                Chronicle_Content.text += "    " + _Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + This_Mob.Name + "\n";
                                BC.Master_BroadCast_Type(_Mob.GetComponent<Mob_Behavior>().This_Mob.Name + " is killed by " + This_Mob.Name);
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
                                Chronicle_Content.text += "    " + _Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + This_Mob.Name + "\n";
                                BC.Master_BroadCast_Type(_Apply_Mob.GetComponent<Figure>().This_Mob.Name + " is killed by " + This_Mob.Name);
                                _Apply_Mob.GetComponent<Figure>().After_Dead(_Apply_Mob.GetComponent<Figure>().Belong_Player_Name, _Apply_Mob, This_Mob.Name);
                            }
                        }

                    }
                    break;
            }

        }

    }



}
