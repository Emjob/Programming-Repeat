using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class NPC_Values : MonoBehaviour
{
    public int NPC_Health;
    private int x;
    private int close;

    public string Status;
    public string name;

    public Vector3 position;

    public float DistanceToPlayer;
    public float[] DistanceToEnemy;

    public TextMeshProUGUI NPCInfo;

    HUDBehaviour Query;
    Player_Values player;

    // Start is called before the first frame update
    void Start()
    {
        player= GameObject.FindWithTag("Player").GetComponent<Player_Values>();
        
        position = transform.position;
        Query = GameObject.FindWithTag("Scene_Manager").GetComponent<HUDBehaviour>();
    }
    private void Awake()
    {
        x = Random.Range(0, 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        NPCInfo = Query.NPC_Status;
        DistanceToPlayer = Vector3.Distance(player.PlayerTransform.position, transform.position);
        
        if (close == 0)
        {
            if (x == 0)
            {
                Status = "Neutral";
                Name();
                close += 1;
            }
            if (x == 1)
            {
                Status = "Enemy";
                Name();
                close += 1;
            }
        }
       
        

        if (NPC_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    

    public void Info()
    {
        NPCInfo.GetComponent<TextMeshProUGUI>().text = Status;
    }

    public void Name()
    {
        name = Query.nameArray[Random.Range(0, Query.nameArray.Length)];
        gameObject.name = name;
    }
}
