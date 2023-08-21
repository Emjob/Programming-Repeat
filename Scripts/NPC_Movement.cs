using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    public NavMeshAgent agent;

    private bool endTurn = true;

    public float range; 

    public Vector3 Destination;


    private GameObject Player;
    private GameObject AllyTarget;

    Animator animator;

    HUDBehaviour Query;
    NPC_Values NPC_Stats;

    public Transform centrePoint; 
    

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Query = GameObject.FindWithTag("Scene_Manager").GetComponent<HUDBehaviour>();
        NPC_Stats = gameObject.GetComponent<NPC_Values>();
    }


    void Update()
    {
        if (transform.position.x == Destination.x)
        {
            if (transform.position.z == Destination.z)
            {
                animator.SetBool("Walk", false);
            }
        }
        
        if (!endTurn)
        {
            if (NPC_Stats.Status == "Enemy")
            {
                if (NPC_Stats.DistanceToPlayer < 10)
                {
                    Query.NPCdoDamage(Player);
                    endTurn = true;
                    Query.NPCMove_Counter += 1;
                }
                if(NPC_Stats.DistanceToPlayer >= 40)
                {
                    Move();
                    
                    Query.NPCMove_Counter += 1;
                }
                if (NPC_Stats.DistanceToPlayer >= 10 && NPC_Stats.DistanceToPlayer <= 40)
                {
                    enemyMove();
                    
                    Query.NPCMove_Counter += 1;
                }
            }
            if(NPC_Stats.Status == "Neutral")
            {
                Move();
                Query.NPCMove_Counter += 1;
            }
            if( NPC_Stats.Status == "Ally")
            {
                if (NPC_Stats.DistanceToPlayer >= 40)
                {
                    Move();

                    Query.NPCMove_Counter += 1;
                }
                if (NPC_Stats.DistanceToPlayer >= 10 && NPC_Stats.DistanceToPlayer <= 40)
                {
                    enemyMove();

                    Query.NPCMove_Counter += 1;
                }
                if (NPC_Stats.DistanceToPlayer < 10)
                {
                    Query.NPCdoDamage(Player);
                    endTurn = true;
                    Query.NPCMove_Counter += 1;
                }

            }
        }

    }

    public void StartTurn()
    {
        endTurn = false;

    }
    void Move()
    {
        

        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            centrePoint = GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                agent.SetDestination(point);
                Destination = point;
                animator.SetBool("Walk", true);
                endTurn = true;
            }
        }
    }
    void enemyMove()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            centrePoint = Player.GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                agent.SetDestination(point);
                Destination = point;
                animator.SetBool("Walk", true);
                endTurn = true;
            }
        }
    }
    void AllyMove()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            centrePoint = Player.GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                agent.SetDestination(point);
                Destination = point;
                animator.SetBool("Walk", true);
                endTurn = true;
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

}
