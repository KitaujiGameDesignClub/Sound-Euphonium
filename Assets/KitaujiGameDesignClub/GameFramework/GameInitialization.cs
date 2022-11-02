
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace KitaujiGameDesignClub.GameFramework
{
    public class GameInitialization : MonoBehaviour
    {
        public int loadScene;
        public int openingScene;


#if UNITY_EDITOR
        public bool textGame = true;
#endif

        /// <summary>
        /// 在这里游戏初始化（在这里要对各种所需的yaml进行读取尝试，如果读取失败，就生成一个有默认值的文件）
        /// </summary>
        [ContextMenu("游戏初始化")]
        public virtual void Awake()
        {
            //读取游戏基础设置文件（如果文件不存在或者不合规，会重置）
            Settings.ReadSettings();
        
        Debug.Log("初始化方法Awake执行");
        }

        /// <summary>
        /// 在这里游戏初始化（在这里要对各种所需的yaml进行读取尝试，如果读取失败，就生成一个有默认值的文件）
        /// </summary>
        public virtual void Start()
        {
            //调整音量
            PublicAudioSource.publicAudioSource.UpdateMusicVolume();


            //加载场景
#if !UNITY_EDITOR
          SceneManager.LoadScene("Opening");

#else

            SceneManager.LoadScene(textGame ? loadScene : openingScene);
#endif
            
            
            Debug.Log("初始化方法Start执行");
        }



    }
}