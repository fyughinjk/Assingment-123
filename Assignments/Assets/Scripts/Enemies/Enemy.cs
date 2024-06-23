using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{

    protected SpriteRenderer sr;
    protected Animator anim;

    protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] private AudioClip Death;

    private AudioSource audioSource;
    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ensure this line is included

        Debug.Log($"AudioSource is {(audioSource == null ? "null" : "assigned")}");
        Debug.Log($"Death clip is {(Death == null ? "null" : "assigned")}");

        if (maxHealth <= 0) maxHealth = 10;
        health = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            if (audioSource != null && Death != null)
            {
                audioSource.PlayOneShot(Death);
            }
            else
            {
                Debug.LogError("AudioSource or Death clip is null");
            }

            if (transform.parent != null)
                Destroy(transform.parent.gameObject, 2);
            else
                Destroy(gameObject, 2);
        }
    }

}