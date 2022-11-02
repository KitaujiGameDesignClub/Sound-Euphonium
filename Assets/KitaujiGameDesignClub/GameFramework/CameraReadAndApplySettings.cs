using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace KitaujiGameDesignClub.GameFramework
{
    
    
    public class CameraReadAndApplySettings 
    {
        /// <summary>
        /// 读取内存中的设置，并应用
        /// </summary>
        /// <param name="camera">空，则为主相机</param>
        public static void ReadAndApply(Camera camera = null)
        {
            UniversalAdditionalCameraData cameraData = camera == null ? Camera.main.GetUniversalAdditionalCameraData():camera
                .GetUniversalAdditionalCameraData();

            cameraData.dithering = Settings.BasicSettingsContent.dithering;
            switch (Settings.BasicSettingsContent.antiAliasing)
            {
                case 0:
                    cameraData.antialiasing = AntialiasingMode.None;
                    break;
                case 1:
                    cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                    break;
                case 2:
                    cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    cameraData.antialiasingQuality = AntialiasingQuality.Low;
                    break;
                    
                case 3:
                    cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    cameraData.antialiasingQuality = AntialiasingQuality.Medium;
                    break;
                case 4:
                    cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    cameraData.antialiasingQuality = AntialiasingQuality.High;
                    break;
            }
        }
    }
}
