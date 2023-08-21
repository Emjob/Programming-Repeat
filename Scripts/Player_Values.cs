using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Values : MonoBehaviour
{
    public int Player_Health;
    public int Ammo;

    public Vector3 PlayerPosition;

    public Transform PlayerTransform;

    public string Position;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        Player_Health = 10;
        Ammo = 50;

        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTransform = transform;
        PlayerPosition = transform.position;
        float x = PlayerPosition.x;
        float z = PlayerPosition.z;
        Position = (x.ToString()) + ", " + (z.ToString());
        
        gameObject.name = name;

        if(Player_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ReadString(string text)
    {
        name = text;
        gameObject.name = name;
    }

    

}
