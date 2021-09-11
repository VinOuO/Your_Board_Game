using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Link : MonoBehaviour
{
    public bool Is_Showing = false;
    public int Is_Preparing = 0;
    float _Last_Show_Time = 0;
    Map_Manager MM;
    GameObject _Point;

    private void Start()
    {
        MM = GameObject.Find("Map_Manager").GetComponent<Map_Manager>();
    }


    public void Ask_Show_Route()
    {
        if (!Is_Showing)
        {
            StartCoroutine(Show_Route());
        }
    }

    public IEnumerator Show_Route()
    {
        int Show_Time = 0;
        Is_Showing = true;
        int _Index = 0;
        string _Color_Now = "Half";
        Color _Half_Trans = new Color(0, 0, 0, 0.5f), _Full_Trans = Color.clear;
        while (Show_Time < 2)
        {
            switch (_Color_Now)
            {
                case "Half":
                    transform.GetChild(_Index).GetComponent<SpriteRenderer>().color = _Half_Trans;
                    break;
                case "Full":
                    transform.GetChild(_Index).GetComponent<SpriteRenderer>().color = _Full_Trans;
                    break;
            }
            yield return new WaitForSeconds(0.05f);
            _Index++;
            if(_Index >= transform.childCount)
            {
                yield return new WaitForSeconds(0.5f);
                _Index = 0;
                switch (_Color_Now)
                {
                    case "Half":
                        _Color_Now = "Full";
                        break;
                    case "Full":
                        _Color_Now = "Half";
                        break;  
                }
                Show_Time++;
            }
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            _Point = transform.GetChild(i).gameObject;
            _Point.transform.SetParent(null);
            MM.Return_Point(_Point);
        }
        Is_Showing = false;
        Is_Preparing = 0;
    }
}
