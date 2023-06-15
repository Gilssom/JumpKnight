using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  싱글톤 매니저
 *  싱글톤을 사용할려면 초입부에 많은 줄의 코드를 작성해야함.
 *  하지만 너무 반복적인 부분이라 따로 꺼내어서 사용하기로 설계함.
 *  성능적인 부분에서는 모르겠으나 코드의 가독성 및 편리함을 위해 제작.
 */

// public class MyClassName : Singleton<MyClassName> {}
public class SingletomManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    if (m_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_Instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}
