using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningCtrl : MonoBehaviour
{
    public Slider MusicVolSlider;
    public Slider EffectVolSlider;

    public CanvasGroup welcome;

    public UnityEvent initialization = new();

    private void Awake()
    {
       
        initialization.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        //按照文件调整滑块
        MusicVolSlider.value = Settings.SettingsContent.MusicVolume;
        EffectVolSlider.value = Settings.SettingsContent.SoundEffectVolume;

        //注册事件
        MusicVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.MusicVolume = arg0; PublicAudioSource.UpdateBackgroundMusicVolume();
            StaticVideoPlayer.UpdateVolume();
        });
        EffectVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.SoundEffectVolume = arg0;PublicAudioSource.UpdateSoundEffectVolume();
        });
        
        //welcome淡入
        StartCoroutine(WelcomeFadeIn());
    }

    IEnumerator WelcomeFadeIn()
    {
        welcome.alpha = 0f;

        yield return new WaitForSeconds(1.2f);
        while (true)
        {
            welcome.alpha = Mathf.Lerp(welcome.alpha, 1f, 0.51f * Time.deltaTime);
            if(welcome.alpha >= 0.99f) yield break;
            else yield return new WaitForEndOfFrame();
            
        }
    }
    

    public void ExitGame()
    {
        Settings.SaveSettings();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    
    public void StartGame(AudioClip clip)
    {
        PlaySoundEffect(clip);
     SceneManager.LoadScene("load");
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        PublicAudioSource.PlaySoundEffect(clip);
    }

 
}
