using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Movement : MonoBehaviour
{
    public NavMeshAgent agent;

    private bool endTurn = true;

    public float range; //radius of sphere

    public Vector3 Destination;
//    private Vector3 point;

    private GameObject Player;
    private GameObject AllyTarget;

    Animator animator;

    HUDBehaviour Query;
    NPC_Values NPC_Stats;

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

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
        

        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            centrePoint = GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
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
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            centrePoint = Player.GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
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
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            centrePoint = Player.GetComponent<Transform>();
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
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

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) 
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

}
