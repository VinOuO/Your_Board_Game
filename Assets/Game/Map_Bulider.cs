using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Map_stuff;

public class Map_Bulider : MonoBehaviourPun
{
    GameObject MM;
    public GameObject Node_Pre, Point_Pre, Arrow_Pre, Link_Pre;
    PhotonView photonView;
    void Start()
    {
        photonView = PhotonView.Get(this);
        MM = GameObject.Find("Map_Manager");
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Build_Map());
        }
    }

    void Update()
    {
        
    }



    IEnumerator Build_Map()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(MM.GetComponent<Map_Manager>().Inis_Point(300));
        int _Link_ID = 0;
        foreach (Node _Node in MM.GetComponent<Map_Manager>().Nodes)
        {
            _Node.Game_Object = PhotonNetwork.Instantiate(Node_Pre.name, _Node.Pos, Node_Pre.transform.rotation, 0);
            _Node.Game_Object.GetComponent<Map_Node>().This_Node = _Node;

            for (int i = 0; i < _Node.Outs.Count; i++)
            {
                _Node.Out_Links.Add(new Link { ID = i });
                Vector3 _To_Pos = MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos;
                Vector3 _temp = (_To_Pos - _Node.Pos).normalized;
                Vector3 Mid = (_Node.Pos + _To_Pos) / 2;
                
                foreach (Photon.Realtime.Player _Player in PhotonNetwork.PlayerListOthers)
                {
                    Debug.Log("???");
                    photonView.RPC("Link_Inis", _Player, _Link_ID);
                }
                
                GameObject _temp_Link = Instantiate(Link_Pre, Vector3.zero, Link_Pre.transform.rotation);
                _Node.Out_Links_Game_Object.Add(_temp_Link);
                _temp_Link.name = "Link_" + _Link_ID;

                for (int j = 0; Vector3.Distance(Mid + _temp * j, _To_Pos) >= 1; j++)
                {
                    Vector3 _temp2 = Quaternion.Euler(0, 0, -90) * _temp;
                    _Node.Out_Links[i].Pos.Add(Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, _To_Pos, j) * _temp2);
                    if (j != 0)
                    {

                        _Node.Out_Links[i].Pos.Insert(0, Mid - _temp * j + Hight_of_Link_Point(_Node.Pos, _To_Pos, j) * _temp2);
                    }
                }
                _Link_ID++;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Build_Map2()
    {
        yield return new WaitForSeconds(2f);
        int _Link_ID = 0;
        foreach (Node _Node in MM.GetComponent<Map_Manager>().Nodes)
        {
            _Node.Game_Object = PhotonNetwork.Instantiate(Node_Pre.name, _Node.Pos, Node_Pre.transform.rotation, 0);
            _Node.Game_Object.GetComponent<Map_Node>().This_Node = _Node;
            for (int i = 0; i < _Node.Outs.Count; i++)
            {
                _Node.Out_Links.Add(new Link { ID = i });
                Vector3 _temp = (MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos - _Node.Pos).normalized;
                Vector3 Mid = (_Node.Pos + MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos) / 2;
                
                foreach (Photon.Realtime.Player _Player in PhotonNetwork.PlayerListOthers)
                {
                    photonView.RPC("Link_Inis", _Player, _Link_ID);
                }
                
                GameObject _temp_Link=Instantiate(Link_Pre, Vector3.zero, Link_Pre.transform.rotation);
                _Node.Out_Links_Game_Object.Add(_temp_Link);
                _temp_Link.name = "Link_" + _Link_ID;

                GameObject _temp3;
                Vector3 _temp_Vector;
                for (int j = 0; Vector3.Distance(Mid + _temp * j, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos) >= 1; j++)
                {
                    Vector3 _temp2 = Quaternion.Euler(0, 0, -90) * _temp;
                    if(Vector3.Distance(Mid + _temp * j, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos) <= 2)
                    {
                        /*
                        _temp3 = PhotonNetwork.Instantiate(Arrow_Pre.name, Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2, Arrow_Pre.transform.rotation, 0);
                        if (MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos.x - _temp3.transform.position.x >= 0)
                        {
                            _temp3.transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos - _temp3.transform.position));
                        }
                        else
                        {
                            _temp3.transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.down, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos - _temp3.transform.position) + 180);
                        }
                        */
                        //_temp3.transform.SetParent(_temp_Link.transform);
                        //_temp3.transform.SetAsLastSibling();
                        //photonView.RPC("Link_Set_Parent", RpcTarget.All, _temp3.GetComponent<PhotonView>().ViewID, _temp_Link.GetComponent<PhotonView>().ViewID, "Last");
                        _temp_Vector = Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2;
                        photonView.RPC("Link_Inis_Point_Set_Parent", RpcTarget.All, _temp_Vector.x, _temp_Vector.y, _temp_Vector.z, _Link_ID, "Last");

                    }
                    else
                    {
                        //_temp3 = PhotonNetwork.Instantiate(Point_Pre.name, Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2, Point_Pre.transform.rotation, 0);
                        //_temp3.transform.SetParent(_temp_Link.transform);
                        //_temp3.transform.SetAsLastSibling();
                        _temp_Vector = Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2;
                        photonView.RPC("Link_Inis_Point_Set_Parent", RpcTarget.All, _temp_Vector.x, _temp_Vector.y, _temp_Vector.z, _Link_ID, "Last");
                    }
                    _Node.Out_Links[i].Pos.Add(Mid + _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2);
                    if (j != 0)
                    {
                        //_temp3 = PhotonNetwork.Instantiate(Point_Pre.name, Mid - _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2, Point_Pre.transform.rotation, 0);
                        //_temp3.transform.SetParent(_temp_Link.transform);
                        //_temp3.transform.SetAsFirstSibling();
                        _temp_Vector = Mid - _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2;
                        photonView.RPC("Link_Inis_Point_Set_Parent", RpcTarget.All, _temp_Vector.x, _temp_Vector.y, _temp_Vector.z, _Link_ID, "First");
                        _Node.Out_Links[i].Pos.Insert(0, Mid - _temp * j + Hight_of_Link_Point(_Node.Pos, MM.GetComponent<Map_Manager>().Nodes[_Node.Outs[i]].Pos, j) * _temp2);
                    }
                    
                }
                _Link_ID++;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    [PunRPC]
    void Link_Inis(int _Link_ID)
    {
        GameObject _temp_Link = Instantiate(Link_Pre, Vector3.zero, Link_Pre.transform.rotation);
        _temp_Link.name = "Link_" + _Link_ID;
    }

    [PunRPC]
    void Link_Inis_Point_Set_Parent(float _x, float _y, float _z, int _Parent_ID, string _Rank)
    {
        StartCoroutine(Link_Set_Parent_Clock(new Vector3(_x, _y, _z), _Parent_ID, _Rank));
    }

    IEnumerator Link_Set_Parent_Clock(Vector3 _Point_Pos, int _Link_ID, string _Rank)
    {
        while (GameObject.Find("Link_" + _Link_ID) == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        GameObject _temp_Point =  Instantiate(Point_Pre, _Point_Pos, Point_Pre.transform.rotation);
        _temp_Point.transform.SetParent(GameObject.Find("Link_" + _Link_ID).transform);
        switch (_Rank)
        {
            case "First":
                _temp_Point.transform.SetAsFirstSibling();
                break;
            case "Last":
                _temp_Point.transform.SetAsLastSibling();
                break;
        }
    }

    float Hight_of_Link_Point(Vector3 _A, Vector3 _B, float _X)
    {
        float _Dis = Vector3.Distance(_B, _A) / 2, a;
        a = 2 / (_Dis * _Dis);
        return a * (_X * _X) - 2;
    }

    public IEnumerator Get_BK(string _BK_url)
    {
        Debug.Log(_BK_url);
        Texture2D BK_Image;
        BK_Image = new Texture2D(4, 4, TextureFormat.RGB24, false);
        //WWW www = new WWW(@"V:\work\碩士\高等遊戲設計\Board_Game_Art\Map.png");
        WWW www = new WWW(_BK_url);
        yield return www;
        www.LoadImageIntoTexture(BK_Image);
        GameObject.Find("BK").GetComponent<SpriteRenderer>().sprite = Sprite.Create(BK_Image, new Rect(0, 0, BK_Image.width, BK_Image.height), new Vector2(0.5f, 0.5f));
    }
}
