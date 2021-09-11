using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map_stuff;
using Mob_stuff;

public class Local_Map_Manager : MonoBehaviour
{
    public GameObject Point_Pre;
    public List<GameObject> Points = new List<GameObject>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }

    public IEnumerator Inis_Point(int _Amount)
    {
        GameObject _Point;

        while (_Amount > 0)
        {
            _Point = Instantiate(Point_Pre);
            Points.Add(_Point);
            _Amount--;
            yield return new WaitForSeconds(0.005f);
        }
    }

    public GameObject Take_Point()
    {
        GameObject _Point = Points[0];
        Points.RemoveAt(0);
        Debug.Log("Point Count: " + Points.Count);
        if (Points.Count < 50)
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
}



