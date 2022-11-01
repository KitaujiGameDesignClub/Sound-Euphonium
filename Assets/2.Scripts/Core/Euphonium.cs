using System;
using UnityEngine;
using UnityEngine.Events;
using KitaujiGameDesignClub.GameFramework;
using KitaujiGameDesignClub.GameFramework.@interface;
using KitaujiGameDesignClub.GameFramework.UI;
 
public class Euphonium : MonoBehaviour,IUpdate
{
    
    //io
    public BasicYamlIO PsitonsActionIO = new BasicYamlIO("PsitonsAction",

        note: "# 活塞记录，用于记录活塞是如何运动的，并产生滑块\n# 格式为 视频帧数:活塞活动\n# 视频帧数：用的是游戏中显示的视频，同时也是Resources文件夹内的Pr工程的视频\n" +
              "# 活塞活动：P(ressed）按下，U(npressed)没按下。前面的数字（123）没用了\n# 如果要进行修改，不要直接照抄游戏右上角的Frame参数，因为有位置的偏差。正确做法是将节奏正确的frame与位置正确的frame进行对比，用差值前移或后移滑块" +
              "\n# DebugMode:无用的变量。");
   [Serializable]
    public struct PsitonsAction
    {
        public bool DebugMode;
        public string[] Psiton1;
        public string[] Psiton2;
        public string[] Psiton3;

        public PsitonsAction(string s)
        {

            DebugMode = false;
            Psiton1 = new[]
            {
                "202:1P",
                "222:1U",
                "334:1P",
                "359:1U",
                "421:1P",
                "454:1U",
                "492:1P",
                "529:1U",
                "653:1P",
                "681:1U",
                "775:1P",
                "796:1U",
                "933:1P",
                "952:1U",
                "1018:1P",
                "1145:1U",
                "1312:1P",
                "1341:1U",
                "1442:1P",
                "1473:1U",
                "1533:1P",
                "1560:1U",
                "1597:1P",
                "1620:1U",
                "1745:1P",
                "1920:1U",
                "1970:1P",
                "2012:1U",
                "2053:1P",
                "2169:1U",
                "2223:1P",
                "2520:1U",
                "2617:1P",
                "2654:1U",
                "2676:1P",
                "2791:1U",
                "2921:1P",
                "3237:1U",
                "3375:1P",
                "3383:1U",
                "3386:1P",
                "3392:1U",
                "3398:1P",
                "3407:1U",
                "3520:1P",
                "3527:1U",
                "3639:2P",
                "3905:2U",
                "3969:1P",
                "4330:1U",
            };

            Psiton2 = new[]
            {
                "2053:2P",
                "2169:2U",
                "2223:2P",
                "2267:2U",
                "2315:2P",
                "2520:2U",
                "2676:2P",
                "2791:2U",
                "3398:1P",
                "3407:1U",
                "3533:2P",
                "3527:2U",
                "3639:2P",
                "3667:2U",
                "3835:2P",
                "3905:2U",
            };
            Psiton3 = new[]
            {
                "97:3P",
                "202:3U",
                "626:1P",
                "653:1U",
                "783:3P",
                "806:3U",
                "915:3P",
                "933:3U",
                "1235:3P",
                "1310:3U",
                "1620:3P",
                "1679:3U",
            };
        }
    }

    public PsitonsAction psitonsAction = new PsitonsAction("6");



    public GameObject euphoModel;


    public Transform Panding;
    public Transform gameBody;



    
    /// <summary>
    /// 活塞  0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    public Transform[] PsitonsTransforms;

    /// <summary>
    /// 0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    public GameObject[] sliders;

    /// <summary>
    /// 粒子成功之后的粒子效果
    /// </summary>
    public ParticleSystem[] successParticleSystems;


    public ScreenButton[] screenButtons;

    /// <summary>
    /// 所有滑块，都是这个的子物体。游戏开始后是sliderParent滑动  
    /// </summary>
    public Transform sliderParent;

