using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExpandFuncs;
public class MonsterManager : MonoBehaviour
{
    private static MonsterManager instance;
    public static MonsterManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }
    Animator anim; // ���� �ִϸ�����
    public RectTransform rt_panel; // ���� ���� �г�
    public Text text_EXP;
    public Image img_EXP;
    public Text text_Level;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Start()
    {
        SettingInfo();
    }
    bool isTouch = false;
    Vector3 pos_down;
    Vector3 pos_up;
    Vector3 pos_pivot;
    bool isPanelOn = false; // ���� �г��� ���������� true

    #region ���� ����
    public enum State {Egg, Baby, Kid }
    public class MonsterInfo
    {
        private string name; //  ����ڰ� ������ �̸�
        private int curLevel; // ���� ����
        private float curEXP; // ���� ����ġ
        private State curState; // ���� ���� [Egg, Baby, Kid]

        // ������ ����ġ�� �÷��ִ� �Լ�
        public void UpExp(float EXP)
        {
            curEXP += EXP;

            //����ġ�� 100%�� �Ǹ� ������
            if(curEXP>=100f)
            {
                UpExp(-100f);
                UpLevel();
            }
            UpdateData();
        }
        public void UpLevel()
        {
            curLevel++;
            UpdateData();
        }
        public void UpdateData()
        {
            instance.text_EXP.text = curEXP.ToString()+"%";
            instance.img_EXP.fillAmount = curEXP * 0.01f;
            instance.text_Level.text = "Lv"+curLevel;
        }
    }
    MonsterInfo curInfo;
    public void SettingInfo()
    {
        curInfo = new MonsterInfo();
    }


    #endregion
    #region ��ġ ����
    public void TouchDown()
    {
        anim.SetTrigger("touch"); // ��ġ ���� ��û
        isTouch = true;
        pos_down = transform.position; // ���� ĳ������ ��ġ
        StartCoroutine(DragCheck()); // �巡�� ���� �ڷ�ƾ
    }
    Coroutine co_panelOnOff = null;
    public void TouchUp()
    {
        isTouch = false;
        pos_up = transform.position;
        float _distance = Vector3.Distance(pos_down, pos_up);
        if (_distance < 1f) // ������ �Ÿ��� �� �ȵ� ���� �г� On
        {
            if(co_panelOnOff!=null)
            {
                StopCoroutine(co_panelOnOff);
            }
            if (isPanelOn)
            {
                co_panelOnOff = StartCoroutine(rt_panel.ScaleUp(Vector2.zero, 0.25f));
            }
            else
            {                
                co_panelOnOff = StartCoroutine(rt_panel.ScaleUp(Vector2.one, 0.25f));
            }
            isPanelOn = !isPanelOn;
        }
    }
    float TouchTime = 0f;
    IEnumerator DragCheck()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        TouchTime = 0f;
        pos_pivot = pos - transform.position; // ��ġ�� ������ ��ǥ�� �ν�
        while (isTouch)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
            transform.position -= pos_pivot;
            TouchTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    } 
    #endregion
    #region �г� ���� ��ư ���
    //���ֱ� ����
    public void Feed()
    {
        curInfo.UpExp(10f);
    }
    
    #endregion

}
