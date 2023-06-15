using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  UI Manager
 *  ȭ�鿡 ������� UI �� Scene, Popup, WorldSpace 3������ ����
 *  
 *  1. Scene UI :: �ٷ� ����, Player GUI ó�� �ΰ��ӿ��� ��� ��Ÿ���� UI �� Sort �� ��ġ�� �ʰ� �����ϱ� ����
 *  2. Popup UI :: �˾�â�� �������� ����� â�� ������ �;��ϰ�, ���� �����ؾ� �ϱ� ������ Stack Container �� ����
 *  3. WorldSpace UI :: �ٷ� ����, Prefabs ���� �� WorldSpace �� �����ϰ�, ȣ���ϴ� �κп��� ��ġ ���� ����
 * 
 *  ��� UI �� UI_Root ������Ʈ�� �𿩼� �����ϰ� �� => 23��° ��
 */
public class UIManager : SingletomManager<UIManager>
{
    int m_Order = 10;

    [Header("Popup UI Open / Close")]
    public bool isInvenOpen, isBossHPOpen;

    Stack<UI_Popup> m_PopupStack = new Stack<UI_Popup>();
    UI_Scene m_SceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@@UI_Root");
            if (root == null)
                root = new GameObject { name = "@@UI_Root" };
            return root;
        }    
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utilles.GetOrAddComponet<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = m_Order;
            m_Order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    #region #World Space ���� ���� UI Prefab �ҷ�����
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponet<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Utilles.GetOrAddComponet<T>(go);
    }
    #endregion

    #region #Sub Item ���� ���� UI Prefab �ҷ�����
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Utilles.GetOrAddComponet<T>(go);
    }
    #endregion

    #region #Scene ���� ���� UI Prefab �ҷ�����
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utilles.GetOrAddComponet<T>(go);
        m_SceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    #endregion

    #region #Popup ���� ���� UI Prefab �ҷ�����
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourcesManager.Instance.Instantiate($"UI/Popup/{name}");
        T popup = Utilles.GetOrAddComponet<T>(go);
        m_PopupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }
    #endregion

    #region #UI �ݱ� Method��
    public void ClosePopupUI(UI_Popup popup)
    {
        if (m_PopupStack.Count == 0)
            return;

        if(m_PopupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (m_PopupStack.Count == 0)
            return;

        UI_Popup popup = m_PopupStack.Pop();
        ResourcesManager.Instance.Destroy(popup.gameObject);
        popup = null;
        m_Order--;
    }

    public void CloseAllPopupUI()
    {
        while (m_PopupStack.Count > 0)
            ClosePopupUI();
    }
    #endregion

    public void Clear()
    {
        CloseAllPopupUI();
        m_SceneUI = null;
    }
}
