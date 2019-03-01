

namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class Util
    {
        #region 查找子物体相关
        //查找子物体
        public static T FindObject<T>(Transform parent, string name) where T : UnityEngine.Object
        {
            Transform obj = GetChild(parent, name);
            if (obj != null)
            {
                if (typeof(T).Equals(typeof(UnityEngine.GameObject)))
                    return obj.gameObject as T;
                if (typeof(T).Equals(typeof(UnityEngine.Transform)))
                    return obj as T;
                return obj.gameObject.GetComponent<T>();
            }
            return null;
        }
        static Transform GetChild(Transform parent, string name)
        {
            if (parent.gameObject.name == name)
                return parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform curr = GetChild(parent.GetChild(i), name);
                if (curr != null && curr.gameObject.name == name)
                    return curr;
            }
            return null;
        }
        #endregion

        #region 销毁物体 相关
        public static void DestroyAllChildObject(Transform parent, float delayTime = 0.0f)
        {
            if(parent == null) return;
            int childCount = parent.childCount;
            for(int i = 0; i < childCount; i++)
            {
                Transform childTrans = parent.GetChild(i);
                UnityEngine.GameObject.Destroy(childTrans.gameObject, delayTime);
            }
        }
        #endregion

        #region 单例 相关
        //创建单例
        public static T GetInstance<T>(ref T instance, string name, bool isDontDestroy = true) where T : UnityEngine.Object
        {
            if (instance != null) return instance;
            if (GameObject.FindObjectOfType<T>() != null)
            {
                instance = GameObject.FindObjectOfType<T>();
                return instance;
            }
            GameObject go = new GameObject(name, typeof(T));
            if (isDontDestroy) UnityEngine.Object.DontDestroyOnLoad(go);
            instance = go.GetComponent(typeof(T)) as T;
            return instance;
        }
        #endregion

        #region 平台路径 相关
        //各平台下路径
        public static string DeviceResPath()
        {
            switch (GameConfig.gamePlatform)
            {
                case GamePlatform.GamePlatform_Editor:
                    return string.Format("{0}/", Application.dataPath);
                case GamePlatform.GamePlatform_PC:
                    return string.Format("{0}/", Application.streamingAssetsPath);
                case GamePlatform.GamePlatform_Mobbile:
                    return string.Format("{0}/", Application.persistentDataPath);
            }
            return string.Format("{0}/", Application.dataPath);
        }
        #endregion

        #region 文件目录相关
        //获取文件目录下,所有的文件路径
        public static void Recursive(string path, ref List<string> list)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".cs") || files[i].Name.EndsWith(".meta") || files[i].Name.EndsWith(".json"))
                    {
                        continue;
                    }
                    list.Add(files[i].FullName);
                }
            }
        }
        #endregion

    }
}
