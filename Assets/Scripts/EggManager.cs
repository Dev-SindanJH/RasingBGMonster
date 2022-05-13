using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    Animator anim;
    public RectTransform rt_panel;
    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    bool isTouch = false;
    public void TouchDown()
    {
        anim.SetTrigger("touch");
        isTouch = true;
        StartCoroutine(DragCheck());
    }
    bool isPanelOn = false;
    public void TouchUp()
    {
        isTouch = false;
        if(TouchTime<0.2f)
        { 
            if(isPanelOn)
            {
                rt_panel.localScale = Vector3.zero;
            }
            else
            {
                rt_panel.localScale = Vector3.one;
            }
            isPanelOn = !isPanelOn;
        }
        
    }
    float TouchTime = 0f;
    IEnumerator DragCheck()
    {
        TouchTime = 0f;
        while(isTouch)
        {
            transform.position = Input.mousePosition;
            TouchTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log(TouchTime);
        yield break;
    }

}
