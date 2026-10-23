using UnityEngine;

public class DroneAI : MonoBehaviour
{
    [Header("Движение")]
    public float moveSpeed = 4f;          
    public float rotationSpeed = 5f;    
    public float explodeDistance = 1.2f;  

    [Header("Взрыв")]
    public float explosionRadius = 3f;    
    public float explosionDamage = 40f;   
    public GameObject explosionEffect;    

    private Transform player;
    private Rigidbody rb;
    private bool isExploding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null || isExploding) return;

        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, rotationSpeed * Time.fixedDeltaTime));

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= explodeDistance)
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (isExploding) return;
        isExploding = true;

        Debug.Log("Дрон взорвался!");

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                float falloff = 1f - (dist / explosionRadius);
                float finalDamage = explosionDamage * Mathf.Clamp01(falloff);
                ph.TakeDamage(finalDamage);
            }

        }

        Destroy(gameObject);
    }
}
