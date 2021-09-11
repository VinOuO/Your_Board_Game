using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fairy : MonoBehaviour
{

    public bool Is_Speaking = false;
    public Text Fairy_Text;
    Color Original_Color;
    List<string> Line_Queue = new List<string>();

    void Start()
    {
        Original_Color = Fairy_Text.color;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //Ask_To_Speak("Every inch of me is trembling But not from the cold Something is familiar Like a dream I can reach but not quite hold I can sense you there Like a friend I've always known I'm arriving And it feels like I am home I have always been a fortress Cold secrets deep inside You have secrets, too But you don't have to hide Show yourself I'm dying to meet you Show yourself It's your turn Are you the one I've been looking for All of my life? Show yourself I'm ready to learn Ah ah ah ah Ah ah ah ah ah I've never felt so certain All my life I've been torn But I'm here for a reason Could it be the reason I was born? I have always been so different Normal rules did not apply Is this the day? Are you the way I finally find out why? Show yourself I'm no longer trembling Here I am I've come so far You are the answer I've waited for All of my life Oh, show yourself Let me see who you are Come to me now Open your door Don't make me wait One moment more Oh, come to me now Open your door Don't make me wait One moment more Where the north wind meets the sea Ah ah ah ah There's a river Ah ah ah ah Full of memory Come, my darling, homeward bound I am found Show yourself Step into your power Throw yourself Into something new You are the one you've been waiting for All of my life All of your life Oh, show yourself Ah ah ah Ah ah ah Ah ah ah");
        }

        if (!Is_Speaking && Line_Queue.Count > 0)
        {
            StartCoroutine(Speak(Line_Queue[0]));
            Line_Queue.RemoveAt(0);
        }
    }
    
    public void Ask_To_Speak(string _String)
    {
        if(!Is_Speaking)
        {
            List<string> _Lines = new List<string>(_String.Split(' '));
            string _temp = "";
            int _Index = 0;
            for (int i = 0; i < _Lines.Count; i++)
            {
                _Index++;
                _temp += _Lines[i] + " ";
                if (_Index >= 9)
                {
                    _Index = 0;
                    Line_Queue.Add(_temp);
                    _temp = "";
                }
                else if (i == _Lines.Count - 1)
                {
                    Line_Queue.Add(_temp);
                }
            }
        }
        
    }

    public IEnumerator Speak(string _String)
    {
        Is_Speaking = true;
        Fairy_Text.color = Original_Color;
        for (int i = 1; i <= _String.Length; i++)
        {
            Fairy_Text.text = _String.Substring(0, i) + "<color=#" + ColorUtility.ToHtmlStringRGBA(Color.clear) + ">" + _String.Substring(i, _String.Length - i) + "</color>";
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (Fairy_Text.color.a > 0)
        {
            Fairy_Text.color = Fairy_Text.color - Color.black * 0.1f;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(3f);
        Is_Speaking = false;
    }
}
