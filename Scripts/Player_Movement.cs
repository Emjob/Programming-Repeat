using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    private NavMeshAgent Player = null;

    HUDBehaviour NewPosition;

    Animator animator;

    RaycastHit hit;

    public Vector3 Destination;

    public Vector3 prevPos;
    public Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        NewPosition = GameObject.FindWithTag("Scene_Manager").GetComponent<HUDBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.x == Destination.x)
        {
            if (transform.position.z == Destination.z)
            {
                animator.SetBool("Walk", false);
            }
        }
    }
    public void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                Player.SetDestination(hit.point);
                Destination = hit.point;
                animator.SetBool("Walk", true);
                gameObject.GetComponent<Player_Values>().Player_Health -= 1;
                NewPosition.doMove = false;
               
            }
        }
        
    }

  
}
