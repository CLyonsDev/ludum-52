using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
    public GameEvent GameOverEvent;
    public GameEvent GameWinEvent;
    public IntVariable HumansToWin;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] humanSpawnSounds;
    [SerializeField] private AudioClip victorySting;

    public static GameManager _Instance;
    public GameEvent UpdateMoneyGameEvent;

    [Header("Enemy Settings")]
    private float enemySpawnDelaySeconds = 120; // The enemy spawns every X amount of seconds
    private bool readyToSpawnEnemy = true;
    [SerializeField] private GameObject warningUiElement;

    [SerializeField] private bool isGameInitialized = false;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToSpawnEnemy && isGameInitialized)
        {
            readyToSpawnEnemy = false;
            Invoke("SpawnEnemyWarning", enemySpawnDelaySeconds - 4.5f);
            Invoke("SpawnEnemy", enemySpawnDelaySeconds);
        }

        if (isGameInitialized && SpawnedHumans.Count == 0)
        {
            Debug.LogWarning("Game over!");
            GameOverEvent.Raise();
        }

        if (isGameInitialized && SpawnedHumans.Count >= HumansToWin.Value)
        {
            WinGame();
        }
    }

    public void InitializeGame()
    {
        for (int i = 0; i < startingHumans; i++)
        {
            CreateHuman();
        }

        isGameInitialized = true;

        UpdateMoneyGameEvent.Raise();
    }

    public void PurchaseHuman(int price)
    {
        if (MoneyManager._Instance.RemoveMoney(price))
        {
            CreateHuman();
        }
        else
        {
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

        AudioClip spawnClip = humanSpawnSounds[Random.Range(0, humanSpawnSounds.Length)];
        AudioManager._Instance.CreateSoundAtPoint(spawnClip, spawnPoint, 0.01f);

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

    public void OnEnemyDeath()
    {
        readyToSpawnEnemy = true;
    }

    private void SpawnEnemyWarning()
    {
        warningUiElement.SetActive(true);
        warningUiElement.GetComponentInChildren<Animator>().SetTrigger("PlayAlert");
        Invoke("DeactivateWarningAfterAnimation", 5f);
        AudioManager._Instance.CreateSoundGlobal(AudioManager._Instance.UiAlertSound, 0.055f);
        AudioManager._Instance.CalmBgmGo.GetComponent<BgmManager>().FadeOut();
    }

    private void DeactivateWarningAfterAnimation()
    {
        warningUiElement.SetActive(false);
    }

    private void SpawnEnemy()
    {
        AudioManager._Instance.CombatBgmGo.GetComponent<AudioSource>().Play();
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
        GameObject newMax = Instantiate(MaxPrefab, spawnPoint, randRotation);

        AudioClip spawnClip = AudioManager._Instance.MaxArriveSound;
        //AudioManager._Instance.CreateSoundAtPoint(spawnClip, spawnPoint, 0.03f);
        newMax.GetComponent<AudioSource>().clip = spawnClip;
        newMax.GetComponent<AudioSource>().Play();
        AudioManager._Instance.CreateSoundAtPoint(AudioManager._Instance.MaxTeleportInSound, spawnPoint, 0.02f);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void WinGame()
    {
        GameWinEvent.Raise();
        isGameInitialized = false;

        for (int i = SpawnedHumans.Count-1; i >= 0; i--)
        {
            RemoveHuman(SpawnedHumans[i]);
        }

        AudioManager._Instance.CalmBgmGo.GetComponent<BgmManager>().FadeOut();
        AudioManager._Instance.CombatBgmGo.GetComponent<BgmManager>().FadeOut();
        AudioManager._Instance.CreateSoundGlobal(victorySting, 0.05f);
        AudioManager._Instance.CreateSoundGlobal(AudioManager._Instance.HarvestAllHumansSound, 0.04f);
    }
}
