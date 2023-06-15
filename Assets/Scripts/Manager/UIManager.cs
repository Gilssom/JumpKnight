using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  UI Manager
 *  화면에 띄워지는 UI 는 Scene, Popup, WorldSpace 3가지로 나뉨
 *  
 *  1. Scene UI :: 바로 생성, Player GUI 처럼 인게임에서 계속 나타내는 UI 라 Sort 는 겹치지 않게 설계하기 때문
 *  2. Popup UI :: 팝업창은 마지막에 띄워진 창이 앞으로 와야하고, 먼저 삭제해야 하기 때문에 Stack Container 로 관리
 *  3. WorldSpace UI :: 바로 생성, Prefabs 설계 시 WorldSpace 로 제작하고, 호출하는 부분에서 위치 값만 조절
 * 
 *  모든 UI 는 UI_Root 오브젝트에 모여서 관리하게 됨 => 23번째 줄
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

    #region #World Space 폴더 안의 UI Prefab 불러오기
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

    #region #Sub Item 폴더 안의 UI Prefab 불러오기
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

    #region #Scene 폴더 안의 UI Prefab 불러오기
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

    #region #Popup 폴더 안의 UI Prefab 불러오기
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

    #region #UI 닫기 Method들
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
