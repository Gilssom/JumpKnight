using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*  유틸리티의 Defines > Sound 의 목록을 추출.
 *  이후 경로 지정으로 해당 사운드를 가져와서 사용.
 * 
 *  AudioClip Casing 역할을 하기 위해 Dictionary 를 구축
 *  일반적으로 효과음 같은 경우는 자주 사용될 것이기 때문에 매 순간마다 Load 를 하기에는 부하가 올 수 있음.
 */
public class SoundManager : SingletomManager<SoundManager>
{
    AudioSource[] m_AudioSources = new AudioSource[(int)Defines.Sound.MaxCount];

    Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

    private bool isPlayingTrack01;

    public float m_BGMSound { get; private set; }
    public float m_SFXSound { get; private set; }

    public AudioMixerGroup[] m_AudioMixer = new AudioMixerGroup[2];

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        AudioMixer audioMixer = ResourcesManager.Instance.Load<AudioMixer>("Sounds/MyMixer");

        AudioMixerGroup[] audioMixGroupBGM = audioMixer.FindMatchingGroups("BGM");
        m_AudioMixer[0] = audioMixGroupBGM[0];
        AudioMixerGroup[] audioMixGroupSFX = audioMixer.FindMatchingGroups("SFX");
        m_AudioMixer[1] = audioMixGroupSFX[0];

        GameObject root = GameObject.Find("@Sound");

        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sound));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                m_AudioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;

                if (i == soundNames.Length - 2)
                {
                    m_AudioSources[i].outputAudioMixerGroup = m_AudioMixer[1];
                }
                else
                {
                    m_AudioSources[i].outputAudioMixerGroup = m_AudioMixer[0];
                }
            }

            m_AudioSources[(int)Defines.Sound.Bgm01].loop = true;
            m_AudioSources[(int)Defines.Sound.Bgm02].loop = true;
            isPlayingTrack01 = true;
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in m_AudioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        m_AudioClips.Clear();
    }

    public void Play(string path, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Defines.Sound type = Defines.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Defines.Sound.Bgm)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBgmTrack(audioClip, pitch));
            isPlayingTrack01 = !isPlayingTrack01;
        }
        else
        {
            AudioSource audioSource = m_AudioSources[(int)Defines.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Defines.Sound type = Defines.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Defines.Sound.Bgm)
        {
            audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
        }
        else
        {
            if (m_AudioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourcesManager.Instance.Load<AudioClip>(path);
                m_AudioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.LogError($"Audio Clip Missing ! {path}");

        return audioClip;
    }

    // 스테이지 연결 간의 BGM Fade 시스템을 위한 함수.
    // 6월 15일 기준 현재 사용 X
    private IEnumerator FadeBgmTrack(AudioClip audioClip, float pitch = 1.0f)
    {
        float timeToFade = 3;
        float timeElapsed = 0;

        if (isPlayingTrack01)
        {
            m_AudioSources[(int)Defines.Sound.Bgm02].pitch = pitch;
            m_AudioSources[(int)Defines.Sound.Bgm02].clip = audioClip;
            m_AudioSources[(int)Defines.Sound.Bgm02].Play();

            while (timeElapsed < timeToFade)
            {
                m_AudioSources[(int)Defines.Sound.Bgm02].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                m_AudioSources[(int)Defines.Sound.Bgm01].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            m_AudioSources[(int)Defines.Sound.Bgm01].Stop();
        }
        else
        {
            m_AudioSources[(int)Defines.Sound.Bgm01].pitch = pitch;
            m_AudioSources[(int)Defines.Sound.Bgm01].clip = audioClip;
            m_AudioSources[(int)Defines.Sound.Bgm01].Play();

            while (timeElapsed < timeToFade)
            {
                m_AudioSources[(int)Defines.Sound.Bgm01].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                m_AudioSources[(int)Defines.Sound.Bgm02].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            m_AudioSources[(int)Defines.Sound.Bgm02].Stop();
        }
    }
}
