using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviour
{
    Vector3 Dir;
    PhotonView photonView;
    float Speed = 100, Roll_Time = 5;
    public Sprite[] Dice_Pic;
    bool Need_To_Move = false, Is_Rolling = false, Is_Dragging = false;
    public GameObject[] Crack_Prefab;
    void Start()
    {
        photonView = PhotonView.Get(this);
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (!Is_Rolling)
        {
            Throll();
        }
        Move();
    }
    Vector3 Pos_Pre = Vector3.zero;
    Camera UI_Camera;
    void Throll()
    {
        if(UI_Camera == null)
        {
            UI_Camera = GameObject.Find("UI_LiKe_Looking_Camera").GetComponent<Camera>();
        }
        if (!Is_Dragging)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(UI_Camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == this.gameObject && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Is_Dragging = true;
                }
            }
        }
        else
        {
            Dir = UI_Camera.ScreenToWorldPoint(Input.mousePosition) - Pos_Pre;
            Pos_Pre = UI_Camera.ScreenToWorldPoint(Input.mousePosition);
            Dir.z = 0;
            Speed = Dir.magnitude * 10;
            transform.position = new Vector3(UI_Camera.ScreenToWorldPoint(Input.mousePosition).x, UI_Camera.ScreenToWorldPoint(Input.mousePosition).y, 0);
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Is_Dragging = false;
                StartCoroutine(Roll());
            }
        }

    }

    private void Move()
    {
        if (Need_To_Move)
        {
            transform.Translate(Dir.normalized * Speed * Time.deltaTime, Space.World);
        }
        
        if(UI_Camera.WorldToScreenPoint(transform.position).x > Screen.width || UI_Camera.WorldToScreenPoint(transform.position).x < 0)
        {
            Need_To_Move = false;
            if (UI_Camera.WorldToScreenPoint(transform.position).x > Screen.width)
            {
                if(Speed > 200 && Is_Rolling)
                {
                    Crack(transform.position, "Right");
                }
                transform.position += Vector3.left * 3;
            }
            else
            {
                if (Speed > 200 && Is_Rolling)
                {
                    Crack(transform.position, "Left");
                }
                transform.position += Vector3.right * 3;
            }
            Dir.x *= -1;
        }
        if (UI_Camera.WorldToScreenPoint(transform.position).y > Screen.height || UI_Camera.WorldToScreenPoint(transform.position).y < 0)
        {
            Need_To_Move = false;
            if (UI_Camera.WorldToScreenPoint(transform.position).y > Screen.height)
            {
                if (Speed > 200 && Is_Rolling)
                {
                    Crack(transform.position, "Top");
                }
                transform.position += Vector3.down * 3;
            }
            else
            {
                if (Speed > 200 && Is_Rolling)
                {
                    Crack(transform.position, "Down");
                }
                transform.position += Vector3.up * 3;
            }
            Dir.y *= -1;
        }
        Need_To_Move = true;
    }

    void Crack(Vector3 _Pos, string _Dir)
    {
        GameObject _temp;
        switch (_Dir)
        {
            case "Top":
                _temp = Instantiate(Crack_Prefab[1], _Pos, Crack_Prefab[1].transform.rotation);
                _temp.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case "Down":
                _temp = Instantiate(Crack_Prefab[1], _Pos, Crack_Prefab[1].transform.rotation);
                break;
            case "Left":
                _temp = Instantiate(Crack_Prefab[0], _Pos, Crack_Prefab[0].transform.rotation);
                break;
            case "Right":
                _temp = Instantiate(Crack_Prefab[0], _Pos, Crack_Prefab[0].transform.rotation);
                _temp.GetComponent<SpriteRenderer>().flipX = true;
                break;
        }
        
    }

    float Timer_1 = 0.05f;
    float Timer_2 = 10;
    IEnumerator Roll()
    {
        Need_To_Move = true;
        Is_Rolling = true;
        //Dir = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().speed = Speed / 30;
        float Move_Speed = Speed / (Roll_Time * 100);
        float Anim_Speed = GetComponent<Animator>().speed / (Roll_Time * 100);
        while (Speed > 0)
        {
            Speed -= Move_Speed;
            GetComponent<Animator>().speed -= Anim_Speed;
            yield return new WaitForSeconds(0.01f);
        }
        Speed = 0;
        GetComponent<Animator>().enabled = false;
        Is_Rolling = false;
        Need_To_Move = false;
        Result();
    }

    public int Roll_Result;
    void Result()
    {
        Roll_Result = Random.Range(1, 21);
        GetComponent<SpriteRenderer>().sprite = Dice_Pic[Roll_Result - 1];
        photonView.RPC("Send_Result", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer.NickName, Roll_Result);
    }

    [PunRPC]
    public void Send_Result(string _Figure_Name, int _Round_Step)
    {
        if (!GameObject.Find(_Figure_Name).GetComponent<Figure>().Condition_List[2])
        {
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Round_Step = _Round_Step;
            GameObject.Find(_Figure_Name).GetComponent<Figure>().Condition_List[2] = true;
        }
        
    }
}
