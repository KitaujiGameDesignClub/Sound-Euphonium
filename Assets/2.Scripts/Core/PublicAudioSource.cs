using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PublicAudioSource : MonoBehaviour
{
    
    private static AudioSource Music;
    private static AudioSource Effect;
 
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        AudioSource[] audio = GetComponents<AudioSource>();
        if (audio[0].playOnAwake)
        {
            Music = audio[0];
            Effect = audio[1];
        }
        else
        {
            Music = audio[1];
            Effect = audio[0];
        }
        
        //停止播放
        Music.Stop();
        
        
    }
    


    public static void UpdateMusicVolume()
    {
        Music.volume = Settings.SettingsContent.MusicVolume;
      
    }



    // Update is called once per frame
    public static void PlaySoundEffect(AudioClip clip)
    {
       Effect.PlayOneShot(clip,Settings.SettingsContent.SoundEffectVolume);
    }
    
    
}
