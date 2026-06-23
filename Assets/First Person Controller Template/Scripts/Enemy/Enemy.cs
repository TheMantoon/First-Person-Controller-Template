using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100;
    private AudioSource audioSource = null;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void TakeDamage(float damage)
    {
        audioSource.Play();
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}