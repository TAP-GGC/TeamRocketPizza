    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Tilemaps;
    using UnityEngine.UI;

    public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        [SerializeField] private GameObject prefabInstance;
        [SerializeField] private GameObject prefabCollider;
        private GameObject currentInstance;
        private Canvas canvas; //grab the component from canvas
        private Camera mainCamera;
        private Defense tower;
        private List<Transform> slotTransforms = new List<Transform>();
        private Text defDesc;
        private Text defName;


        void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            GameObject[] slots = GameObject.FindGameObjectsWithTag("Slots");
            foreach (GameObject slot in slots)
            {
                slotTransforms.Add(slot.transform);
            }
            mainCamera = Camera.main;
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
                tower = currentInstance.GetComponent<Defense>();
                
                tower.enabled = false;
                
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
            Debug.Log("OnDrag");

            if (currentInstance != null)
            {
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
                tower.enabled = true;
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
                if (closestSlot != null && closestDistance <= 1.3f & closestSlot.CompareTag("Slots") && closestSlot.tag != "Occupied")
                {
                    // Check if the slot is already tagged as "Occupied"
                    // Spend coins and place tower only if enough coins are available
                    if (LevelManager.main.SpendCoins(tower.cost))
                    {
                        Debug.Log("Snapping to closest slot and spending coins");
                        currentInstance.transform.position = closestSlot.position;
                        // Mark the slot as occupied by setting the tag
                        closestSlot.tag = "Occupied";
                        tower.SetOccupiedSlot(closestSlot);
                    }
                    else
                    {
                        Debug.Log("Not enough coins to place the tower");
                        Destroy(currentInstance.gameObject); // Destroy the object if not enough coins
                    }
                    
                }
                else
                {
                    Debug.Log("No valid slot within range");
                    Destroy(currentInstance.gameObject); // Destroy the object if no valid slot is found
                }
                    
            }
            else{
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
                if(clickedObject.CompareTag("FirewallTower") && clickedObject.layer == LayerMask.NameToLayer("UI")){
                    defDesc.text = "";
                    defName.text = "";
                    defName.text = "Firewall Tower";
                    defDesc.text = "===========\n A pulsing fire tower. It can slow down the viruses within its range.";
                }
                if(clickedObject.CompareTag("IDSTower") && clickedObject.layer == LayerMask.NameToLayer("UI")){
                    defDesc.text = "";
                    defName.text = "";
                    defName.text = "IDS Tower";
                    defDesc.text = "===========\n A powerful defense that can defends against anything, LASERRR BEAAM!!!";
                }
                if(clickedObject.CompareTag("BackupTower") && clickedObject.layer == LayerMask.NameToLayer("UI")){
                    defDesc.text = "";
                    defName.text = "";
                    defName.text = "Back-up tower";
                    defDesc.text = "===========\n A tower that pulse a shockwave, it helps gets rid of the ransomware.";
                }
            }
            
        }

    
}
