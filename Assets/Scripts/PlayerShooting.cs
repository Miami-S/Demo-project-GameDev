using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerShooting: MonoBehaviour
{
    public Transform gunMuzzle;
    public GameObject flashEffect;
    public LineRenderer tracer;

    public float range = 100f;
    public int damage = 10;
    public float fireRate = 0.2f;

    private float nextFire = 0f;
    private Camera cam;

    [Header("Audio Settings")]
    public AudioSource audioSource;          // Перетащи сюда AudioSource с игрока
    public AudioResource shootContainer;     // Перетащи сюда твой Audio Random Container
    [Range(0f, 1f)] public float shootVolume = 0.5f;

    void Start()
    {
        cam = Camera.main;
        tracer.enabled = false;
        tracer.positionCount = 2; // Обязательно 2 точки

    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Вспышка
        if (flashEffect != null)
            Destroy(Instantiate(flashEffect, gunMuzzle.position, gunMuzzle.rotation), 1f);

        if (audioSource != null && shootContainer != null)
        {
            audioSource.volume = shootVolume;
            audioSource.resource = shootContainer; // Наш ARC
            audioSource.Play();
        }

        // Луч из центра экрана
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            // Если попали в дрона - бьем его
            DroneAI drone = hit.collider.GetComponentInParent<DroneAI>();
            if (drone != null) drone.TakeDamage(damage);
        }
        else
        {
            endPoint = ray.GetPoint(range); // Если промахнулись, луч летит в даль
        }

        // Рисуем линию и выключаем её через 0.05 сек
        tracer.enabled = true;
        tracer.SetPosition(0, gunMuzzle.position);
        tracer.SetPosition(1, endPoint);
        Invoke("HideTracer", 0.05f);
    }

    void HideTracer()
    {
        tracer.enabled = false;
    }
}
