using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map_stuff;
using Mob_stuff;
using Photon.Pun;

public class Map_Manager : MonoBehaviour
{
    public List<Node> Nodes = new List<Node>();
    public List<Item> Items = new List<Item>();
    public List<Round_Title> Round_Titles = new List<Round_Title>();
    public GameObject Point_Pre;
    public List<GameObject> Points = new List<GameObject>();
    PhotonView photonView;
    private Sprite BK;
    public string BK_url;// = @"https://i.imgur.com/mnyFzS8.png";
    void Start()
    {
        photonView = PhotonView.Get(this);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }

    public IEnumerator Inis_Point(int _Amount)
    {
        GameObject _Point;

        while(_Amount > 0)
        {
            _Point = Instantiate(Point_Pre);
            Points.Add(_Point);
            _Amount--;
            yield return new WaitForSeconds(0.005f);
        }
    }

    public void Inis_Ins_Point()
    {
        photonView.RPC("Global_Ins_Point",RpcTarget.All);
    }

    [PunRPC]
    public void Global_Ins_Point()
    {
        StartCoroutine(Inis_Point(300));
    }

    public GameObject Take_Point()
    {
        GameObject _Point = Points[0];
        Points.RemoveAt(0);
        Debug.Log("Point Count: " + Points.Count);
        if(Points.Count < 50)
        {
            StartCoroutine(Inis_Point(50));
        }
        return _Point;
    }

    public void Return_Point(GameObject _Point)
    {
        Debug.Log("Point Count1: " + Points.Count);
        Points.Add(_Point);
        Debug.Log("Point Count2: " + Points.Count);
    }

    public IEnumerator Get_BK()
    {

        Texture2D BK_Image;
        BK_Image = new Texture2D(4, 4, TextureFormat.RGB24, false);
        WWW www = new WWW(BK_url);
        yield return www;
        www.LoadImageIntoTexture(BK_Image);
        GameObject.Find("BK").GetComponent<SpriteRenderer>().sprite = Sprite.Create(BK_Image, new Rect(0, 0, BK_Image.width, BK_Image.height), new Vector2(0.5f, 0.5f));
    }

}



