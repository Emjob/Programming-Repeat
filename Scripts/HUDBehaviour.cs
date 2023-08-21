using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HUDBehaviour : MonoBehaviour
{
    public GameObject NPC;
    public GameObject Player;
    public GameObject target;

    public GameObject[] NPCs;

    private Camera camera;

    public int NPCMove_Counter;

    private int NPC_Count;
    private int Player_Count;
    public int Move_Counter;

    private float PlayerX;
    private float PlayerZ;

    private string PlayerString;
    private string NPCString;

    public string[] nameArray;

    private bool doAsk;
    private bool End;
    private bool doShoot;
    private bool doAlly;

    public bool StartTurn;
    public bool doMove;

    public AudioSource Gunshot;

    Player_Movement Player_Move;
    Player_Values Player_Stats;

    public TextMeshProUGUI Player_Status;
    public TextMeshProUGUI NPC_Status;
    public TextMeshProUGUI AllNPC_Status;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Player_Stats = GameObject.FindWithTag("Player").GetComponent<Player_Values>();
        Player_Move = GameObject.FindWithTag("Player").GetComponent<Player_Movement>();

        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        string PlayerHealthString = "Health: " + Player_Stats.Player_Health;
        string PlayerAmmoString = "Ammo: " + Player_Stats.Ammo;
        string PlayerPositionString = "Position: " + Player_Stats.Position;
        PlayerString = (PlayerHealthString + '\n' + PlayerAmmoString + '\n' + PlayerPositionString);

        if (doAlly)
        {
            DetectObjectWithRaycast();
            if (target.GetComponent<NPC_Values>().Status == "Neutral")
            {
                target.GetComponent<NPC_Values>().Status = "Ally";
                Move_Counter++;
            }
        }
        if (doMove)
        {
            Player_Move.Move();
        }
        if (doAsk)
        {
            DetectObjectWithRaycast();
            target.GetComponent<NPC_Values>().Info();
        }
        if (doShoot)
        {
            DetectObjectWithRaycast();
            doDamage();
            Player_Stats.Ammo -= 1;
        }
        if (Move_Counter >= 2)
        {

            EndingTurn();

        }
        if (StartTurn)
        {
            Move_Counter = 0;
            StartTurn = false;
        }

    }

    public void SpawnNPC()
    {
        if (NPC_Count < 5)
        {
            Instantiate(NPC, new Vector3(Random.Range(0, 100), 1, Random.Range(0, 100)), Quaternion.identity);
            NPC_Count += 1;
        }
    }
    public void SpawnPlayer()
    {
        if (Player_Count < 1)
        {
            Instantiate(Player, new Vector3(Random.Range(0, 100), 1, Random.Range(0, 100)), Quaternion.identity);
            Player_Count += 1;
        }
    }

    public void PlayerStatus()
    {
        Player_Status.GetComponent<TextMeshProUGUI>().text = PlayerString;

        //Player_Status.GetComponent<TextMeshProUGUI>().text = "Ammo:" + Player_Stats.Ammo;
        // Player_Status.GetComponent<TextMeshProUGUI>().text = "Position:" + Player_Stats.Position;
    }

    public void PlayerMove()
    {
        doMove = true;
        Move_Counter++;
    }

    public void AskNPC()
    {
        doAsk = true;
        Move_Counter++;
    }

    public void AskAll()
    {
        int y = 0;
        End = false;
        if (y < NPCs.Length && End == false)
        {
            NPCString = string.Empty;
            for (int i = 0; i < NPCs.Length; i++)
            {
                NPCString += '\n' + NPCs[i].GetComponent<NPC_Values>().name + " " + NPCs[i].GetComponent<NPC_Values>().Status + " " + NPCs[i].GetComponent<NPC_Values>().DistanceToPlayer;
                y++;
            }
            AllNPC_Status.GetComponent<TextMeshProUGUI>().text = NPCString;
            Move_Counter++;
            End = true;
        }
    }

    public void Shoot()
    {
        doShoot = true;
    }

    public void Alliance()
    {
        doAlly = true;

    }

    public void doDamage()
    {
        if (target.GetComponent<NPC_Values>().DistanceToPlayer < 50)
        {
            if (target.GetComponent<NPC_Values>().Status == "Enemy")
            {
                target.GetComponent<NPC_Values>().NPC_Health -= 1;
                Gunshot.Play();
                Move_Counter++;
            }
            if (target.GetComponent<NPC_Values>().Status == "Ally" || target.GetComponent<NPC_Values>().Status == "Neutral")
            {
                Player_Stats.Player_Health -= 2;
                Gunshot.Play();
                Move_Counter++;
            }

        }

    }
    public void NPCdoDamage(GameObject point)
    {
        if (point.CompareTag("NPC"))
        {
            point.GetComponent<NPC_Values>().NPC_Health -= 1;
            Gunshot.Play();
        }
        if (point.CompareTag("Player"))
        {
            point.GetComponent<Player_Values>().Player_Health -= 1;
            Gunshot.Play();
        }
    }

    public void NPCHeal(GameObject point)
    {
        if(point.CompareTag("Player"))
        {
            point.GetComponent<Player_Values>().Player_Health += 1;
            Gunshot.Play();
        }
    }

    void EndingTurn()
    {
       for(int i = 0; i < NPCs.Length; i++)
        {
            NPCs[i].GetComponent<NPC_Movement>().StartTurn();
        }
        StartTurn = true;   
    }

    public void DetectObjectWithRaycast()
    {
        target = null;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"{hit.collider.name} Detected",
                    hit.collider.gameObject);
                if(hit.collider.CompareTag("NPC"))
                {
                    target = hit.collider.gameObject;
                    doAsk = false;
                    doShoot = false;
                    doAlly = false;
                }
            }

        }
    }
    public void ReadString(string text)
    {
        name = text;
        Player_Stats.name = name;
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("SavedHealth", Player_Stats.Player_Health);
        PlayerPrefs.SetInt("SavedAmmo", Player_Stats.Ammo);
        PlayerPrefs.SetString("SavedName", Player_Stats.name);

    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedHealth"))
        {
            Player_Stats.Player_Health = PlayerPrefs.GetInt("SavedHealth");
            Player_Stats.Ammo = PlayerPrefs.GetInt("SavedAmmo");
            Player_Stats.name = PlayerPrefs.GetString("SavedName");
            Debug.Log("Game data loaded!");
        }
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        Player_Stats.Player_Health = 10;
        Player_Stats.Ammo = 50;
        Player_Stats.name = "";
        Debug.Log("Data reset complete");
    }
}
