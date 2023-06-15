using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PowerGage : UI_Scene
{
    public enum GameObjects
    { 
        PowerGageSlider
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }

    void Update()
    {
        float powerRatio = GameManager.Instance.m_Player.m_JumpPower;

        SetRatio(GameObjects.PowerGageSlider, powerRatio);
    }

    public void SetRatio(GameObjects objects, float ratio)
    {
        GetObject((int)objects).GetComponent<Slider>().value = ratio;
    }
}
