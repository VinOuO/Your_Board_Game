using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Sprite : MonoBehaviour
{
    Particle_Effect_Mouse PEM;
    Vector3 L, R;
    void Start()
    {
        PEM = transform.parent.GetComponent<Particle_Effect_Mouse>();
        L = new Vector3(-0.5f, 0.5f, 0);
        R = new Vector3(0.5f, 0.5f, 0);
        StartCoroutine(Grow());
    }

    void Update()
    {
        if(PEM.Dir.x >= 0 && transform.GetChild(0).localScale.x < 0)
        {
            transform.GetChild(0).localScale = R;
        }
        else if (PEM.Dir.x < 0 && transform.localScale.x > 0)
        {
            transform.GetChild(0).localScale = L;
        }
    }

    public IEnumerator Grow()
    {
        bool Adding = false;
        while (true)
        {
            if (Adding)
            {
                GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.01f);
            }
            else
            {
                GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.01f);
            }
            if(GetComponent<SpriteRenderer>().color.a >= 0.8f)
            {
                Adding = false;
            }
            else if (GetComponent<SpriteRenderer>().color.a < 0.1f)
            {
                Adding = true;
            }
            yield return new WaitForSeconds(0.01f);
        }

    }
}
