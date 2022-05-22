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
    Animator anim; // 몬스터 애니메이터
    public RectTransform rt_panel; // 동작 제어 패널
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
    bool isPanelOn = false; // 현재 패널이 켜져있으면 true

    #region 몬스터 정보
    public enum State {Egg, Baby, Kid }
    public class MonsterInfo
    {
        private string name; //  사용자가 지정한 이름
        private int curLevel; // 현재 레벨
        private float curEXP; // 현재 경험치
        private State curState; // 현재 상태 [Egg, Baby, Kid]

        // 몬스터의 경험치를 올려주는 함수
        public void UpExp(float EXP)
        {
            curEXP += EXP;

            //경험치가 100%가 되면 레벨업
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
    #region 터치 동작
    public void TouchDown()
    {
        anim.SetTrigger("touch"); // 터치 동작 요청
        isTouch = true;
        pos_down = transform.position; // 현재 캐릭터의 위치
        StartCoroutine(DragCheck()); // 드래그 동작 코루틴
    }
    Coroutine co_panelOnOff = null;
    public void TouchUp()
    {
        isTouch = false;
        pos_up = transform.position;
        float _distance = Vector3.Distance(pos_down, pos_up);
        if (_distance < 1f) // 움직인 거리가 얼마 안될 때만 패널 On
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
        pos_pivot = pos - transform.position; // 터치한 지점을 좌표로 인식
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
    #region 패널 내부 버튼 기능
    //밥주기 동작
    public void Feed()
    {
        curInfo.UpExp(10f);
    }
    
    #endregion

}
