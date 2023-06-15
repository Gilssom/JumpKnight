using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*  시네머신 카메라를 이용한 카메라 움직임
 * 
 */
public class CameraShake : SingletomManager<CameraShake>
{
    [SerializeField]
    private CinemachineVirtualCamera m_FreeLook;

    [SerializeField]
    private float m_ShakerTimer;

    void Awake()
    {
        m_FreeLook = GetComponent<CinemachineVirtualCamera>();
    }

    public void CameraShaking(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin
            = m_FreeLook.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        m_ShakerTimer = time;
    }

    void Update()
    {
        if (m_ShakerTimer > 0)
        {
            m_ShakerTimer -= Time.deltaTime;
            if (m_ShakerTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin
                    = m_FreeLook.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
