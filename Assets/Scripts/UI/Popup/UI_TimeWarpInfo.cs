using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeWarpInfo : UI_Popup
{
    enum Images
    {
        TimeImage,
    }

    enum Texts
    {
        TitleText,
        InfoText,
    }

    private float m_AlphaSpeed;
    private float m_DestroyTime;

    private Color m_color1;
    private Color m_color2;
    private Color m_color3;

    public override void Init()
    {
        base.Init();

        m_AlphaSpeed = 0.3f;
        m_DestroyTime = 3f;

        Invoke("DestroyObject", m_DestroyTime);

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        m_color1 = GetImage((int)Images.TimeImage).color;
        m_color2 = GetText((int)Texts.TitleText).color;
        m_color3 = GetText((int)Texts.InfoText).color;
    }

    void Update()
    {
        m_color1.a = Mathf.Lerp(m_color1.a, 0, Time.deltaTime * m_AlphaSpeed);
        m_color2.a = Mathf.Lerp(m_color2.a, 0, Time.deltaTime * m_AlphaSpeed);
        m_color3.a = Mathf.Lerp(m_color3.a, 0, Time.deltaTime * m_AlphaSpeed);

        GetImage((int)Images.TimeImage).color = m_color1;
        GetText((int)Texts.TitleText).color = m_color2;
        GetText((int)Texts.InfoText).color = m_color3;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
