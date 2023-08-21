using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{

    GameObject door1;
    GameObject door2;
    GameObject door3;

    public bool open1;
    public bool open2;
    public bool open3;
    // Start is called before the first frame update
    void Start()
    {
        door1 = GameObject.FindWithTag("Door1");
        door2 = GameObject.FindWithTag("Door2");
        door3 = GameObject.FindWithTag("Door3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Button1"))
        {
            if (open1 == true)
            {
                open1 = false;
                door1.transform.Translate(0,-20, 0);
                
            }
            if (open1 == false)
            {
                door1.transform.Translate(0, 10, 0);
                open1 = true;
            }
        }
        if (other.gameObject.CompareTag("Button2"))
        {
            if (open2 == true)
            {
                open2 = false;
                door2.transform.Translate(0,-20,0);
                
            }
            if (open2 == false)
            {
                door2.transform.Translate(0, 10, 0);
                open2 = true;
            }

        }
        if (other.gameObject.CompareTag("Button3"))
        {
            if (open3 == true)
            {
                open3 = false;
                door3.transform.Translate(0, -20, 0);
                
            }
            if (open3 == false)
            {
                door3.transform.Translate(0, 10, 0);
                open3 = true;
            }
        }
    }
}
