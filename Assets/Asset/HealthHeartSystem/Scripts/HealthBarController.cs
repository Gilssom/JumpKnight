using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private void Start()
    {
        heartContainers = new GameObject[BaseStat.m_PlayerStat.MaxHp];
        heartFills = new Image[BaseStat.m_PlayerStat.MaxHp];

        BaseStat.m_PlayerStat.onHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < BaseStat.m_PlayerStat.MaxHp)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < BaseStat.m_PlayerStat.Hp)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        if (BaseStat.m_PlayerStat.Hp % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(BaseStat.m_PlayerStat.Hp);
            heartFills[lastPos].fillAmount = BaseStat.m_PlayerStat.Hp % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < BaseStat.m_PlayerStat.MaxHp; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
