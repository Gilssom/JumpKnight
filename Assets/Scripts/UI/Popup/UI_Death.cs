using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Death : UI_Popup
{
    enum Texts
    {
        ScoreText,
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));

        string str = string.Format("{0:#,###}", GameManager.Instance.m_Score);
        GetText((int)Texts.ScoreText).text = $"Á¡¼ö : {str}";
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
