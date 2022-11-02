using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KitaujiGameDesignClub.GameFramework.UI
{
    public class OpeningCtrl : MonoBehaviour
    {
        

        public AudioClip bgm;

        public UnityEvent initialization = new();

        public GameObject welcome;
        public GameObject tutorial;
        public GameObject staff;
        public GameObject setting;

        public TMP_Text Version;

        public virtual void Awake()
        {
            Time.timeScale = 1f;

            Application.targetFrameRate = -1;

            initialization.Invoke();

            welcome.SetActive(true);
            tutorial.SetActive(false);
            staff.SetActive(false);
            setting.SetActive(false);
            Version.text = Application.version;
            Settings.ApplySettings();
            
            Debug.Log("初始化完成");
        }

        // Start is called before the first frame update
        public virtual void Start()
        {
            
            //播放BGM
            if (PublicAudioSource.publicAudioSource != null)
                PublicAudioSource.publicAudioSource.PlayBackgroundMusic(bgm);
        }
    }
    
    
    
}