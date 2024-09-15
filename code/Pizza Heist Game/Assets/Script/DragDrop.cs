using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private GameObject prefabInstance;
    private GameObject currentInstance;
    private Canvas canvas; //grab the component from canvas
    private Camera mainCamera;
    
    
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        if (prefabInstance != null)
        {
            // Instantiate the prefab in the game world
            currentInstance = Instantiate(prefabInstance);
            if(currentInstance.CompareTag("MalwareTower")){
                currentInstance.GetComponent<MalwareTower>().enabled = false;
            }
            else if(currentInstance.CompareTag("NetTower")){
                currentInstance.GetComponent<NetTower>().enabled = false;
            }
            else{
                Debug.Log("No Instance found.");
            }
            // Set the initial position of the object
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                mainCamera,
                out worldPosition
            );
            currentInstance.transform.position = worldPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");

        if (currentInstance != null)
        {

            
            // else{
            //     Debug.Log("script didnt enable");
            // }
            Vector3 worldPosition;
            //set the pointer to the world and not canvas
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                mainCamera,
                out worldPosition
            );
            currentInstance.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        
        if (currentInstance != null)
        {
            if(currentInstance.CompareTag("MalwareTower")){
                currentInstance.GetComponent<MalwareTower>().enabled = true;
            }
            else if(currentInstance.CompareTag("NetTower")){
                currentInstance.GetComponent<NetTower>().enabled = true;
            }
            else{
                Debug.Log("No Instance found.");
            }
            // Optionally: Destroy the instantiated object or do something with it
            
        }else{
            Debug.Log("Instance is null");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
