using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Serialization;

public static class YamlReadWrite 
{
    public enum FileName
    {
        Settings,
        PsitonsAction,
    }
 
  
  
  /// <summary>
  /// Assets上一级的目录（结尾没有/）
  /// </summary>
  /// <returns></returns>
  public static string UnityButNotAssets
  {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
      get
      {
          string[] raw = Application.dataPath.Split("/");

          string done = string.Empty;
          for (int i = 1; i < raw.Length - 1; i++)
          {
              done = $"{done}/{raw[i]}";
          }

          return done;
      }

#elif  UNITY_ANDROID
        get
        {
            return Application.persistentDataPath;
        }
   
        
#endif
      
  }

/// <summary>
/// 外部写yaml文件
/// </summary>
/// <param name="content">内容</param>
/// <param name="fileName">文件名</param>
/// <param name="notes">注释（按照yaml规范书写）</param>
/// <typeparam name="T"></typeparam>
  public static void Write<T>(T content, FileName fileName,string notes = null)
  {

      Serializer serializer = new Serializer();

      //把注释写入的内容
      string authenticContent = $"{notes}\n{serializer.Serialize(content)}";

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
      StreamWriter streamWriter =
          new StreamWriter($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", false, Encoding.UTF8);

#elif UNITY_ANDROID
           StreamWriter streamWriter =
            new StreamWriter($"{Application.persistentDataPath}/saves/{fileName.ToString()}.yaml", false,
                Encoding.UTF8);
#endif
      streamWriter.Write(authenticContent);
      streamWriter.Dispose();
      streamWriter.Close();




  }

/// <summary>
/// 外部读取yaml
/// </summary>
/// <param name="fileName">文件名</param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public static T Read<T>(FileName fileName)
{
    Deserializer deserializer = new();
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    StreamReader streamReader =
        new StreamReader($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", Encoding.UTF8);
    var content = deserializer.Deserialize<T>(streamReader.ReadToEnd());
    streamReader.Dispose();
    streamReader.Close();
    return content;
#elif UNITY_ANDROID
            if (fileName == FileName.Settings)
        {
            StreamReader streamReader =
                new StreamReader($"{Application.persistentDataPath}/saves/{fileName.ToString()}.yaml", Encoding.UTF8);
            var content = deserializer.Deserialize<T>(streamReader.ReadToEnd());
            streamReader.Dispose();
            streamReader.Close();
            return content;
        }
        //除了设置文件，其他的yaml都从res文件夹中读取
        else
        {
            return deserializer.Deserialize<T>(Resources.Load($"saves/{fileName.ToString()}").ToString());
        }

#endif


}

/// <summary>
/// Res文件夹读取yaml
/// </summary>
/// <param name="fileName"></param>
/// <typeparam name="T"></typeparam>
/// <returns></returns>
public static T ReadFromResources<T>(FileName fileName)
{
    Deserializer deserializer = new(); 
    var res = Resources.Load<TextAsset>($"saves/{fileName.ToString()}").ToString();
    return deserializer.Deserialize<T>(res);
}

 
/// <summary>
/// 检查是否存在所需的游戏文件夹，不存在则创建
/// </summary>
public static void CheckAndCreateDirectory()
{
#if UNITY_EDITOR || UNITY_EDITOR_WIN
    if (!Directory.Exists($"{UnityButNotAssets}/saves"))
    {
        Directory.CreateDirectory($"{UnityButNotAssets}/saves");
    }
  
        
#elif UNITY_ANDROID

          if (!Directory.Exists($"{Application.persistentDataPath}/saves"))
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/saves");
        }
#endif
       
       
}



    #region yaml用的各种结构体（类）
    /// <summary>
    /// 储存两位路人王时间的结构体
    /// </summary>
    [Serializable]
    public struct PsitonsAction
    {
        public bool DebugMode;
        public List<string> Psiton1;
        public List<string> Psiton2;
        public List<string> Psiton3;
    }
    
    /// <summary>
    /// 设置的内容（唯一一个永远是外部储存的）
    /// </summary>
    [Serializable]
    public struct SettingsContent
    {
        public  float MusicVolume;
        public  float SoundEffectVolume;
        public int lag;

    }
    
    
    

    #endregion
}
