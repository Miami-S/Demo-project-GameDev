using System.Collections; // Обязательно для корутин (IEnumerator)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance { get; private set; }

    [Header("Настройки пула")]
    public AudioSource[] audioSources; // Перетащи сюда свои AudioSource из инспектора

    private Queue<AudioSource> availableSources = new Queue<AudioSource>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSources == null || audioSources.Length == 0)
        {
            Debug.LogError("ОШИБКА: В AudioPool не добавлены AudioSource объекты!");
            return;
        }

        foreach (var source in audioSources)
        {
            source.playOnAwake = false;
            source.gameObject.SetActive(false); // Прячем
            availableSources.Enqueue(source);
        }
    }

    public void PlaySound(AudioResource resource, Vector3 position, float volume = 1f)
    {
        if (resource == null || availableSources.Count == 0) return;

        AudioSource source = availableSources.Dequeue();

        source.transform.position = position;
        source.resource = resource;
        source.volume = volume;
        source.gameObject.SetActive(true); // Включаем

        source.Play();

        // Запускаем корутину возврата (используется System.Collections.IEnumerator)
        StartCoroutine(ReturnToPoolWhenDone(source));
    }

    // ВОТ ЗДЕСЬ БЫЛА ОШИБКА. Тип должен быть просто IEnumerator (без <T>)
    private IEnumerator ReturnToPoolWhenDone(AudioSource source)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => source.isPlaying);

        source.gameObject.SetActive(false); // Прячем обратно
        availableSources.Enqueue(source); // Возвращаем в пул
    }
}