    public UnityEvent onInitialization = new UnityEvent();
    public UnityEvent onPressedInTime = new UnityEvent();
    public UnityEvent onMiss = new UnityEvent();
    public UnityEvent watchVideo = new UnityEvent();
    public UnityEvent showAchievement = new UnityEvent();
    

    /// <summary>
    /// 三个轨道滑块的出生位置（local，相对于GameBody）， 0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    private Vector3[] sliderBirthPlace = new Vector3[3];
    //sliderBirthPlace：位置是上一个滑块的后面的空白的最右侧位置。相当于下一个滑块的出生位置


    /// <summary>
    /// 在判定方块那里产生射线的起始位置，以进行判定    0=1键J/左箭头 ；1=2键K/下箭头；2=3键L/右箭头
    /// </summary>
    private Vector3[] panDingRaycast = new Vector3[3];

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
    private Vector3[] pressedPsitonZ = new Vector3[3];


    private float TimeDelta;

    /// <summary>
    /// 阶段
    /// </summary>
    private int episode;
    

    private void Start()
    {
        //初始化
        Initialization();
        //解析活塞记录
        ParsePsitons(0);
        ParsePsitons(1);
        ParsePsitons(2);

      
        //缓存未被按下和被按下的活塞的局部坐标
        for (int i = 0; i < 3; i++)
        {
            unpressedPsitonsZ[i] = new Vector3(PsitonsTransforms[i].localPosition.x,
                PsitonsTransforms[i].localPosition.y, -0.6273391F);
            pressedPsitonZ[i] = new Vector3(PsitonsTransforms[i].localPosition.x,
                PsitonsTransforms[i].localPosition.y, -0.55f);
        }

       StaticVideoPlayer.staticVideoPlayer.Play();
       
       //注册update
       UpdateManager.RegisterUpdate(this);
    }


    // Update is called once per frame
    public void FastUpdate()
    {

        if (!StaticVideoPlayer.staticVideoPlayer.VideoPlayer.isPlaying || Time.timeScale < 0.5f) return;

        TimeDelta = Time.deltaTime;


        //判断演奏的阶段
        switch (StaticVideoPlayer.staticVideoPlayer.VideoPlayer.frame)
        {
            //前半段，玩家吹上低音号
            case < 4360:
                sliderParent.Translate(TimeDelta * 0.12F * Vector3.left, Space.Self);
                sliderParent.localPosition = new Vector3(sliderParent.localPosition.x, 0F, 0.005F);
                break;
            //后面看视频结束
            case >= 4419 when episode == 0:
                euphoModel.SetActive(false);
                gameBody.gameObject.SetActive(false);
                episode++;
                watchVideo.Invoke();
                for (int i = 0; i < 3; i++)
                {
                    Destroy(screenButtons[i].gameObject);
                }
                break;
            //视频放完，结算画面
            case >= 5273:
                showAchievement.Invoke();
                UpdateManager.Remove(this);
                Debug.Log("视频播放完成");
                break;
        }


        #if UNITY_EDITOR || UNITY_STANDALONE_WIN
        
        _keyDown[0] = Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftArrow);
        _keyDown[1] = Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.DownArrow);
        _keyDown[2] = Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow);
        
        #elif UNITY_ANDROID
          for (int i = 0; i < 3; i++)
        {
            _keyDown[i] = screenButtons[i].OnPressed;
        }
        
