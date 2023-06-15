using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  유틸리티형 Resources Manager
 *  반복 재사용되는 코드들을 싱글톤 형식으로 한번에 묶어 사용
 *  인스턴스 생성으로 생기는 (Clone) 은 알아서 제거
 *  
 */
public class ResourcesManager : SingletomManager<ResourcesManager>
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if(prefab == null)
        {
            Debug.LogWarning($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        CloneDelete(go);

        return go;
    }

    public void CloneDelete(GameObject go)
    {
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
