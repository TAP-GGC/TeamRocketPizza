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
        private Tilemap map;
        private GameObject currentInstance;
        private Canvas canvas; //grab the component from canvas
        private Camera mainCamera;
        private Defense tower;
        private BoxCollider2D towerCollider;
        private Text defDesc;
        private Text defName;
        void Start()
        {
            canvas = GetComponentInParent<Canvas>();

            mainCamera = Camera.main;
            map = GameObject.Find("Background").GetComponent<Tilemap>();
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
                towerCollider = currentInstance.GetComponent<BoxCollider2D>();
                tower.enabled = false;
                // towerCollider.enabled = false;
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
                // towerCollider.enabled = true;
                Vector3Int cellPosition = map.WorldToCell(currentInstance.transform.position);
                
                
                    if (map.HasTile(cellPosition))
                    {
                        if(!tower.isColliding && LevelManager.main.SpendCoins(tower.cost)){
                            Debug.Log("is not colliding with anything");
                            currentInstance.transform.position = map.GetCellCenterWorld(cellPosition);
                    
                        }else{
                            Debug.Log("is Colliding");
                            Destroy(currentInstance);
                        }
                    }
                    else
                    {
                        Destroy(currentInstance);
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
            }
            
        }
    }
