
/*
    AssetBundle打包， 按照指定目录，和指定目录下的Prefab方式打包，自动生成自己的依赖文件
    打包路径Application.streamingAssetPath路径下
 */

namespace TDFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Xml.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using TDFramework.Utils;

    public class ABConfig : ScriptableObject
    {
        //基于文件夹下所有单个文件进行打包, 针对的是Prefab, 该文件夹下Prefab的名字不能重复,必须保证名字的唯一性
        public List<string> prefabABPathList = new List<string>(); //所有的Prefab对应的AB包的路径
        public List<DirectoryAB> directoryABPathList = new List<DirectoryAB>(); //所有的文件夹对应的AB包路径

        [System.Serializable]
        public struct DirectoryAB
        {
            public string ABName;           //文件夹打包后的AB包名
            public string DirectoryPath;    //文件夹的路径                 
        }
    }

    public class PackageAssetBundle
    {
        //Key为打包目录的AssetBundleName, Value为打包目录Assets/相对路径
        private static Dictionary<string, string> m_directoryABDict = new Dictionary<string, string>();
        //Key为预制件的AssetBundleName, Value为该预制件依赖的所有资源文件Assets/相对路径
        private static Dictionary<string, List<string>> m_prefabABDict = new Dictionary<string, List<string>>();
        private static List<string> m_buildedFiles = new List<string>(); //记录已经被标记了要打包的文件, 用于过滤，避免打包到重复的文件
        private static List<string> m_loadFiles = new List<string>(); //记录哪些会被动态加载的文件

        [MenuItem("Tools/AssetBundle/配置文件生成(推荐)", false, 1)]
        private static void ProduceAssetBundleConfig()
        {
            ABConfig abConfig = new ABConfig();
            abConfig.directoryABPathList.Add(new ABConfig.DirectoryAB()
            {
                ABName = "assetbundleconfig",
                DirectoryPath = "Assets/Scripts/TDFramework/ResourcesLoad/AssetBundleHelper/Config"
            });
            UnityEditor.AssetDatabase.CreateAsset(abConfig, ABPathConfig.AssetBundleConfigPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        [MenuItem("Tools/AssetBundle/打包(推荐)", false, 2)]
        private static void BuildAssetBundle()
        {

            m_directoryABDict.Clear();
            m_prefabABDict.Clear();
            m_buildedFiles.Clear();
            m_loadFiles.Clear();

            //创建打包目的目录
            if (!Directory.Exists(ABPathConfig.AssetBundleBuildTargetPath))
            {
                Directory.CreateDirectory(ABPathConfig.AssetBundleBuildTargetPath);
            }

            //读取打包策略对应的配置文件
            ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABPathConfig.AssetBundleConfigPath);

            //获取整体目录打包的文件夹
            foreach (ABConfig.DirectoryAB item in abConfig.directoryABPathList)
            {
                if (m_directoryABDict.ContainsKey(item.ABName))
                {
                    Debug.Log("当前文件夹: " + item.DirectoryPath + "的AB包名字重复, 请检查!");
                }
                else
                {
                    m_directoryABDict.Add(item.ABName, item.DirectoryPath);
                    m_buildedFiles.Add(item.DirectoryPath); //文件夹是会被打包的，所以需要加入
                    m_loadFiles.Add(item.DirectoryPath); //文件夹中的资源是可能被动态加载的, 视频,音频,texture等资源
                }
            }

            //获取需要打包的Prefab的文件路径, 得到GUID, GUID可以转Path
            string[] allPrefabGUIDAry = AssetDatabase.FindAssets("t:Prefab", abConfig.prefabABPathList.ToArray());
            foreach (string guid in allPrefabGUIDAry)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                m_loadFiles.Add(path); //Prefab预制件是一定会被动态加载的
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] goDependencies = AssetDatabase.GetDependencies(path); //得到某个路径下资源需要依赖的所有文件全路径，包括资源自身的路径在内
                List<string> goDependenciesPathList = new List<string>();
                for (int i = 0; i < goDependencies.Length; i++)
                {
                    if (goDependencies[i].EndsWith(".cs") == false && BuildedFilesContainFile(goDependencies[i]) == false) //过滤掉.cs脚本和已经被标记打包的文件
                    {
                        goDependenciesPathList.Add(goDependencies[i]);
                        m_buildedFiles.Add(goDependencies[i]);
                    }
                }
                if (m_prefabABDict.ContainsKey(go.name))
                {
                    Debug.Log("当前Prefab: " + go.name + "的AB包名字重复, 请检查!");
                }
                else
                {
                    m_prefabABDict.Add(go.name, goDependenciesPathList);
                }
            }

            //对所有的整体文件夹设置AssetBundleName
            foreach (var item in m_directoryABDict)
            {
                SetAssetBundleName(item.Key, item.Value);
            }
            //对所有的指定文件中的Prefab设置AssetBundleName
            foreach (var item in m_prefabABDict)
            {
                foreach (string filePath in item.Value)
                {
                    SetAssetBundleName(item.Key, filePath);
                }
            }

            //制作依赖关系
            MakeDependenceRelationShip();

            //开始打包
            BuildPipeline.BuildAssetBundles(ABPathConfig.AssetBundleBuildTargetPath,
                BuildAssetBundleOptions.ChunkBasedCompression,
                EditorUserBuildSettings.activeBuildTarget);

            //打包成功后, 需要取消项目中的AssetBundleName的标记
            string[] assetBundleNameAry = AssetDatabase.GetAllAssetBundleNames();
            foreach (string assetBundleName in assetBundleNameAry)
            {
                AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
            }

            //生成Md5文件, 目前, 我先把Md5File文件生成关闭了. 
            // CreateMd5File4AssetBundleFiles();

            //刷新Project面板
            AssetDatabase.Refresh();
            Debug.Log("打包成功...");
        }

        //为需要打包的文件进行AssetBundleName的设置
        private static void SetAssetBundleName(string abName, string filePath)
        {
            AssetImporter importer = AssetImporter.GetAtPath(filePath);
            if (importer == null)
            {
                Debug.Log("文件: " + filePath + "被标记AssetBundleName时, 找不到该文件路径!");
            }
            else
            {
                importer.assetBundleName = abName;
            }
        }

        //制作依赖关系
        private static void MakeDependenceRelationShip()
        {
            string[] assetBundleNameAry = AssetDatabase.GetAllAssetBundleNames(); //得到所有的AssetBundle的名字
            Dictionary<string, string> dict = new Dictionary<string, string>(); //记录需要被加载的资源
            Dictionary<string, string> allDict = new Dictionary<string, string>(); //记录所有的资源
            for (int i = 0; i < assetBundleNameAry.Length; i++)
            {
                string[] assetBundleAllFiles = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleNameAry[i]); //得到某个AssetBundle下的所有被标记的文件
                for (int j = 0; j < assetBundleAllFiles.Length; j++)
                {
                    if(assetBundleAllFiles[j].EndsWith(".cs") == false){
                        allDict.Add(assetBundleAllFiles[j], assetBundleNameAry[i]);
                    }
                    if (assetBundleAllFiles[j].EndsWith(".cs") == true || CanBeLoadByDynamic(assetBundleAllFiles[j]) == false) //生成依赖文件，我们只生成那些有可能会被实例化的资源，那些不可能被实例化的资源不需要写入依赖文件中
                    {
                        continue;
                    }
                    dict.Add(assetBundleAllFiles[j], assetBundleNameAry[i]); //这个文件Assets/相对路径和对应这个文件所在的AB包名
                }
            }

            //删除已经没有的AssetBundle包文件
            DirectoryInfo directoryInfo = new DirectoryInfo(ABPathConfig.AssetBundleBuildTargetPath);
            FileInfo[] fileInfoAry = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in fileInfoAry)
            {
                bool isExists = false;
                foreach (string assetBundleName in assetBundleNameAry)
                {
                    if (assetBundleName == fileInfo.Name) //AssetBundle包名和打包时标记名一致
                    {
                        isExists = true;
                    }
                }
                if (isExists == false && File.Exists(fileInfo.FullName))
                {
                    Debug.Log(fileInfo.Name + "这个包已经不需要存在了, 删除中...");
                    File.Delete(fileInfo.FullName);
                }
            }

            //写依赖文件
            AssetBundleConfig config = new AssetBundleConfig();
            config.ABList = new List<ABBase>();
            foreach (string path in dict.Keys)
            {
                ABBase abBase = new ABBase();
                abBase.Path = path;
                abBase.Crc = CrcHelper.StringToCRC32(path);
                abBase.ABName = dict[path];
                abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
                abBase.DependABList = new List<string>();
                string[] resDependce = AssetDatabase.GetDependencies(path);
                for (int i = 0; i < resDependce.Length; i++)
                {
                    string tempPath = resDependce[i];
                    if (tempPath == path || path.EndsWith(".cs")) //排除对本身文件和.cs脚本文件
                        continue;
                    string abName = "";
                    if(allDict.TryGetValue(tempPath, out abName)){
                        if(abName == allDict[path])
                        {
                            continue;
                        }
                        if(!abBase.DependABList.Contains(abName)){
                            abBase.DependABList.Add(abName);
                        }
                    }
                }
                config.ABList.Add(abBase);
            }
            //写入xml
            if (File.Exists(ABPathConfig.AssetBundleDependenceXmlPath))
            {
                File.Delete(ABPathConfig.AssetBundleDependenceXmlPath);
            }
            FileStream fileStream = new FileStream(ABPathConfig.AssetBundleDependenceXmlPath,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            XmlSerializer xs = new XmlSerializer(config.GetType());
            xs.Serialize(sw, config);
            sw.Close();
            fileStream.Close();

            //写入二进制
            foreach (ABBase item in config.ABList)
            {
                item.Path = "";
            }
            if (File.Exists(ABPathConfig.AssetBundleDependenceBytePath))
            {
                File.Delete(ABPathConfig.AssetBundleDependenceBytePath);
            }
            fileStream = new FileStream(ABPathConfig.AssetBundleDependenceBytePath,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fileStream, config);
            fileStream.Close();
        }

        //用于判断参数文件是否已经被标记进打包文件中了，true表示已经被标记了，false表示还没有被标记，需要标记该文件
        private static bool BuildedFilesContainFile(string filePath)
        {
            foreach (string path in m_buildedFiles)
            {
                if (filePath == path || (filePath.Contains(path) && (filePath.Replace(path, "")[0] == '/')))
                {
                    return true;
                }
            }
            return false;
        }

        //用于判断哪些文件是不需要代码动态加载使用的, 比如materials这些文件一般不会动态加载并使用,这些文件只提供对其他Prefab的使用
        private static bool CanBeLoadByDynamic(string filePath)
        {
            foreach (string loadFile in m_loadFiles)
            {
                if (filePath.Contains(loadFile))
                {
                    return true;
                }
            }
            return false;
        }

        //生成记录所有AssetBundle包的Md5文件
        private static void CreateMd5File4AssetBundleFiles()
        {
            try
            {
                List<string> list = new List<string>();
                Util.Recursive(ABPathConfig.AssetBundleBuildTargetPath, ref list);
                string outPath = Util.DeviceResPath() + AppConfig.Md5FilePath;
                if (File.Exists(outPath)) File.Delete(outPath);
                FileStream fs = new FileStream(outPath, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs);
                for (int i = 0; i < list.Count; i++)
                {
                    string file = list[i];
                    if (file.EndsWith(".manifest")) continue;
                    string md5 = Md5Helper.Md5File(file);
                    file = file.Replace("\\", "/");
                    file = file.Replace(Application.streamingAssetsPath + "/", "");
                    sw.WriteLine(file + "|" + md5);
                }
                fs.Flush();
                sw.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log("CreateMd5File4AssetBundleFiles error: " + e.Message);
            }
        }
    }
}


