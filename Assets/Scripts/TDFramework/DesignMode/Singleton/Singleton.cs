

namespace TDFramework
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly object lockobj = new object();
        private static T m_instance = default(T);

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (lockobj)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new T();
                        }
                    }
                }
                return m_instance;
            }
        }
    }
}
