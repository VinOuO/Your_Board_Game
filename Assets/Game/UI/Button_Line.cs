using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_Line : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool Pointer_On = false;
    void Start()
    {
        StartCoroutine(Button_Line_Fill_Fade());
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Pointer_On = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pointer_On = false;
    }

    IEnumerator Button_Line_Fill_Fade()
    {
        while (true)
        {
            if (Pointer_On && GetComponent<Image>().fillAmount < 1)
            {
                GetComponent<Image>().fillAmount += 0.2f;
            }
            else if (!Pointer_On && GetComponent<Image>().fillAmount > 0)
            {
                GetComponent<Image>().fillAmount -= 0.2f;
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

}
