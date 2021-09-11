using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mob_stuff;

namespace Map_stuff
{
    public class Node
    {
        public int ID { get; set; }
        public Vector3 Pos { get; set; }
        public string Type { get; set; }
        public GameObject Game_Object { get; set; }
        public List<int> Ins = new List<int>();
        public List<int> Outs = new List<int>();
        public List<GameObject> Out_Links_Game_Object = new List<GameObject>();
        public List<Link> Out_Links = new List<Link>();
        public List<GameObject> Players = new List<GameObject>();
        public List<GameObject> Mobs = new List<GameObject>();
        public List<Item> Stuffs = new List<Item>();

        public Node()
        {
            Type = "Normal";
        }
    }

    public class Link
    {
        public int ID { get; set; }
        public List<Vector3> Pos = new List<Vector3>();
    }

    public class Round_Title
    {
        public int Num { get; set; }
        public string Title { get; set; }
    }


}

