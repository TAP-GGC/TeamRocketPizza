using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private GameObject prefabInstance;
    private GameObject currentInstance;
    private Canvas canvas; //grab the component from canvas
    private Camera mainCamera;
    [SerializeField] private LayerMask layer;
    private List<Transform> slotTransforms = new List<Transform>();

    private int turretCost;
    private Text defDesc;
    private Text defName;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
       GameObject[] slots = GameObject.FindGameObjectsWithTag("Slots");
        foreach (GameObject slot in slots)
        {
            slotTransforms.Add(slot.transform);
        }

        defDesc = GameObject.Find("DefenseDescription").GetComponent<Text>();
        defName = GameObject.Find("DefenseName").GetComponent<Text>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        if (prefabInstance != null)
        {
            // Instantiate the prefab in the game world
            currentInstance = Instantiate(prefabInstance);
            
            if(currentInstance.CompareTag("MalwareTower")){
                turretCost = 60;
                currentInstance.GetComponent<MalwareTower>().enabled = false;
            }
            else if(currentInstance.CompareTag("NetTower")){
                turretCost = 100;
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

            Transform closestSlot = null;
            float closestDistance = float.MaxValue;

            foreach (Transform slotTransform in slotTransforms)
            {
                float distance = Vector2.Distance(slotTransform.position, currentInstance.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSlot = slotTransform;
                }
            }

            // Snap to the closest slot if within range
            if (closestSlot != null && closestDistance <= 1.3f && LevelManager.main.SpendCoins(turretCost))
            {
                currentInstance.transform.position = closestSlot.position;
            }
            else{
                Destroy(currentInstance.gameObject);
            }
            // Optionally: Destroy the instantiated object or do something with it
            
        }else{
            Debug.Log("Instance is null");
        }
    }

    

    public void OnPointerDown(PointerEventData eventData)
    {
        
        GameObject clickedObject = eventData.pointerEnter;

        if(clickedObject != null){
            if(clickedObject.CompareTag("MalwareTower") && clickedObject.layer == LayerMask.NameToLayer("UI")){
                defDesc.text = "";
                defName.text = "";
                defName.text = "Malware Tower";
                defDesc.text = "===========\n A defense that only defends against normal malware viruses.";
            }
            if(clickedObject.CompareTag("NetTower") && clickedObject.layer == LayerMask.NameToLayer("UI")){
                defDesc.text = "";
                defName.text = "";
                defName.text = "Network Defender";
                defDesc.text = "===========\n A 2 shot defense, it also has the ability to detects worms and destroy them.";
            }
        }
        
    }
}
