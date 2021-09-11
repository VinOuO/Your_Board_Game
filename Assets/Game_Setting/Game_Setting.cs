using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Map_stuff;
using Com.Vincent.Board_Game;

public class Game_Setting : MonoBehaviour
{

    public GameObject Node, Point, Arrow;
    GameObject MM;
    public string Stage = "Setting_Node";
    //public List<string> temp_Habitats = new List<string>();
    //public List<string> temp_Attributes = new List<string>();
    void Start()
    {
        DontDestroyOnLoad(this);
        MM = GameObject.Find("Map_Manager");
    }

    void Update()
    {
        if (Stage == "Setting_Node")
        {
            Node_Seeting();
        }
        else if (Stage == "Setting_Link")
        {
            Link_Seeting();
        }
        else if (Stage == "End")
        {
            //GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>().Attribute_Kinds = temp_Attributes;
            //GameObject.Find("Mob_Manager").GetComponent<Mob_Manager>().Habitat_Kinds = temp_Habitats;
            GameObject.Find("Network_Manager").GetComponent<Network_Manager>().Change_All_Player_Scene("Figure_Setting");
            Stage = "Finish";
            Destroy(this.gameObject);
        }
    }

   public void Change_Stage()
    {
        if (Stage == "Start")
        {
            Stage = "Setting_Node";
        }
        else if (Stage == "Setting_Node")
        {
            Stage = "Setting_Link";
        }
        else if (Stage == "Setting_Link")
        {
            Stage = "Other_Setting";
        }
    }

    void Node_Seeting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 point = new Vector3();
            Vector2 mousePos = new Vector2();
            GameObject _temp;

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            point.z = 0;

            _temp = Instantiate(Node, point, Node.transform.rotation);
            MM.GetComponent<Map_Manager>().Nodes.Add(new Node { ID = MM.GetComponent<Map_Manager>().Nodes.Count, Pos = _temp.transform.position, Type = GameObject.Find("Node_Habitat_Drop_down").GetComponent<Dropdown>().captionText.text });
        }
    }

    public int[] Link_Nodes_ID = new int[2];

    void Link_Seeting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 point = new Vector3();
            Vector2 mousePos = new Vector2();

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            point.z = 0;

            float _temp = Mathf.Infinity;
            for (int i = 0; i < MM.GetComponent<Map_Manager>().Nodes.Count; i++)
            {
                if (Vector3.Distance(MM.GetComponent<Map_Manager>().Nodes[i].Pos, point) < _temp)
                {
                    _temp = Vector3.Distance(MM.GetComponent<Map_Manager>().Nodes[i].Pos, point);
                    Link_Nodes_ID[0] = MM.GetComponent<Map_Manager>().Nodes[i].ID;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Vector3 point = new Vector3();
            Vector2 mousePos = new Vector2();

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;

            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            point.z = 0;

            float _temp = Mathf.Infinity;
            for (int i = 0; i < MM.GetComponent<Map_Manager>().Nodes.Count; i++)
            {
                if (Vector3.Distance(MM.GetComponent<Map_Manager>().Nodes[i].Pos, point) < _temp)
                {
                    _temp = Vector3.Distance(MM.GetComponent<Map_Manager>().Nodes[i].Pos, point);
                    Link_Nodes_ID[1] = MM.GetComponent<Map_Manager>().Nodes[i].ID;
                }
            }
            MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Outs.Add(MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].ID);
            MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Ins.Add(MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].ID);
            Vector3 _Point_Pos = MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos;
            Vector3 _Dis = (MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos - MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos).normalized;
            _Point_Pos += _Dis;
            while (Vector3.Distance(MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos, _Point_Pos + _Dis * 3) > 1)
            {
                _Point_Pos += _Dis;
                Instantiate(Point, _Point_Pos, Quaternion.FromToRotation(transform.up, MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos - MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos));
                //PhotonNetwork.Instantiate(Point.name, _Point_Pos, Quaternion.FromToRotation(transform.up, MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos - MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos));
            }
            Instantiate(Arrow, _Point_Pos + _Dis, Quaternion.FromToRotation(transform.up, MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos - MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos));
            //PhotonNetwork.Instantiate(Arrow.name, _Point_Pos + _Dis, Quaternion.FromToRotation(transform.up, MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[1]].Pos - MM.GetComponent<Map_Manager>().Nodes[Link_Nodes_ID[0]].Pos));
        }
    }

}
