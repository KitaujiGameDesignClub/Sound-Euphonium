using System;
using UnityEngine;

namespace KitaujiGameDesignClub.GameFramework
{
    /// <summary>
    /// 用于记录yaml文件在哪里读写，读写什么
    /// </summary>
    [Serializable]
    public class BasicYamlIO
    {
        public string FileName;
        public string Path;
        public string Note;

        public const int BasicYamlVersion = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">保存的文件名（不含拓展名）</param>
        /// <param name="path">从根目录开始的路径，与saves同级，开头结尾不能有“ / ”</param>
        /// <param name="note">注释（需要写入#符号）</param>
        public BasicYamlIO(string fileName ="Default",string path = "saves",string note = null)
        {
            FileName = fileName;
            Path = path;
            Note = note;
        }
    }

    /// <summary>
    /// 游戏基本的设置（为了兼容旧游戏）
    /// </summary>
    [Serializable]
    public struct BasicSettings
    {
        public const int SettingVersion = 1;
        
        //音频设置
        public float MusicVolume;
        public float SoundEffectVolume;
       
        //视频设置
        public FullScreenMode fullscreenMode ;
        public Resolution resolution;
        public bool sync;
        public int antiAliasing;
        public bool dithering;
        
        //调试模式
        public bool showFps;
        public bool showConsole;
        
        
        /// <summary>
        /// 初始化（不用任何变量，直接初始化得了）
        /// </summary>
        /// <param name="musicVolume"></param>
        /// <param name="soundEffectVolume"></param>
        public BasicSettings(float musicVolume,float soundEffectVolume)
        {
            //默认设置
            MusicVolume = musicVolume;
            SoundEffectVolume = soundEffectVolume;
            fullscreenMode = FullScreenMode.Windowed;
            sync = false;
            resolution = new Resolution()
            {
                height = 720,
                width = 1280,
                refreshRate = 60,
            };
            antiAliasing = 1;
            dithering = false;
            showConsole = true;
            showFps = true;
        }
    }
}