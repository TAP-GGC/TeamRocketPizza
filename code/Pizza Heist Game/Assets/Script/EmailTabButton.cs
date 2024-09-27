using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EmailTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public EmailTabGroup tabGroup;
    public Image background;


    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);

        //Get game objected called "Content" and set it to tabGroup

        tabGroup = transform.parent.parent.GetComponent<EmailTabGroup>();
        



    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}
