using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Round_BroadCast : MonoBehaviour
{
    public Text Round_BroadCast_Text;
    public Image Round_BroadCast_Image;
    PhotonView photonView;
    List<string> Round_BroadCast_Queue = new List<string>();

    bool Is_Busy = false;
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            Master_Round_BroadCast_Type("Round 1");
            Master_Round_BroadCast_Type("Devil Retruns.");
        }
        */
        if (!Is_Busy && Round_BroadCast_Queue.Count > 0)
        {
            StartCoroutine(Appear(Round_BroadCast_Queue[0]));
            Round_BroadCast_Queue.RemoveAt(0);
        }
    }

    [PunRPC]
    public void Master_Round_BroadCast_Type(string _Content)
    {
        photonView.RPC("Round_BroadCast_Type", RpcTarget.All, _Content);
    }

    [PunRPC]
    public void Round_BroadCast_Type(string _Content)
    {
        Round_BroadCast_Queue.Add(_Content);
    }

    public IEnumerator Appear(string _String)
    {
        Is_Busy = true;

        while(Round_BroadCast_Image.fillAmount < 1)
        {
            Round_BroadCast_Image.fillOrigin = 0;
            Round_BroadCast_Image.fillAmount += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        Round_BroadCast_Text.color = Color.black;
        for (int i = 1; i <= _String.Length; i++)
        {
            Round_BroadCast_Text.text = _String.Substring(0, i) + "<color=#" + ColorUtility.ToHtmlStringRGBA(Color.clear) + ">" + _String.Substring(i, _String.Length - i) + "</color>";
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(_String));
    }


    IEnumerator Fade(string _String)
    {
        Is_Busy = true;

        Round_BroadCast_Text.color = Color.black;
        for (int i = 1; i <= _String.Length; i++)
        {
            Round_BroadCast_Text.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(Color.clear) + ">" + _String.Substring(0, i) + "</color>" + _String.Substring(i, _String.Length - i);
            yield return new WaitForSeconds(0.1f);
        }

        Round_BroadCast_Image.fillOrigin = 1;
        while (Round_BroadCast_Image.fillAmount > 0)
        {
            Round_BroadCast_Image.fillAmount -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        Is_Busy = false;
    }
}
