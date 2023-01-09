using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseBehavior : MonoBehaviour
{

    public LayerMask FoodPlacementLayermask;
    public LayerMask BiofuelLayermask;
    public LayerMask CharacterLayermask;
    public LayerMask InteractionLayermask;
    private Camera cam;
    private FoodManager foodManager;
    private AudioManager audioManager;

    [SerializeField] private Transform currentlyHovering;

    public GameObject AttackParticleSystem;

    private void Start()
    {
        audioManager = AudioManager._Instance;
        cam = Camera.main.GetComponent<Camera>();
        foodManager = FoodManager._Instance;
    }

    private void Update()
    {
        Ray hoverRay = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(hoverRay.origin, hoverRay.direction * 1000f, Color.yellow, 0.1f);
        if (Physics.Raycast(hoverRay, out RaycastHit hoverHit, 200, InteractionLayermask))
        {
            currentlyHovering = hoverHit.transform;
        }
        else
        {
            currentlyHovering = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Pointer over UI GO");
                return;
            }

            Ray leftClickRay = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(leftClickRay.origin, leftClickRay.direction * 1000f, Color.red, 0.1f);

            if (Physics.Raycast(leftClickRay, out RaycastHit enemyHit, Mathf.Infinity, CharacterLayermask))
            {
                // Clicked on enemy
                //Debug.Log("Enemy Click");
                AudioClip clip = audioManager.RandomSoundFromArray(audioManager.AttackSounds);
                audioManager.CreateSoundAtPoint(clip, enemyHit.point, 0.025f);
                enemyHit.transform.root.gameObject.BroadcastMessage("TakeDamage");
                Instantiate(AttackParticleSystem, enemyHit.point, Quaternion.identity);
            }
            else if (Physics.Raycast(leftClickRay, out RaycastHit biofuelHit, Mathf.Infinity, BiofuelLayermask))
            {
                // Clicked on biofuel
                //Debug.Log("Biofuel Click");
                audioManager.CreateSoundAtPoint(audioManager.BiofuelPickupSound, biofuelHit.point, 0.015f);
                MoneyManager._Instance.AddMoney(15);
                Destroy(biofuelHit.transform.root.gameObject);
            }
            else if (Physics.Raycast(leftClickRay, out RaycastHit foodHit, Mathf.Infinity, FoodPlacementLayermask))
            {
                // Clicked on ground to place food
                //Debug.Log("Floor Click");
                foodManager.CreateFood(foodHit.point);
            }
            else
            {
                Debug.Log("No valid click found");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray rightClickRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rightClickRay, out RaycastHit biofuelHit, Mathf.Infinity, BiofuelLayermask))
            {
                // Clicked on biofuel
                //Debug.Log("Biofuel Click");
                //MoneyManager._Instance.AddMoney(15);
                //Destroy(biofuelHit.transform.root.gameObject);
            }
        }   
    }
}