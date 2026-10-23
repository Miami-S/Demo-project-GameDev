using System.Collections;
using UnityEngine;

public class PlayerShooting: MonoBehaviour
{
    [Header("Ссылки")]
    public Transform gunMuzzle;
    public GameObject muzzleFlashPrefab;
    public GameObject hitEffectPrefab;  
    public LineRenderer tracerLine;     

    [Header("Настройки")]
    public float range = 100f;
    public int damage = 10;
    public float fireRate = 0.2f;
    public float tracerTime = 0.05f;

    private float nextFireTime = 0f;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;

        if (tracerLine != null)
            tracerLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, gunMuzzle.position, gunMuzzle.rotation);
            Destroy(flash, 1f); 
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;
            Debug.Log("Попали в: " + hit.transform.name);

            if (hitEffectPrefab != null)
            {
                GameObject sparks = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(sparks, 2f);
            }

            DroneHealth drone = hit.transform.GetComponent<DroneHealth>();
            if (drone != null)
            {
                drone.TakeDamage(damage);
            }
        }
        else
        {
            endPoint = ray.GetPoint(range);
        }

        if (tracerLine != null)
        {
            StartCoroutine(ShowTracer(gunMuzzle.position, endPoint));
        }
    }

    IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        tracerLine.enabled = true;
        tracerLine.SetPosition(0, start);
        tracerLine.SetPosition(1, end);

        yield return new WaitForSeconds(tracerTime);

        tracerLine.enabled = false;
    }
}
