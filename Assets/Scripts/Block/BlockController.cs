using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    List<Block> m_BlockList = new List<Block>();

    [SerializeField]
    private int m_BlockCount;

    private void Start()
    {
        SetStage();
        m_BlockCount = 10;
    }

    //Test
    void SetStage()
    {
        GameManager.Instance.m_PlayerGUI.ComboReset = false;
        m_BlockCount += GameManager.Instance.m_CurStage;

        for (int i = 0; i < m_BlockCount; i++)
        {
            if (m_BlockList.Count != 0)
            {
                GameObject newBlock = ResourcesManager.Instance.Instantiate("Block", gameObject.transform);
                newBlock.transform.position = new Vector2(0, m_BlockList.Last().transform.position.y +
                    newBlock.transform.localScale.y + 0.3f);

                m_BlockList.Add(newBlock.GetComponent<Block>());
            }
            else
            {
                GameObject newBlock = ResourcesManager.Instance.Instantiate("Block", gameObject.transform);
                newBlock.transform.position = new Vector2(0, transform.position.y);

                m_BlockList.Add(newBlock.GetComponent<Block>());
            }
        }
    }

    public void Broken()
    {
        m_BlockList.RemoveAt(0);

        Debug.Log($"남은 블럭 갯수 : {m_BlockList.Count}");

        if (m_BlockList.Count == 0)
        {
            Debug.Log("페이즈 클리어!!");
            GameManager.Instance.m_PlayerGUI.ComboReset = true;
            GameManager.Instance.m_CurStage++;
            Invoke("SetStage", 2.5f);
        }
    }

    public void ShieldCrush(float power)
    {
        for (int i = 0; i < m_BlockList.Count; i++)
        {
            m_BlockList[i].Shield(power);
        }
    }
}
