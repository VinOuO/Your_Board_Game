using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Effect_Mouse : MonoBehaviour
{
    Camera UI_Camera;
    public Vector3 Target_Pos, Dir, Pos_Now, Pos_Before;
    bool Stop_Move;
    float Speed = 1;
    string State;
    public string Name = "Fairy";
    void Start()
    {
        Stop_Move = false;
        StartCoroutine(Move_Softly());
        StartCoroutine(Clock());
        State = "Fallow";
        Dir = Vector3.zero;
    }

    void Update()
    {
        if (UI_Camera == null)
        {
            UI_Camera = GameObject.Find("UI_LiKe_Looking_Camera").GetComponent<Camera>();
        }

        Target_Pos = UI_Camera.ScreenToWorldPoint(Input.mousePosition);
        Target_Pos.z = 0;
        Mouse_On();
        Measure();

        if (!GetComponent<Fairy>().Is_Speaking && Speed == 0.3f)
        {
            Speed = 1;
        }
        else if (GetComponent<Fairy>().Is_Speaking && Speed == 1)
        {
            Speed = 0.3f;
        }

        Move(Target_Pos);
    }

    void Measure()
    {
        Pos_Before = Pos_Now;
        Pos_Now = transform.position;
        Dir = (Pos_Now - Pos_Before).normalized;
    }

    void Move(Vector3 _Target_Pos)
    {
        switch (State)
        {
            case "Fallow":
                if (Vector3.Distance(_Target_Pos, transform.position) >= 10)
                {
                    transform.Translate((_Target_Pos - transform.position).normalized * 3f * Time.deltaTime * Speed, Space.World);
                    Turn_Around_Softly(Angle, 25 * Speed);
                }
                else
                {
                    switch (State_1)
                    {
                        case 0:
                            Turn_Around_Softly(Angle, 18 * Speed);
                            break;
                        case 1:
                            Float();
                            break;
                    }
                }
                break;
            case "Leave":
                transform.Translate(-(_Target_Pos - transform.position).normalized * 3f * Time.deltaTime * Speed, Space.World);
                Turn_Around_Softly(Angle, 25);
                break;
        }

        
        
    }
    
    void Mouse_On()
    {
        //---------------------------------------Node_Info
        RaycastHit2D hit;
        hit = Physics2D.Raycast(UI_Camera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
        if (true)
        {
            if (hit.collider != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    State = "Leave";
                    Timer_3 = 1f;
                }
            } 
        }
        //---------------------------------------Node_Info
    }

    void Float()
    {
        switch (State_2)
        {
            case 0:
                transform.Translate(Vector3.up.normalized * 2f * Time.deltaTime, Space.World);
                break;
            case 1:
                transform.Translate(Vector3.down.normalized * 2f * Time.deltaTime, Space.World);
                break;
        }
        
    }

    void Turn_Around_Softly(float _Angle, float _Speed)
    {
        transform.Translate(Set_Dir(_Angle) * Dir.normalized * _Speed * Time.deltaTime, Space.World);
    }

    Quaternion Set_Dir(float _Angle)
    {
        return Quaternion.Euler(0, 0, _Angle);
    }

    public float Angle = 0;
    public IEnumerator Move_Softly()
    {
        while (true)
        {
            Angle = Random.Range(-10f, 10f);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
        
    }

    float Timer_1;
    float Timer_2;
    float Timer_3;
    public int State_1;
    public int State_2;
    public IEnumerator Clock()
    {
        Timer_1 = Random.Range(0.5f, 2);
        State_1 = 0;
        Timer_2 = 0.2f;
        State_2 = 0;
        while (true)
        {
            if (Timer_1 <= 0)
            {
                State_1++;
                State_1 %= 2;
                Timer_1 = Random.Range(1f, 2);
            }
            Timer_1 -= 0.01f;

            if (Timer_2 <= 0)
            {
                State_2++;
                State_2 %= 2;
                Timer_2 = 0.2f;
            }
            Timer_2 -= 0.01f;

            if (State == "Leave")
            {
                Timer_3 -= 0.01f;
                if (Timer_3 <= 0)
                {
                    State = "Fallow";
                }
            }
            yield return new WaitForSeconds(0.01f);
        }

    }
}
