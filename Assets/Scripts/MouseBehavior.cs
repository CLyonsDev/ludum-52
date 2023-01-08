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

    [SerializeField] private Transform currentlyHovering;

    private void Start()
    {
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

            Ray clickRay = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(clickRay.origin, clickRay.direction * 1000f, Color.red, 0.1f);

            if (Physics.Raycast(clickRay, out RaycastHit enemyHit, Mathf.Infinity, CharacterLayermask))
            {
                // Clicked on enemy
                //Debug.Log("Enemy Click");
                enemyHit.transform.root.gameObject.BroadcastMessage("TakeDamage");
            }
            else if (Physics.Raycast(clickRay, out RaycastHit biofuelHit, Mathf.Infinity, BiofuelLayermask))
            {
                // Clicked on biofuel
                //Debug.Log("Biofuel Click");
                MoneyManager._Instance.AddMoney(25);
                Destroy(biofuelHit.transform.root.gameObject);
            }
            else if (Physics.Raycast(clickRay, out RaycastHit foodHit, Mathf.Infinity, FoodPlacementLayermask))
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
    }
}