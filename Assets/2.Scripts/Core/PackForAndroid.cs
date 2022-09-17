#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
 
public class PackForAndroid : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
 
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log(File.ReadAllLines($"{YamlReadWrite.UnityButNotAssets}/Platform.ini")[0]);
     
        //1=Windows平台 0是Android平台
        if (File.ReadAllLines($"{YamlReadWrite.UnityButNotAssets}/Platform.ini")[0] == "1")
        {
           
            return;
        }

        //打包前，将外部目录下的文件，复制到Resources文件夹中
        Directory.CreateDirectory($"{Application.dataPath}/Resources/saves"); 
        DirectoryInfo directoryInfo = new DirectoryInfo($"{YamlReadWrite.UnityButNotAssets}/saves");
        FileInfo[] manifests = directoryInfo.GetFiles("*.yaml");
       
       //移动saves文件夹中所有的Yaml文件到saves文件夹中
       directoryInfo = new DirectoryInfo($"{YamlReadWrite.UnityButNotAssets}/saves");
       manifests = directoryInfo.GetFiles("*.yaml");
       for (int i = 0; i < manifests.Length; i++)
       {
           File.Copy(manifests[i].FullName,$"{Application.dataPath}/Resources/saves/{manifests[i].Name}");
       }
     
    }
 
    public void OnPostprocessBuild(BuildReport report)
    {
        //编译完了，删掉Resources文件夹
        DirectoryInfo directoryInfo = new DirectoryInfo($"{Application.dataPath}/Resources/saves");
        FileInfo[] manifests = directoryInfo.GetFiles();
        for (int i = 0; i < manifests.Length; i++)
        {
          File.Delete(manifests[i].FullName);
        }
       
        Directory.Delete($"{Application.dataPath}/Resources/saves");

    
    }
}
#endif
