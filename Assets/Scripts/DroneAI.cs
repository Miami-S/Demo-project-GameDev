using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.Audio;

[RequireComponent(typeof(NavMeshAgent))] 
public class DroneAI : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 30f;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float explodeDistance = 1.2f;

    [Header("Explosion")]
    public float explosionRadius = 3f;
    public float explosionDamage = 40f;
    public GameObject explosionEffect;

    public event System.Action OnDeath;   

    private float currentHealth;
    private Transform player;
    private PlayerController playerController;
    private NavMeshAgent agent;
    private bool isExploding;

    [Header("Audio Settings")]
    public AudioSource audioSource;          
    public AudioResource explosionContainer;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = explodeDistance;

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerController = p.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (player == null || isExploding) return;

        agent.SetDestination(player.position);
        if (Vector3.Distance(transform.position, player.position) <= explodeDistance)
            Explode();
    }

    public void TakeDamage(float damage)
    {
        if (isExploding) return;
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            playerController?.AddKill();   
            Explode(dealDamage: false);    
        }
    }

    public void Explode(bool dealDamage = true)
    {
        if (isExploding) return;
        isExploding = true;

        if (agent != null) agent.isStopped = true;
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        if (AudioPool.Instance != null && explosionContainer != null)
        {
            AudioPool.Instance.PlaySound(explosionContainer, transform.position, 1f);
        }
        
        if (dealDamage)
        {
            foreach (var hit in Physics.OverlapSphere(transform.position, explosionRadius))
            {
                var pc = hit.GetComponent<PlayerController>();
                if (pc != null) pc.TakeDamage(explosionDamage);
            }
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}