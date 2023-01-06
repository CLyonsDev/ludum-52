using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    private NavMeshAgent agent;

    [Header("Idle Behavior")]
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 4.5f;
    [SerializeField] private bool isIdling = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PickNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance && isIdling == false)
        {
            isIdling = true;
            Invoke("PickNextWaypoint", Random.Range(minIdleTime, maxIdleTime));
        }
    }



    private void PickNextWaypoint()
    {
        Vector3 newDestination = Vector3.zero;

        while(newDestination == Vector3.zero)
        {
            Vector3 randpt = transform.position + Random.insideUnitSphere * 5;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randpt, out hit, 1.0f, NavMesh.AllAreas))
            {
                newDestination = hit.position;
            }
        }

        agent.SetDestination(newDestination);
        isIdling = false;
    }
}
