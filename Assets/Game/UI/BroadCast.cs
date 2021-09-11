using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BroadCast : MonoBehaviour
{
    public Text BroadCast_Text;
    PhotonView photonView;
    List<string> BroadCast_Queue = new List<string>();

    bool Is_Busy = false;
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    void Update()
    {
        if (!Is_Busy && BroadCast_Queue.Count > 0)
        {
            StartCoroutine(Type(BroadCast_Queue[0]));
            BroadCast_Queue.RemoveAt(0);
        }
    }

    [PunRPC]
    public void Master_BroadCast_Type(string _Content)
    {
        photonView.RPC("BroadCast_Type", RpcTarget.All, _Content);
    }

    [PunRPC]
    public void BroadCast_Type(string _Content)
    {
        BroadCast_Queue.Add(_Content);
    }

    public IEnumerator Type(string _String)
    {
        Is_Busy = true;
        BroadCast_Text.color = Color.black;
        for (int i = 1; i <= _String.Length; i++)
        {
            BroadCast_Text.text = _String.Substring(0, i) + "<color=#" + ColorUtility.ToHtmlStringRGBA(Color.clear) + ">" + _String.Substring(i, _String.Length - i) + "</color>";
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Fade());
    }


    IEnumerator Fade()
    {
        while(BroadCast_Text.color.a > 0)
        {
            BroadCast_Text.color = BroadCast_Text.color - Color.black * 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        Is_Busy = false;
    }
}
