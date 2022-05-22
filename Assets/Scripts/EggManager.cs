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
    Vector3 pos_down;
    Vector3 pos_up;
    Vector3 pos_pivot;
    public void TouchDown()
    {
        anim.SetTrigger("touch");
        isTouch = true;
        pos_down = transform.position;
        StartCoroutine(DragCheck());
    }
    bool isPanelOn = false;
    public void TouchUp()
    {
        isTouch = false;
        pos_up = transform.position;
        float _distance = Vector3.Distance(pos_down, pos_up);
        if(_distance<1f)
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
        pos_pivot = Input.mousePosition - transform.position;
        while (isTouch)
        {
            transform.position = Input.mousePosition;
            transform.position -= pos_pivot;
            TouchTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

}
