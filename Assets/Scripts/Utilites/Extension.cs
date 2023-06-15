using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*  유틸리티 코드
 *  GetOrAddComponet : Utilles 설명과 동일
 *  BindEvent : 원하는, 지정된 Event 를 등록
 *  IsValid : 해당 오브젝트가 있는지 없는지 확인
 *  
 */
public static class Extension
{
    public static T GetOrAddComponet<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utilles.GetOrAddComponet<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Defines.UIEvent type = Defines.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
