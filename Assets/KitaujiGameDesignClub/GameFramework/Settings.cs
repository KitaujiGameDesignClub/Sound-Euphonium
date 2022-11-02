using KitaujiGameDesignClub.GameFramework.UI;
using UnityEngine;

namespace KitaujiGameDesignClub.GameFramework
{
    /// <summary>
    /// 对于游戏基本的设置进行读写
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// 保存内存里的基本设置内容
        /// </summary>
        public static BasicSettings BasicSettingsContent = new BasicSettings(0.80f, 0.80f);

        /// <summary>
        /// 用于读写文件，包含要读写的文件名路径
        /// </summary>
        public static BasicYamlIO BasicSettingIO
        {
            get { return _basicSetting; }
        }

        /// <summary>
        /// 用于读写文件，包含要读写的文件名路径
        /// </summary>
        private static readonly BasicYamlIO _basicSetting =
            new BasicYamlIO(fileName: "BasicSettings", note: "# 游戏基本设置");


        /// <summary>
        /// 读取基础设置文件
        /// </summary>
        public static void ReadSettings()
        {
            BasicSettingsContent = YamlReadWrite.Read(_basicSetting, BasicSettingsContent);
        }

        /// <summary>
        /// 设置文件写入文件
        /// </summary>
        public static void SaveSettings()
        {
            YamlReadWrite.Write(_basicSetting, BasicSettingsContent);
        }

        /// <summary>
        /// 初始化设置。内存+文件
        /// </summary>
        public static void InitializeSettings()
        {
            BasicSettingsContent = new BasicSettings(0.80f, 0.80f);
            SaveSettings();
        }


        /// <summary>
        /// 应用设置（从内存中读取设置）
        /// </summary>
        public static void ApplySettings()
        {
            QualitySettings.vSyncCount = BasicSettingsContent.sync ? 1 : 0;
            CameraReadAndApplySettings.ReadAndApply();
            Screen.SetResolution(BasicSettingsContent.resolution.width, BasicSettingsContent.resolution.height,
                BasicSettingsContent.fullscreenMode == FullScreenMode.FullScreenWindow, 60);
            if (PublicAudioSource.publicAudioSource != null) PublicAudioSource.publicAudioSource.UpdateMusicVolume();
            if (StaticVideoPlayer.staticVideoPlayer != null) StaticVideoPlayer.staticVideoPlayer.UpdateVol();
        }
    }
}