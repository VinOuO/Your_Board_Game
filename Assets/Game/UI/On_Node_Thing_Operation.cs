using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class On_Node_Thing_Operation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    void Start()
    {
        Hide();
    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.GetComponent<On_Node_Thing>().PointerOn++;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (this.name)
        {
            case "Cast":
                transform.parent.GetComponent<On_Node_Thing>().Do_Operation_Cast();
                break;
            case "Pick":
                transform.parent.GetComponent<On_Node_Thing>().Do_Operation_Pick();
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(PointerLeave());
    }

    IEnumerator PointerLeave()
    {
        yield return new WaitForSeconds(0.25f);
        transform.parent.GetComponent<On_Node_Thing>().PointerOn--;
    }
    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}