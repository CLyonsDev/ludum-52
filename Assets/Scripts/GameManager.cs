using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject HumanPrefab;
    public GameObject MaxPrefab;
    public List<GameObject> SpawnedHumans = new List<GameObject>();

    [Header("Game Init Settings")]
    [SerializeField] private int startingHumans;
    [SerializeField] private Vector3 mapCenter = new Vector3(25, 1, 25);
    [SerializeField] private int roomSize = 10;

    public static GameManager _Instance;
    public GameEvent UpdateMoneyGameEvent;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeGame()
    {
        for (int i = 0; i < startingHumans; i++)
        {
            CreateHuman();
        }

        UpdateMoneyGameEvent.Raise();
    }

    public void PurchaseHuman(int price) {
        if(MoneyManager._Instance.RemoveMoney(price))
        {
            CreateHuman();
        }else{
            Debug.LogWarning("Not enough money to purchase human");
        }
    }

    private void CreateHuman()
    {
        Vector3 spawnPoint = Vector3.zero;

        while (spawnPoint == Vector3.zero)
        {
            Vector3 randPoint = mapCenter + Random.insideUnitSphere * roomSize;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                spawnPoint = hit.position;
            }
        }

        Quaternion randRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject newHuman = Instantiate(HumanPrefab, spawnPoint, randRotation);

        SpawnedHumans.Add(newHuman);
    }

    public void RemoveHuman(GameObject humanToRemove)
    {
        SpawnedHumans.Remove(humanToRemove);
        Destroy(humanToRemove);
    }

    public Transform GetClosestHuman(Vector3 currentPos)
    {
        GameObject closest = null;
        float minDst = Mathf.Infinity;
        foreach (GameObject g in SpawnedHumans)
        {
            float dst = Vector3.Distance(g.transform.position, currentPos);
            if (dst < minDst)
            {
                closest = g;
                minDst = dst;
            }
        }

        return closest.transform;
    }
}
