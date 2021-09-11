using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (GetComponent<SpriteRenderer>().color.a > 0)
        {
            GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color - Color.black * 0.0005f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
