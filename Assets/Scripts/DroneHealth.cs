using UnityEngine;

public class DroneHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Дрон ранен, осталось: " + currentHealth);

        if (currentHealth <= 0f)
        {
            DroneAI drone = GetComponent<DroneAI>();
            if (drone != null)
                drone.Explode();
            else
                Destroy(gameObject);
        }
    }
}
