using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingMiniGameCanvas : MonoBehaviour
{
    public GameObject thirdPlanGroup = null;
    public GameObject secondPlanGroup = null;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            i = (i + 1) % 2;
            ActivateNextPhase(i);
        }
    }
#endif

    public void ActivateNextPhase(int i)
    {
        if (i == 0)
        {
            thirdPlanGroup.SetActive(true);
            secondPlanGroup.SetActive(false);
        }else if (i == 1)
        {
            thirdPlanGroup.SetActive(false);
            secondPlanGroup.SetActive(true);
        }
    }

    public void DisableSecondPlanGroupSelection()
    {
        foreach (StencilButton s in secondPlanGroup.GetComponentsInChildren<StencilButton>())
        {
            if (s.Image.sprite == s.unselectedSprite)
            {
                s.Image.color = new Color(s.Image.color.r, s.Image.color.g, s.Image.color.b,0.5f);
            }
            s.Button.enabled = false;
        }
        
    }

    public void SetEnabledButtonsInGroup(GameObject group, bool enabledValue)
    {
        foreach (Button b in group.GetComponentsInChildren<Button>())
        {
            b.enabled = enabledValue;
        }            
    }
}
