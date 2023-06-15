using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevilSkill : UI_Popup
{
    enum Images
    {
        TimeImage,
    }

    private float m_AlphaSpeed;
    private float m_DestroyTime;

    private Color m_color1;

    public override void Init()
    {
        base.Init();

        m_AlphaSpeed = 0.3f;
        m_DestroyTime = 5f;

        Invoke("DestroyObject", m_DestroyTime);

        Bind<Image>(typeof(Images));

        m_color1 = GetImage((int)Images.TimeImage).color;
    }

    void Update()
    {
        m_color1.a = Mathf.Lerp(m_color1.a, 0, Time.deltaTime * m_AlphaSpeed);

        GetImage((int)Images.TimeImage).color = m_color1;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
