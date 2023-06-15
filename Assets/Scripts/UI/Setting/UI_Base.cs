using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*   ������ Ÿ�Կ� �ش�Ǵ� ������Ʈ���� Objects[] �� �������ش�.
 *   Bind �� ������ Ÿ�Կ� �´� UI ������Ʈ�� ������ ( ��, �ڽ� ������Ʈ�� ���� )
 *   Ÿ ��ũ��Ʈ������ 52 ~ 55�ٰ� ���� ����.
 */
public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> m_Objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_Objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utilles.FindChild(gameObject, names[i], true);

            else
                objects[i] = Utilles.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.LogError($"Failed to bind({name[i]})");
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (m_Objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index)         { return Get<Text>(index); }
    protected Button GetButton(int index)     { return Get<Button>(index); }
    protected Image GetImage(int index)       { return Get<Image>(index); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_EvnetHandler evt = Utilles.GetOrAddComponet<UI_EvnetHandler>(go);

        switch (type)
        {
            case Defines.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Defines.UIEvent.Drop:
                evt.OnDropHandler -= action;
                evt.OnDropHandler += action;
                break;
            case Defines.UIEvent.BeginDrag:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case Defines.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Defines.UIEvent.EndDrag:
                evt.OnEndDragHandler -= action;
                evt.OnEndDragHandler += action;
                break;
            case Defines.UIEvent.Enter:
                evt.OnEnterHandler -= action;
                evt.OnEnterHandler += action;
                break;
            case Defines.UIEvent.Exit:
                evt.OnExitHandler -= action;
                evt.OnExitHandler += action;
                break;
        }
    }
}
