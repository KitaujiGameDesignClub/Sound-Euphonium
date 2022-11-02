using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Pack : IPreprocessBuildWithReport
{
  
    public int callbackOrder { get; }
    public void OnPreprocessBuild(BuildReport report)
    {
        //编辑器将显示设置设为默认值
        //公共
        Application.targetFrameRate = 60;
        //standalone
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 720;
        PlayerSettings.resizableWindow = false;
        PlayerSettings.defaultIsNativeResolution = true;
        PlayerSettings.allowFullscreenSwitch = true;
        PlayerSettings.SetAspectRatio(AspectRatio.Aspect4by3,false);
        PlayerSettings.SetAspectRatio(AspectRatio.Aspect5by4,false);
        PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by10,false);
        PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9,true);
        PlayerSettings.SetAspectRatio(AspectRatio.AspectOthers,false);
        PlayerSettings.runInBackground = false;
        //android
        PlayerSettings.Android.renderOutsideSafeArea = false;
        PlayerSettings.Android.fullscreenMode = FullScreenMode.FullScreenWindow;
        PlayerSettings.Android.resizableWindow = false;
        PlayerSettings.Android.optimizedFramePacing = true;
        PlayerSettings.Android.maxAspectRatio = 1.86f;//16:9
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.useAnimatedAutorotation = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
    }
}
