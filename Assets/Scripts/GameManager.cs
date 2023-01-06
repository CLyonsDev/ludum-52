using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject HumanPrefab;
    public GameObject MaxPrefab;

    [Header("Game Init Settings")]
    [SerializeField] private int startingHumans;
    [SerializeField] private Vector3 mapCenter = new Vector3(25, 1, 25);
    [SerializeField] private int roomSize = 10;

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
    }

    private void CreateHuman()
    {
        Vector3 spawnPoint = Vector3.zero;
        
        while(spawnPoint == Vector3.zero) {
            Vector3 randPoint = mapCenter + Random.insideUnitSphere * roomSize;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                spawnPoint = hit.position;
            }
        }

        Quaternion randRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        GameObject newHuman = Instantiate(HumanPrefab, spawnPoint, randRotation);
    }
}
