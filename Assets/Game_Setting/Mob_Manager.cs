using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mob_stuff;

public class Mob_Manager : MonoBehaviour
{
    public List<Attribute_Rule> Attribute_Rules = new List<Attribute_Rule>();


    public List<string> Attribute_Kinds = new List<string>();
    public List<string> Habitat_Kinds = new List<string>();
    public List<Item> Item_Kinds = new List<Item>();
    public List<Skill> Skill_Kinds = new List<Skill>();
    public List<Mob> Mob_Kinds = new List<Mob>();
    public List<Mob> Player_Mob_Kinds = new List<Mob>();
    public List<GameObject> Mobs = new List<GameObject>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            
            foreach (Skill a in Skill_Kinds)
            {
                Debug.Log(a.Content());
            }
            foreach (Mob a in Mob_Kinds)
            {
                Debug.Log(a.Content(0));
            }
            foreach (Item a in Item_Kinds)
            {
                Debug.Log(a.Name+a.Drop_Rate);
            }
        }
    }
}
