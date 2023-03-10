using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    private FoodManager foodManager;
    private AudioManager audioManager;

    private Animator anim;

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

    [SerializeField]
    private float speed;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float minBarkDelay = 12f;
    private float maxBarkDelay = 24f;
    private bool canBark = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        foodManager = FoodManager._Instance;
        audioManager = AudioManager._Instance;
        PickNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (foodManager == null)
            foodManager = FoodManager._Instance;

        if (isHungry && foodManager.PlacedFood.Count > 0)
        {
            agent.SetDestination(GetClosestFood().position);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance + 0.1f && isIdling == false)
        {
            isIdling = true;
            //anim.SetBool("walking", false);
            Invoke("PickNextWaypoint", Random.Range(minIdleTime, maxIdleTime));
        }else if (!isHungry && !isIdling)
        {
            stuckTimer += Time.deltaTime;
            if(stuckTimer >= 7f)
            {
                PickNextWaypoint();
            }
        }

        if(canBark)
        {
            canBark = false;
            Invoke("PlayBark", Random.Range(minBarkDelay, maxBarkDelay));
        }

        //Debug.Log(speed);
        if (speed <= 0.01f)
            anim.SetBool("walking", false);
        else
            anim.SetBool("walking", true);
    }

    private void FixedUpdate() {
        //speed = Mathf.Lerp(speed, (transform.position - lastPosition).magnitude, 0.7f);
        speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    private void PickNextWaypoint()
    {
        Vector3 newDestination = Vector3.zero;

        while (newDestination == Vector3.zero)
        {
            Vector3 randpt = transform.position + Random.insideUnitSphere * 5;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randpt, out hit, 1.0f, NavMesh.AllAreas))
            {
                newDestination = hit.position;
            }
        }

        agent.SetDestination(newDestination);
        stuckTimer = 0f;
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
            if (dst < minDst)
            {
                closest = t;
                minDst = dst;
            }
        }

        return closest;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger Entered" + other.gameObject.tag);
        if (other.gameObject.CompareTag("Food") && isHungry)
        {
            // Eat food
            Debug.Log("Eat time");
            SendMessage("EatFood", other.gameObject.GetComponentInParent<FoodStats>().ReliefAmt);
            foodManager.RemoveFood(other.gameObject);

            audioManager.CreateSoundAtPoint(audioManager.RandomSoundFromArray(audioManager.EatSounds), transform.position, 0.07f);
        }
    }

    private void PlayBark()
    {
        if(Random.Range(0, 100) > 35)
            return;

        AudioClip barkClip = audioManager.RandomSoundFromArray(audioManager.HumanBarks);
        audioManager.CreateSoundAtPoint(barkClip, transform.position, 0.04f);
        canBark = true;
    }
}
