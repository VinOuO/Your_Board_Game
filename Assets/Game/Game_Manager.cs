using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Map_stuff;
using Mob_stuff;

public class Game_Manager : MonoBehaviour
{
    public GameObject Figure;
    public GameObject Mob_Prefab;
    public List<GameObject> Players = new List<GameObject>();
    public List<Attribute_Rule> Attribute_Rules = new List<Attribute_Rule>();
    public int Round = 0;
    public int Playing_Player = 0;
    GameObject MM;
    Mob_Manager MobM;
    Figure_Manager FM;
    Round_BroadCast RBC;
    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        FM = GameObject.Find("Figure_Manager").GetComponent<Figure_Manager>();
        MM = GameObject.Find("Map_Manager");
        MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        RBC = GameObject.Find("Round_BroadCast").GetComponent<Round_BroadCast>();
        //CreatPlayerPrefab(2);
        StartCoroutine(Start_Game());
    }

    void Update()
    {
        
    }

    public IEnumerator Start_Game()
    {
        yield return new WaitForSeconds(5f);
        if (MobM.Mob_Kinds.Count > 0)//This line is for testing
        {
            CreatMobs();
        }
        /*
        for(int i = 0; i <=10; i++)
        {
            Mob _temp = new Mob(0) { ID = -1, Name = "Snail_" + i, Hp = 20, Mp = 100, Strength = 10, Agility = 10, Wit = 10, Description = "???", Begin_Round = 0, End_Round = 3};
            if (i == 3)
            {
                _temp.Agility = 9999;
            }
            _temp.Items.Add(new Item { Name = "Snail Poop", Hp_Plus = 10, Mp_Plus = 10, Strength_Plus = 10, Agility_Plus = 10, Wit_Plus = 10, Hp_Multiply = 1, Mp_Multiply = 1, Strength_Multiply = 1, Agility_Multiply = 1, Wit_Multiply = 1, Description = "A poop", Drop_Rate = 1 });
            CreatMob(_temp, MM.GetComponent<Map_Manager>().Nodes[0]);
        }
        */
        yield return new WaitForSeconds(1f);
        RBC.Master_Round_BroadCast_Type("Round" + Round);
        BroadCast_Round_Title();
        Players[Playing_Player].GetComponent<Figure>().Start_Turn();
    }

    public void Next_Round()
    {
        Round++;
        if (MobM.Mob_Kinds.Count > 0)//This line is for testing
        {
            CreatMobs();
        }

        for (int i = 0; i < MobM.Mobs.Count; i++)
        {
            if (MobM.Mobs[i].GetComponent<Mob_Behavior>().This_Mob.End_Round >=0 && MobM.Mobs[i].GetComponent<Mob_Behavior>().This_Mob.End_Round <= GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Round)
            {
                DeletMob(MobM.Mobs[i]);
                i--;
            }
        }

        RBC.Master_Round_BroadCast_Type("Round" + Round);
        BroadCast_Round_Title();
        Debug.Log("New Round, Round " + Round + "!!!!!!!!!!!!!!!!!!!!");
        Playing_Player = 0;
    }

    void BroadCast_Round_Title()
    {
        int _temp = MM.GetComponent<Map_Manager>().Round_Titles.FindIndex(_r => _r.Num == Round);
        if (_temp != -1)
        {
            RBC.Master_Round_BroadCast_Type(MM.GetComponent<Map_Manager>().Round_Titles[_temp].Title);
        }
    }

    public void Next_Player()
    {
        Playing_Player++;
        if(Playing_Player == Players.Count)
        {
            Next_Round();
        }
        Players[Playing_Player].GetComponent<Figure>().Start_Turn();
    }

    public void Player_Dead_Next_Player(GameObject _Player)
    {
        Players.Remove(_Player);
        if (Playing_Player == Players.Count)
        {
            Next_Round();
        }
        Players[Playing_Player].GetComponent<Figure>().Start_Turn();
    }

    public IEnumerator CreatPlayerPrefab(string _Figure_Name, int _Figure_Hp, int _Figure_Mp, int _Figure_Strength, int _Figure_Agility, int _Figure_Wit, int _Figure_Level, string _Figure_Description, string _Figure_Url, List<string> _temp_Attributes, List<string> _temp_Items, List<string> _temp_Skills)
    {
        yield return new WaitForSeconds(1f);
        Debug.LogError(_Figure_Name);
        Debug.LogError(_temp_Items);
        if (MobM == null)
        {
            MobM = GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>();
        }

        GameObject _temp = PhotonNetwork.Instantiate(Figure.name, Vector3.zero, Figure.transform.rotation, 0);
        Players.Add(_temp);
        _temp.GetComponent<Figure>().Pic_url = _Figure_Url;
        _temp.name = _Figure_Name;
        _temp.transform.SetAsFirstSibling();
        _temp.GetComponent<Figure>().Belong_Player_Name = _temp.name;
        _temp.GetComponent<Figure>().Inis_Transport();

        List<Skill> _temp_Ss = new List<Skill>();
        Debug.Log(_temp_Skills.Count);
        foreach (string _S in _temp_Skills)
        {
            _temp_Ss.Add(MobM.GetComponent<Mob_Manager>().Skill_Kinds.Find(_s => _s.Name == _S));
        }
        List<Item> _temp_Ts = new List<Item>();
        
        foreach (string _S in _temp_Items)
        {
            Debug.LogError(_S);
            _temp_Ts.Add(MobM.GetComponent<Mob_Manager>().Item_Kinds.Find(_t => _t.Name == _S));
        }
        _temp.GetComponent<Figure>().Build_This_Mob(_Figure_Name, _Figure_Hp, _Figure_Mp, _Figure_Strength, _Figure_Agility, _Figure_Wit, _Figure_Level, _Figure_Description, _temp_Attributes, _temp_Ts, _temp_Ss);
        
    }

    float Spawn_Rate = 1f;

    public void CreatMobs()
    {
        
        //-----------------------------------------------Mod
        List<Node> _temp_Nodes = new List<Node>();
        foreach (Mob _Mob_Kind in MobM.Mob_Kinds)
        {
            if (_Mob_Kind.Begin_Round == GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Round)
            {
                foreach (Node _N in MM.GetComponent<Map_Manager>().Nodes)
                {
                    if (_Mob_Kind.Habitats.Exists(_H => _H == _N.Type))
                    {
                        _temp_Nodes.Add(_N);
                    }
                }
                int _Spawn_Num = (int)(MM.GetComponent<Map_Manager>().Nodes.Count * 5f);
                while (_Spawn_Num > 0)
                {
                    CreatMob(_Mob_Kind, _temp_Nodes[Random.Range(0, _temp_Nodes.Count)]);
                    _Spawn_Num--;
                }
            }
        }
        //-----------------------------------------------Mod
        
        if (MobM.Mobs.Count < (int)(MM.GetComponent<Map_Manager>().Nodes.Count * Spawn_Rate))
        {
            List<int> _Nodes_Chosen_Time = new List<int>();
            for(int i = 0; i < MM.GetComponent<Map_Manager>().Nodes.Count; i++)
            {
                _Nodes_Chosen_Time.Add(0);
            }

            Node _Node;
            bool All_Spawned = false;
            while((int)(MM.GetComponent<Map_Manager>().Nodes.Count * Spawn_Rate) - MobM.Mobs.Count > 0 && !All_Spawned)
            {
                List<Mob> _temp_Mob_Kinds = new List<Mob>();
                //-----------------------------------------------Node
                _Node = MM.GetComponent<Map_Manager>().Nodes[Random.Range(0, MM.GetComponent<Map_Manager>().Nodes.Count)];
                _Nodes_Chosen_Time[_Node.ID]++;
                foreach (Mob _Mob_Kind in MobM.Mob_Kinds)
                {
                    if (_Mob_Kind.Habitats.Exists(_H => _H == _Node.Type) && _Mob_Kind.Begin_Round <= GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Round && _Mob_Kind.End_Round > GameObject.Find("Game_Manager").GetComponent<Game_Manager>().Round)
                    {
                        _temp_Mob_Kinds.Add(_Mob_Kind);
                    }
                }
                if (_temp_Mob_Kinds.Count != 0)
                {
                    CreatMob(_temp_Mob_Kinds[Random.Range(0, _temp_Mob_Kinds.Count)], _Node);
                }
                All_Spawned = true;
                foreach (int _Node_Chosen_Time in _Nodes_Chosen_Time)
                {
                    if(_Node_Chosen_Time == 0)
                    {
                        All_Spawned = false;
                    }
                }
                //-----------------------------------------------Node
            }
        }
        
    }

    public void CreatMob(Mob _Mob_Kind, Node _Node)
    {
        GameObject _temp;

        _temp = Instantiate(Mob_Prefab);
        MobM.Mobs.Add(_temp);

        _temp.GetComponent<Mob_Behavior>().This_Mob = new Mob(0) { Name = _Mob_Kind.Name, Hp = _Mob_Kind.Hp, Mp = _Mob_Kind.Mp, Strength = _Mob_Kind.Strength, Agility = _Mob_Kind.Agility, Wit = _Mob_Kind.Wit,Begin_Round = _Mob_Kind.Begin_Round,End_Round = _Mob_Kind.End_Round , Description = _Mob_Kind.Description}; 
        foreach(string _A in _Mob_Kind.Attributes)
        {
            _temp.GetComponent<Mob_Behavior>().This_Mob.Attributes.Add(_A);
        }
        foreach (Item _I in _Mob_Kind.Items)
        {
            _temp.GetComponent<Mob_Behavior>().This_Mob.Items.Add(_I);
        }
        foreach (string _H in _Mob_Kind.Habitats)
        {
            _temp.GetComponent<Mob_Behavior>().This_Mob.Habitats.Add(_H);
        }
        _temp.name = _temp.GetComponent<Mob_Behavior>().This_Mob.Name;
        _Node.Mobs.Add(_temp);
        _temp.GetComponent<Mob_Behavior>().This_Mob.Node_Now = _Node;

        for (int i = 0; i < _temp.GetComponent<Mob_Behavior>().This_Mob.Items.Count; i++)
        {
            if (Random.Range(0, 100) > _temp.GetComponent<Mob_Behavior>().This_Mob.Items[i].Drop_Rate * 100)
            {
                _temp.GetComponent<Mob_Behavior>().This_Mob.Items.Remove(_temp.GetComponent<Mob_Behavior>().This_Mob.Items[i]);
            }
        }
        
    }

    public void DeletMob(GameObject _Mob)
    {
        _Mob.GetComponent<Mob_Behavior>().This_Mob.Node_Now.Mobs.Remove(_Mob);
        MobM.Mobs.Remove(_Mob);
        Destroy(_Mob);
    }
}
