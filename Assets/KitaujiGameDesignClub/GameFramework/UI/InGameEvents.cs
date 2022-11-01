using System;
using KitaujiGameDesignClub.GameFramework.@interface;
using UnityEngine;

namespace KitaujiGameDesignClub.GameFramework.UI
{
    public class InGameEvents : basicEvents,IUpdate
    {
        [Header("调试内容")] public GameObject[] DebugMode;
      [Header("展示fps的框框")]
        public GameObject fpsShow;
        
        private void Awake()
        {
            Settings.ApplySettings();

            if (!Settings.BasicSettingsContent.showConsole)
            {
                for (int i = 0; i < DebugMode.Length; i++)
                {
                    Destroy(DebugMode[i]);
                }
            }
            
            if (!Settings.BasicSettingsContent.showFps)
            {
               Destroy(fpsShow);
            }
        }

        private  void Start()
        {
            UpdateManager.RegisterUpdate(this);
            //给控制台发个彩蛋
            Debug();
        }
    
        public void FastUpdate()
        {
      
        
            //响应玩家的按键
            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale > 0.5f  && !IngameDebugConsole.DebugLogManager.ShowDebugLogManager)
            {
                OnEscapeClick.Invoke();
                Pause();

            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="force">强制级别。-1强制暂停 0根据游戏暂停状况调整 1强制继续游戏</param>
        public static void Pause()
        {
      
        
        
            if (Time.timeScale >= 0.5F)
            {
           
          
        
                Time.timeScale = 0f;
            
                if( StaticVideoPlayer.staticVideoPlayer != null) StaticVideoPlayer.staticVideoPlayer.Pause();

                if (PublicAudioSource.publicAudioSource != null)
                {
                    PublicAudioSource.publicAudioSource.PauseMusicPlaying();
                    PublicAudioSource.publicAudioSource.PlaySoundEffect(PublicAudioSource.AudioType.Click);
                }
         
            }
            else
            {

           
                Time.timeScale = 1f;
                if( StaticVideoPlayer.staticVideoPlayer != null)   StaticVideoPlayer.staticVideoPlayer.Play();
                if (PublicAudioSource.publicAudioSource != null)  PublicAudioSource.publicAudioSource.PlayBackgroundMusic(null);

            }
     
        }
    }
}
