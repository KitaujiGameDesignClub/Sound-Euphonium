using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

namespace KitaujiGameDesignClub.GameFramework.UI
{
    public class ApplyUISettings : MonoBehaviour
    {
        //视频设置
        public TMP_Dropdown fullScreenMode;
        public TMP_Dropdown resolution;

        /// <summary>
        /// 可以用到的全部分辨率在其数组内的id
        /// </summary>
        private List<int> availableResolution = new List<int>();

        /// <summary>
        /// availableResolution内被选定的分辨率
        /// </summary>
        private int selectedResolutionId = -1;

        public TMP_Dropdown antiAliasing;
        public LeanToggle sync;

        public LeanToggle dithering;

        //音频设置
        public Slider MusicVolSlider;

        public Slider EffectVolSlider;

        //调试模式
        public LeanToggle showFps;
        public LeanToggle showConsole;


        // Start is called before the first frame update
        void Start()
        {
            //注册事件，滑条更新音量
            MusicVolSlider.onValueChanged.AddListener(delegate(float arg0)
            {
                Settings.BasicSettingsContent.MusicVolume = arg0;
                if (PublicAudioSource.publicAudioSource != null)
                    PublicAudioSource.publicAudioSource.UpdateMusicVolume();
            });
            EffectVolSlider.onValueChanged.AddListener(delegate(float arg0)
            {
                Settings.BasicSettingsContent.SoundEffectVolume = arg0;
            });
        }


        /// <summary>
        /// 从内存中读取并应用设置，并修改设置界面
        /// </summary>
        [ContextMenu("从内存中读取设置，并修改设置界面")]
        public void ReadSettingsFromMemoryAndApplyToSettingsPage()
        {
            
            //android禁用组件
#if UNITY_ANDROID
             fullScreenMode.interactable = false;
            resolution.interactable = false;
#endif


            //视频设置
            fullScreenMode.value = (int)Settings.BasicSettingsContent.fullscreenMode == 1 ? 0 : 1;


            //本机所有分辨率
            var machineResoultion = Screen.resolutions;
           
            //清除分辨率下拉框
            resolution.ClearOptions();
            //弄个容器，存放所有可用分辨率
            var available = new List<TMP_Dropdown.OptionData>();
            //默认值“不修改”作为第一个
            available.Add(new TMP_Dropdown.OptionData(text: "不修改"));
         
          
            for (int i = 0; i < machineResoultion.Length; i++)
            {
                //如果是16：9，则视为可用分辨率，加到下拉框里
                if (Mathf.Abs((float)machineResoultion[i].width / machineResoultion[i].height - 1.77f) <= 0.1f)
                {
                    //此分辨率添加到下拉框
                    var res =new TMP_Dropdown.OptionData( $"{machineResoultion[i].width.ToString()} x {machineResoultion[i].height.ToString()}");

                    if (!available.Contains(res))
                    {
                        available.Add(res);
                    }
                }
            }
            resolution.AddOptions(available);
            //设置下拉框的初始值
            resolution.value = 0;

            antiAliasing.value = (int)Settings.BasicSettingsContent.antiAliasing;
           
          
            MusicVolSlider.value = Settings.BasicSettingsContent.MusicVolume;
            EffectVolSlider.value = Settings.BasicSettingsContent.SoundEffectVolume;

            if (Settings.BasicSettingsContent.showConsole)
            {
                showConsole.TurnOn();
                showConsole.OnTransitions.Begin();
            }
            else
            {
                showConsole.TurnOff();
                showConsole.OffTransitions.Begin();
            }

            if (Settings.BasicSettingsContent.dithering)
            {
                dithering.TurnOn();
                dithering.OnTransitions.Begin();
            }
            else
            {
                dithering.TurnOff();
                dithering.OffTransitions.Begin();
            }
            
            if (Settings.BasicSettingsContent.sync)
            {
                sync.TurnOn();
                sync.OnTransitions.Begin();
            }
            else
            {
                sync.TurnOff();
                sync.OffTransitions.Begin();
            }
          
            if (Settings.BasicSettingsContent.showFps)
            {
                showFps.TurnOn();
                showFps.OnTransitions.Begin();
            }
            else
            {
                showFps.TurnOff();
                showFps.OffTransitions.Begin();
            }
        }

        [ContextMenu("从文件中读取设置,并修改设置界面")]
        public void ReadSettingsFromFileAndApplyToSettingsPage()
        {
            Settings.ReadSettings();
            ReadSettingsFromMemoryAndApplyToSettingsPage();
        }


        [ContextMenu("写入文件并应用设置")]
        public void WriteAndApplySettings()
        {
          
            if (fullScreenMode.value == 0)
            {
                Settings.BasicSettingsContent.fullscreenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                Settings.BasicSettingsContent.fullscreenMode = FullScreenMode.Windowed;
            }
           
            if (resolution.value != 0)
            {
                var res = resolution.options[resolution.value].text.Split(" x ");
                Settings.BasicSettingsContent.resolution = new Resolution()
                {
                    height = int.Parse(res[1]),
                    width = int.Parse(res[0]),
                    refreshRate = 60,
                };
                
            }

            Settings.BasicSettingsContent.sync = sync.On;
            Settings.BasicSettingsContent.antiAliasing = antiAliasing.value; //其他场景相机自动读取
            //相机一起修改了
            
            Settings.BasicSettingsContent.showConsole = showConsole.On; //此设置适用于游戏场景。进入游戏场景时自动读取此设置，并应用
            Settings.BasicSettingsContent.showFps = showFps.On; //此设置适用于游戏场景。进入游戏场景时自动读取此设置，并应用
            Settings.BasicSettingsContent.dithering = dithering.On;
            Settings.BasicSettingsContent.MusicVolume = MusicVolSlider.value; //此设置有onValueChanged事件直接修改音量。其他场景会自动读取
            Settings.BasicSettingsContent.SoundEffectVolume =
                EffectVolSlider.value; //此设置有onValueChanged事件直接修改音量。其他场景会自动读取

            //写入yaml 
            YamlReadWrite.Write(Settings.BasicSettingIO, Settings.BasicSettingsContent);
            //应用设置
            Settings.ApplySettings();
        }
        
        
        public void InitializeSettings()
        {
            Settings.InitializeSettings();
          
        
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
    
    
   
}