﻿using Devil.Utility;

namespace Devil
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T mInstance;
#if UNITY_EDITOR
        private static bool mNewInstance;
#endif

        protected Singleton()
        {
#if UNITY_EDITOR
            if (!mNewInstance)
            {
                RTLog.LogWarning(LogCat.Game, string.Format("{0} 被定义为一个单例类，请使用.Instance 或者 .GetOrNewInstance() 方法替换 new 方法。", typeof(T)));
            }
#endif
        }

        public static bool HasInstance { get { return mInstance != null; } }

        public static T Instance
        {
            get { return mInstance; }
        }

        public static T GetOrNewInstance()
        {
            if (mInstance == null)
            {
#if UNITY_EDITOR
                mNewInstance = true;
#endif
                mInstance = new T();
                mInstance.OnInit();
#if UNITY_EDITOR
                mNewInstance = false;
                RTLog.LogFormat(LogCat.Game, "Instance Singleton<{0}>.", mInstance.GetType());
#endif
            }
            return mInstance;
        }

        public static void Release()
        {
            if (mInstance != null)
            {
                mInstance.OnDestroy();
#if UNITY_EDITOR
                RTLog.LogFormat(LogCat.Game, "Release Singleton<{0}>.", mInstance.GetType());
#endif
                mInstance = null;
            }
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnDestroy()
        {
            
        }
    }
}