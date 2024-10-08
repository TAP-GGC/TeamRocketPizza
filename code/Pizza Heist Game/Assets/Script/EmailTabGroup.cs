// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class EmailTabGroup : MonoBehaviour
// {

//     public List<EmailTabButton> tabButtons;
//     public Color tabIdle;
//     public Color tabHover;
//     public Color tabActive;
//     public EmailTabButton selectedTab;
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     public void Subscribe(EmailTabButton button)
//     {
//         if (tabButtons == null)
//         {
//             tabButtons = new List<EmailTabButton>();
//         }

//         tabButtons.Add(button);
//     }


//     public void OnTabEnter(EmailTabButton button)
//     {
//         ResetTabs();
//         if (button.background != null)
//         {
//             button.background.color = tabHover;
//         }
//     }

//     public void OnTabExit(EmailTabButton button)
//     {
//         ResetTabs();
//     }

//     public void OnTabSelected(EmailTabButton button)
//     {
//         if (button.background != null)
//         {
//             ResetTabs();
//             button.background.color = tabActive;
//         }
        
//     }

//     public void ResetTabs()
//     {
//         foreach (EmailTabButton button in tabButtons)
//         {
//             if (button.background != null)
//             {
//                 button.background.color = tabIdle;
//             }
//         }
//     }



//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
