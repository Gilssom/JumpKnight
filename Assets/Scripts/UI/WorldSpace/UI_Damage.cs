using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damage : UI_Base
{
    enum Texts
    {
        Damage,
    }

    private float m_MoveSpeed;
    private float m_AlphaSpeed;
    private float m_DestroyTime;
    private Color alpha;
    private Text text;

    public int m_Damage;
    public bool m_Critical;
    public bool m_PlayerHit;
    public Transform m_Pos;

    public override void Init()
    {
        Bind<Text>(typeof(Texts));

        m_MoveSpeed = 3f;
        m_AlphaSpeed = 3f;
        m_DestroyTime = 1f;

        Invoke("DestroyObject", m_DestroyTime);

        Transform pos = m_Pos;

        transform.position = pos.position + Vector3.up;

        text = GetText((int)Texts.Damage);
        alpha = text.color;

        float RanXpos = Random.Range(-0.75f, 0.75f);
        transform.position = new Vector2(RanXpos, transform.position.y);
    }

    void Update()
    {
        GetDamage(m_Damage, m_Critical, m_PlayerHit);

        transform.rotation = Camera.main.transform.rotation;

        transform.Translate(new Vector3(0, m_MoveSpeed * Time.deltaTime, 0));

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * m_AlphaSpeed);

        text.color = alpha;
    }

    void GetDamage(int damage, bool critical, bool m_PlayerHit = false)
    {
        if (m_PlayerHit)
        {
            alpha = new Color(198 / 255f, 0f, 0f);
        }
        else if(critical)
        {
            alpha = new Color(1f, 225 / 250f, 74 / 250f);
        }

        text.text = $"{damage}";
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
