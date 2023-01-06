using UnityEngine;

public class MouseBehavior : MonoBehaviour
{

    public LayerMask FoodPlacementLayermask;
    public LayerMask CharacterLayermask;
    private Camera cam;
    private FoodManager foodManager;

    private void Start()
    {
        cam = Camera.main.GetComponent<Camera>();
        foodManager = FoodManager._Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, FoodPlacementLayermask))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
}