using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Euphonium : MonoBehaviour
{
    /// <summary>
    /// 活塞活动记录
    /// </summary>
    public string[] Psitons;
    
    
    
    private Animator animator;

    /// <summary>
    /// 活塞  0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    public Transform[] PsitonsTransforms;

    public Transform sliderBirthPlace;
    
    /// <summary>
    /// 0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    private bool[] _keyDown = new bool[3];

    /// <summary>
    /// 活塞未被按下去时，的Z坐标（Local）
    /// </summary>
    private Vector3[] unpressedPsitonsZ = new Vector3[3];

    /// <summary>
    /// 活塞被按下去时，的Z坐标（Local）
    /// </summary>
    private  Vector3[] pressedPsitonZ = new Vector3[3];

    private float TimeDelta;
    
    private void Awake()
    {
       
        animator = GetComponent<Animator>();
        
        //缓存未被按下和被按下的活塞的局部坐标
        for (int i = 0; i < 3; i++)
        {
            unpressedPsitonsZ[i] = new Vector3(PsitonsTransforms[i].localPosition.x,
                PsitonsTransforms[i].localPosition.y, -0.6273391F);
            pressedPsitonZ[i] = new Vector3(PsitonsTransforms[i].localPosition.x,
                PsitonsTransforms[i].localPosition.y,  -0.55f);
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        _keyDown[0] = Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftArrow);
        _keyDown[1] = Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.DownArrow);
        _keyDown[2] = Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow);
        TimeDelta = Time.deltaTime;
        
        
        for (int i = 0; i < 3; i++)
        {
            //如果某个活塞对应的按键，发生了按下或者没按下
            if (_keyDown[i])
            {
                PsitonsTransforms[i].localPosition =
                    Vector3.Lerp(PsitonsTransforms[i].localPosition, pressedPsitonZ[i], 60f * TimeDelta);
            }
            else
            {
                PsitonsTransforms[i].localPosition =
                    Vector3.Lerp(PsitonsTransforms[i].localPosition, unpressedPsitonsZ[i], 60f * TimeDelta);
            }
        }
        
     
    }


    /// <summary>
    /// 解析活塞活动记录
    /// </summary>
    private void ParsePsitons()
    {
        foreach (var Psiton in Psitons)
        {
            //分离记录
            string[] s = Psiton.Split(":");
            //得到位置（视频第几帧）
            int location = int.Parse(s[0]);
            //得到第几个塞子
            int which = int.Parse(s[1][..1]);
            //那个塞子是被按下去了吗
            bool pressed = s[1].Substring(1, 1) == "P";
        }
    }






#if UNITY_EDITOR
    /// <summary>
    /// 保存活塞活动记录
    /// </summary>
    public void Save()
    {
        
    }
    
    /// <summary>
    /// 读取活塞活动记录
    /// </summary>
    public void Load()
    {
        
    }
    
#endif

    //这里记录一下：GameBody下的滑条（Slider，即玩家按键提示部分），单方向向右每延伸0.5，scale.x+1
    //每个视频帧=scale.x=0.02
  
}
