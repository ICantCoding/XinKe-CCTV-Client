

namespace TDFramework.Utils.Ini
{
	using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IniHelper
    {
		#region 字段和属性
        private static WWW www;
        private static bool isWwwDone = false;
        public static bool IsWwwDone
        {
            get
            {
                if (www != null && www.isDone)
                {
                    Load(www.bytes);
                    isWwwDone = true;
                    www = null;
                    return isWwwDone;
                }
                return isWwwDone;
            }
        }
		protected static Dictionary<string, Dictionary<string, string>> mDictionary =
		 new Dictionary<string, Dictionary<string, string>>();
		#endregion

		#region 方法
        //使用WWW获取加载ini文件
        public static void LoadConfigFromWWW(string configPath)
        {
            if (isWwwDone)
                return;
            if (www == null)
            {
#if UNITY_EDITOR
                www = new WWW("file://" + Application.dataPath + "/StreamingAssets/" + configPath);
#elif UNITY_STANDALONE_WIN
				www = new WWW("file://" + Application.dataPath + "/StreamingAssets/" + configPath);
#elif UNITY_IPHONE
				www = new WWW("file://" + Application.dataPath + "/Raw/" + configPath);	
#elif UNITY_ANDROID
				www = new WWW("jar:file://" + Application.dataPath + "!/assets/" + configPath);
#endif
            }
        }
        //使用File加载ini文件
        public static void LoadConfigFromStreamingAssets(string configPath)
        {
            string configFilePath = Application.streamingAssetsPath + "/" + configPath;
            byte[] bytes = File.ReadAllBytes(configFilePath);
            Load(bytes);
        }
        //从bytes中读取
        public static void LoadConfigFromBytes(byte[] bytes)
        {
            Load(bytes);
        }
        static void Load(byte[] bytes)
        {
            IniFileReader reader = new IniFileReader(bytes);
            mDictionary = reader.ReadDictionary();
        }
        public static string Get(string mainKey, string subKey)
        {
            if (mDictionary.ContainsKey(mainKey) && mDictionary[mainKey].ContainsKey(subKey))
                return mDictionary[mainKey][subKey];

            return mainKey + "." + subKey;
        }
        public static Dictionary<string, string> Get(string mainKey)
        {
            if (mDictionary.ContainsKey(mainKey))
                return mDictionary[mainKey];
            return null;
        }
        public static int GetInt(string mainKey, string subKey)
        {
            int ret;
            int.TryParse(Get(mainKey, subKey), out ret);
            return ret;
        }
        public static float GetFloat(string mainKey, string subKey)
        {
            float ret;
            float.TryParse(Get(mainKey, subKey), out ret);
            return ret;
        }
        public static string GetContent(string mainKey, string subKey)
        {
            string ret = Get(mainKey, subKey);
            if (ret.StartsWith("\"")) ret = ret.Substring(1, ret.Length - 1);
            if (ret.EndsWith(";")) ret = ret.Substring(0, ret.Length - 2);
            if (ret.EndsWith("\"")) ret = ret.Substring(0, ret.Length - 2);
            return ret;
        }
		#endregion
    }
}