using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    int Offset = Screen.height / 6;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            Move();
            Zoom();
        }
        
    }

    void Move()
    {
        if(Input.mousePosition.x < Offset)
        {
            transform.Translate(Vector3.left * GetComponent<Camera>().orthographicSize * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.x >= Screen.width - Offset)
        {
            transform.Translate(Vector3.right * GetComponent<Camera>().orthographicSize * Time.deltaTime, Space.World);
        }

        if (Input.mousePosition.y < Offset)
        {
            transform.Translate(Vector3.down * GetComponent<Camera>().orthographicSize * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.y > Screen.height - Offset)
        {
            transform.Translate(Vector3.up * GetComponent<Camera>().orthographicSize * Time.deltaTime, Space.World);
        }
    }

    void Zoom()
    {
        if (Input.mouseScrollDelta.y < 0 && GetComponent<Camera>().orthographicSize < 80)
        {
            GetComponent<Camera>().orthographicSize += 5;
        }
        else if (Input.mouseScrollDelta.y > 0 && GetComponent<Camera>().orthographicSize > 5)
        {
            GetComponent<Camera>().orthographicSize -= 5;
        }
        
    }
}
