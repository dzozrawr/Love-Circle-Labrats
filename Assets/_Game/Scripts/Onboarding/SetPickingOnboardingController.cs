using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPickingOnboardingController : MonoBehaviour
{
    public List<GameObject> onboardingElementsList = new List<GameObject>();

    public List<Button> buttonsToReEnable = new List<Button>();

    public GameObject setPickElementsParent = null;

    private void Start()
    {
        if (GameController.missionID != 2)
        {
            foreach (GameObject go in onboardingElementsList)
            {
                Destroy(go);
            }

            foreach (Button b in buttonsToReEnable)
            {
                b.enabled = true;
            }

            if (setPickElementsParent != null)  //reenabling set buttons
            {
                foreach (Button b in setPickElementsParent.GetComponentsInChildren<Button>())
                {
                    b.enabled = true;
                }
            }
        }
    }
}