#endif
      
      
//活塞按下松开的动画
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

        //射线判定是否正确按下活塞
        RayUpdate();
    }

    private void RayUpdate()
    {
        if (Time.timeScale <= 0.5f)
        {
            return;
        }
        
        for (int i = 0; i < 3; i++)
        {
            //只有在射线碰上且按下按键的时候，才算俺的及时
            if (_keyDown[i] && Physics.Raycast(panDingRaycast[i], Vector3.forward, 0.1f, 1 << 7))
            {
                onPressedInTime.Invoke();
                
                //激活粒子效果
                successParticleSystems[i].Play();
            }
            //来了，没按不行
            else if (Physics.Raycast(panDingRaycast[i], Vector3.forward, 0.1f, 1 << 7) && !_keyDown[i])
            {
                onMiss.Invoke();
            }
            //提前按也不行
            else if (!Physics.Raycast(panDingRaycast[i], Vector3.forward, 0.1f, 1 << 7) && _keyDown[i])
            {
                onMiss.Invoke();
            }
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    private void Initialization()
    {
        //调用初始化事件
        onInitialization.Invoke();
        //读取上低音号的节拍文件
        psitonsAction = YamlReadWrite.Read(PsitonsActionIO, psitonsAction);

#if UNITY_ANDROID
        //显示Android的按键
        foreach (var VARIABLE in screenButtons)
        {
            VARIABLE.gameObject.SetActive(true);
        }
#endif 
       
     
       
        //初始化滑块的出生位置（相对于gameBody）
        var localPosition = sliderParent.localPosition;
        sliderBirthPlace[1] = localPosition;
        sliderBirthPlace[2] = new Vector3(localPosition.x, 0.14f, localPosition.z);
        sliderBirthPlace[0] = new Vector3(localPosition.x, -0.14f, localPosition.z);
        //panDingRaycast在panding方块上位置的初始化（转为世界坐标）
        var position = Panding.localPosition;
        panDingRaycast[1] = Panding.position;
        panDingRaycast[0] = gameBody.TransformPoint(new Vector3(position.x, -0.14f, position.z));
        panDingRaycast[2] = gameBody.TransformPoint(new Vector3(position.x, 0.14f, position.z));
    }


    /// <summary>
    /// 解析活塞活动记录
    /// </summary>
    /// <param name="psitonsActionIndex">解析第几个活塞</param>
    private void ParsePsitons(int psitonsActionIndex)
    {
        // 暂存每个解析后产生的滑块
        Transform slider;

        
        string[] dividedPsitonsAction;
        switch (psitonsActionIndex)
        {
            case 0:
                dividedPsitonsAction = psitonsAction.Psiton1;
                break;
            case 1:
                dividedPsitonsAction = psitonsAction.Psiton2;
                break;
            case 2:
                dividedPsitonsAction = psitonsAction.Psiton3;
                break;
            default:
                dividedPsitonsAction = Array.Empty<string>();
                break;
        }

        //用于缓存锚点
        Vector3[] anchorPoint = new Vector3[dividedPsitonsAction.Length];

//计算锚点 视频帧*0.01，作为间隔
        for (int i = 0; i < dividedPsitonsAction.Length; i++)
        {
            //分离记录
            string[] s = dividedPsitonsAction[i].Split(":");
            //储存锚点位置
            anchorPoint[i] = new Vector3(float.Parse(s[0]) * 0.01f + sliderBirthPlace[psitonsActionIndex].x,
                sliderBirthPlace[psitonsActionIndex].y, sliderBirthPlace[psitonsActionIndex].z);
        }


        //再来一遍，迫不得已了。。
        //取一半
        for (int i = 0; i < dividedPsitonsAction.Length; i++)
        {
            //i为偶数。即P（按下）
            if (i % 2 == 0)
            {
                //在记录的上一个滑块尾（含空白）处生成新的滑块
                slider = Instantiate(sliders[psitonsActionIndex], Vector3.one, gameBody.rotation, gameBody).transform;
                //在这里修复位置
                slider.localPosition = new Vector3((anchorPoint[i].x + anchorPoint[i + 1].x) / 2f, anchorPoint[i].y,
                    anchorPoint[i].z);
                //计算同组的U P之间,X的距离
                float distance = anchorPoint[i + 1].x - anchorPoint[i].x;
                //宽度修复，左右把U P的锚点连接起来
                slider.localScale = new Vector3(distance, 0.122253f, 1f);
                //父对象修复
                slider.parent = sliderParent;
            }
        }
    }


    /// <summary>
    /// 跳过最后的视频片段
    /// </summary>
    public void skip()
    {
      UpdateManager.Remove(this);
    }
}