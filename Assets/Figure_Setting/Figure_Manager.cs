using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mob_stuff;
using Photon.Pun;

public class Figure_Manager : MonoBehaviour
{
    public Mob Figure;
    public string Figure_Url;
    PhotonView photonView;
    public List<string> temp_Skills = new List<string>();
    public List<string> temp_Items = new List<string>();
    void Start()
    {
        photonView = PhotonView.Get(this);
        Figure = new Mob { ID = -1, Name = "None", Hp = -1, Mp = -1, Strength = -1, Agility = -1, Wit = -1 ,Description = "???"};
        DontDestroyOnLoad(gameObject);
    }



    void Update()
    {

    }

   /*
    public void Set_Figure(string _Name,int _Hp, int _Mp, int _Strength, int _Agility, int _Wit, string _Description)
    {
        Figure.Name = _Name;
        Figure.Hp = _Hp;
        Figure.Mp = _Mp;
        Figure.Strength = _Strength;
        Figure.Agility = _Agility;
        Figure.Wit = _Wit;
        Figure.Description = _Description;
    }
    */
}
