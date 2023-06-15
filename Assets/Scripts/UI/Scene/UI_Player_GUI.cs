using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player_GUI : UI_Scene
{
    public enum GameObjects
    {
        
    }

    enum Texts
    {
        Score,
        ComboText,
        CoinText,
        StageText,
        EnemyText,
        PlayerText,
    }

    private Color alpha;
    public bool ComboReset = false;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        alpha = GetText((int)Texts.ComboText).color;
        GetText((int)Texts.ComboText).gameObject.SetActive(false);
    }

    void Update()
    {
        if(GameManager.Instance.m_Player.m_ComboScore > 2)
            SetCombo();

        if (ComboReset)
            ResetCombo();

        SetStage(GameManager.Instance.m_CurStage);
    }

    public void SetScore(int score)
    {
        GetText((int)Texts.Score).text = string.Format("{0:#,###}", score);
        GetText((int)Texts.CoinText).text = string.Format("{0:#,###}", BaseStat.m_PlayerStat.Gold);
    }

    void SetCombo()
    {
        GetText((int)Texts.ComboText).gameObject.SetActive(true);

        GetText((int)Texts.ComboText).gameObject.transform.rotation = Camera.main.transform.rotation;

        transform.Translate(new Vector3(0, 1f * Time.deltaTime, 0));

        GetText((int)Texts.ComboText).text = $"Combo {GameManager.Instance.m_Player.m_ComboScore}";

        if (alpha.a <= 0)
        {
            GetText((int)Texts.ComboText).gameObject.SetActive(false);
            ComboReset = false;
        }
    }

    public void ResetCombo()
    {
        GetText((int)Texts.ComboText).gameObject.SetActive(false);
    }

    public void SetStage(int stage)
    {
        GetText((int)Texts.StageText).text = $"Stage : {stage}";
        GetText((int)Texts.PlayerText).text = $"공격력 : {BaseStat.m_PlayerStat.Attack}";
        GetText((int)Texts.EnemyText).text = $"적 체력 : {stage * (stage + 3)}";
    }
}
