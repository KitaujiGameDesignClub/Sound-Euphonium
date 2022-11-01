using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 负责游戏中文字的动态变化和成果记录
/// </summary>
public class TextUI : MonoBehaviour
{
    public static TextUI textUI;
    

    // public TMP_Text Accuracy;

    /// <summary>
    /// 得分
    /// </summary>
    public TMP_Text score;

  

    /// <summary>
    /// 展示成果
    /// </summary>
    public TMP_Text showAchievement;
   
    /// <summary>
    /// 正确按下按键的个数
    /// </summary>
    private int right;
  
    /// <summary>
    /// 总共有多少按键
    /// </summary>
    private int total;
    

    private void Awake()
    {
        textUI = this;
        total = 0;
        right = 0;
        score.text = "";
    }
    

    // Update is called once per frame


    /// <summary>
    /// 触碰期间，判断玩家是否按下相应的按键（活塞），计算准确率
    /// </summary>
    /// <param name="inTime"></param>
    public void CheckPressedInTime(bool inTime)
    {
        if (inTime)
        {
            right++; 
            total++;
        }
        else total ++;

        score.text = $"准确率：{((float)right / total):P2}";

    }
    
    
    public void ShowAchievement()
    {
        showAchievement.text = score.text;
    }
    




  
  
}