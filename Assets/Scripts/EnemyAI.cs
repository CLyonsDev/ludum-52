using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameManager gameManager;
    private float thinkRate = 1f;    // Runs target-gathering algorithm every X seconds.
    [SerializeField] private int ClicksToDefeat = 20;
    [SerializeField] private int CurrentClickCount = 0;
    [SerializeField] private Transform currentTarget;

    private AudioManager audioManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager._Instance.CreateSoundAtPoint(AudioManager._Instance.MaxArriveSound, transform.position, 0.055f);
        gameManager = GameManager._Instance;
        audioManager = AudioManager._Instance;
        //StartCoroutine(HumanAiLogic());
    }

    // Update is called once per frame
    void Update()
    {
        currentTarget = GetClosestHuman();

        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);

            if (Vector3.Distance(transform.position, currentTarget.position) <= agent.stoppingDistance)
            {
                AudioManager._Instance.CreateSoundAtPoint(AudioManager._Instance.HumanTeleportedAwaySound, transform.position, 0.045f);
                gameManager.RemoveHuman(currentTarget.gameObject);
            }
        }
        else
        {
            agent.SetDestination(agent.transform.position);
        }
    }

    private IEnumerator HumanAiLogic()
    {
        while (true)
        {
            //yield return new WaitForSeconds(thinkRate);
            yield return new WaitForEndOfFrame();
        }
    }

    public Transform GetClosestHuman()
    {
        if (gameManager.SpawnedHumans.Count == 0)
            return null;

        Transform closest = null;
        float minDst = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject go in gameManager.SpawnedHumans)
        {
            float dst = Vector3.Distance(go.transform.position, currentPos);
            if (dst < minDst)
            {
                closest = go.transform;
                minDst = dst;
            }
        }

        return closest;
    }

    public void TakeDamage()
    {
        CurrentClickCount++;

        AudioClip painClip = audioManager.RandomSoundFromArray(audioManager.MaxPainSound);
        audioManager.CreateSoundAtPoint(painClip, transform.position, 0.04f);

        if (CurrentClickCount >= ClicksToDefeat)
        {
            gameManager.OnEnemyDeath();
            AudioManager._Instance.CreateSoundAtPoint(AudioManager._Instance.MaxLeaveSound, transform.position, 0.04f);
            AudioManager._Instance.CombatBgmGo.GetComponent<AudioSource>().Stop();
            AudioManager._Instance.CalmBgmGo.GetComponent<BgmManager>().FadeIn();

            Destroy(this.gameObject);
        }
    }
}
