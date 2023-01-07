using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    private FoodManager foodManager;

    [Header("Movement Attributes")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    private NavMeshAgent agent;

    [Header("Idle Behavior")]
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 4.5f;
    [SerializeField] private bool isIdling = false;

    [Header("Behavior Tree")]
    [SerializeField] private bool isHungry = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foodManager = FoodManager._Instance;
        PickNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(foodManager == null)
            foodManager = FoodManager._Instance;

        if(isHungry && foodManager.PlacedFood.Count > 0)
        {
            agent.SetDestination(GetClosestFood().position);
        } else if(agent.remainingDistance <= agent.stoppingDistance && isIdling == false)
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

    public void StartSearchForFood()
    {
        isHungry = true;
    }

    public void StopSearchingForFood()
    {
        isHungry = false;
    }

    public Transform GetClosestFood()
    {
        Transform closest = null;
        float minDst = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in foodManager.PlacedFood)
        {
            float dst = Vector3.Distance(t.position, currentPos);
            if(dst < minDst)
            {
                closest = t;
                minDst = dst;
            }
        }

        return closest;
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log("Trigger Entered" + other.gameObject.tag);
        if(other.gameObject.CompareTag("Food") && isHungry) {
            // Eat food
            Debug.Log("Eat time");
            SendMessage("EatFood", other.gameObject.GetComponentInParent<FoodStats>().ReliefAmt);
            foodManager.RemoveFood(other.gameObject);
        }
    }
}
