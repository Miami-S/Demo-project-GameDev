using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic;
    public AudioMixerGroup musicMixerGroup; // Перетащим сюда группу Music

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.spatialBlend = 0f; // 0 = 2D звук (для музыки)
            musicSource.outputAudioMixerGroup = musicMixerGroup; // Подключаем микшер
            musicSource.Play();
        }
    }
}
