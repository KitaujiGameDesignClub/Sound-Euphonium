using System;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Serialization;

namespace KitaujiGameDesignClub.GameFramework
{
    /// <summary>
    /// 如果要修改FileName或是添加其他IO用变量，请继承
    /// </summary>
    public class YamlReadWrite
    {

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
        /// 在规定的文件夹中写yaml文件
        /// </summary>
        /// <param name="profile">yamlIO文件设置</param>
        /// <param name="content">写入的内容</param>
        /// <typeparam name="T"></typeparam>
        public static void Write<T>(BasicYamlIO profile,T content) 
        {

            Serializer serializer = new Serializer();

            //得到最终呈现在文件中的文本内容
            string authenticContent = $"# Only for {Application.productName}\n# fileVersion:{BasicYamlIO.BasicYamlVersion}\n{profile.Note}\n{serializer.Serialize(content)}";

           
           
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            
            //创建文件夹
            Directory.CreateDirectory($"{UnityButNotAssets}/{profile.Path}");
            
            StreamWriter streamWriter =
                new StreamWriter($"{UnityButNotAssets}/{profile.Path}/{profile.FileName}.yaml", false, Encoding.UTF8);

#elif UNITY_ANDROID
            //创建文件夹
            Directory.CreateDirectory($"{Application.persistentDataPath}/{profile.Path}");

           StreamWriter streamWriter =
            new StreamWriter($"{Application.persistentDataPath}/{profile.Path}/{profile.FileName}.yaml", false,
                Encoding.UTF8);
#endif
            streamWriter.Write(authenticContent);
            streamWriter.Dispose();
            streamWriter.Close();
     



        }

        /// <summary>
        /// 读取yaml
        /// </summary>
        /// <param name="yaml"></param>
        /// <param name="content">读取文件的内容（作为默认值）</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Read<T>(BasicYamlIO yaml,T content)
        {
           Deserializer deserializer = new();
        
            //存在的话就读取
            StreamReader streamReader = StreamReader.Null;

        
            //尝试yaml文件
            try
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                streamReader =
                    new StreamReader($"{UnityButNotAssets}/{yaml.Path}/{yaml.FileName}.yaml", Encoding.UTF8);
#elif UNITY_ANDROID
           streamReader =
                new StreamReader($"{Application.persistentDataPath}/{yaml.Path}/{yaml.FileName}.yaml", Encoding.UTF8);
#endif
            
                var fileContent = deserializer.Deserialize<T>(streamReader.ReadToEnd());
                streamReader.Dispose();
                streamReader.Close();
                Debug.Log($"成功加载：{yaml.Path}/{yaml.FileName}.yaml");
                return fileContent;
           
            }
            catch (Exception)
            {

                //关闭之前的文件流，防止出现IOException: Sharing violation错误
                streamReader.Dispose();
                streamReader.Close();
            
                //不存在的话，初始化一个
                Debug.Log($"{yaml.Path}中不存在合规的{yaml.FileName}.yaml，已经初始化此文件");
                BasicYamlIO newFile = new BasicYamlIO(yaml.FileName,yaml.Path,yaml.Note);
                Write(newFile,content);
                return content;
            }
        
        }


        /// <summary>
        /// 检查是否存在所需的游戏文件夹，不存在则创建
        /// <param name="path">从根目录开始的路径，与saves同级，开头结尾不能有“ / ”</param>
        /// </summary>
        static void CheckAndCreateDirectory(string path)
        {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
            if (!Directory.Exists($"{UnityButNotAssets}/{path}"))
            {
                Directory.CreateDirectory($"{UnityButNotAssets}/{path}");
            }
  
#elif UNITY_ANDROID

          if (!Directory.Exists($"{Application.persistentDataPath}/{path}"))
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/{path}");
        }
#endif
       
       
        }


        /// <summary>
        /// 【PR标记点专用】将有好的时间线转化为电脑可以用的（视频帧数） 
        /// </summary>
        public static int ConvertFriendlyToReadable(int videoFps, string friendlyContent,int lag)
        {
            //00:00:00:00
            string[] fix = friendlyContent.Split(':');
            return int.Parse(fix[3]) + int.Parse(fix[2]) * videoFps + int.Parse(fix[1]) * 60 * videoFps + lag;
        }


    }
}