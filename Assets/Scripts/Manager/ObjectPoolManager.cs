using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  오브젝트 풀링
 *  인스턴스 생성 > Destroy 삭제 형식으로 반복 재사용되는 객체를
 *  한번에 묶어 SetActive true > false 형식으로 부담을 덜어주는 함수
 */

[System.Serializable]
public class ObjectsInfo
{
    public string m_ObjectName;
    public GameObject m_Prefab;
    public int m_Count;
    public GameObject m_Parent;
}

public class ObjectPoolManager : SingletomManager<ObjectPoolManager>
{
    public GameObject Pooling
    {
        get
        {
            GameObject root = GameObject.Find("@@##_Object_Pooling");
            if (root == null)
                root = new GameObject { name = "@@##_Object_Pooling" };
            return root;
        }
    }

    [SerializeField]
    ObjectsInfo[] m_ObjectInfos = null;

    public List<Queue<GameObject>> m_ObjectPoolList;

    void Start()
    {
        m_ObjectPoolList = new List<Queue<GameObject>>();

        if (m_ObjectInfos != null)
            for (int i = 0; i < m_ObjectInfos.Length; i++)
            {
                m_ObjectPoolList.Add(InsertQueue(m_ObjectInfos[i]));
            }
    }

    Queue<GameObject> InsertQueue(ObjectsInfo prefab_objectInfo, GameObject parent = null)
    {
        Queue<GameObject> m_Queue = new Queue<GameObject>();

        for (int i = 0; i < prefab_objectInfo.m_Count; i++)
        {
            GameObject objectClone = Instantiate(prefab_objectInfo.m_Prefab) as GameObject;

            objectClone.SetActive(false);

            if(prefab_objectInfo.m_Parent)
                objectClone.transform.SetParent(prefab_objectInfo.m_Parent.transform);
            else
                objectClone.transform.SetParent(Pooling.transform);

            int index = objectClone.name.IndexOf("(Clone)");
            if (index > 0)
                objectClone.name = objectClone.name.Substring(0, index);

            m_Queue.Enqueue(objectClone);
        }

        return m_Queue;
    }

    public IEnumerator DestroyObj(float Seconds, int PoolNumber, GameObject Object)
    {
        yield return new WaitForSeconds(Seconds);
        m_ObjectPoolList[PoolNumber].Enqueue(Object);
        Object.SetActive(false);
    }
}
