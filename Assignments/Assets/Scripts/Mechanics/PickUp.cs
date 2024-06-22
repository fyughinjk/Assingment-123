using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

public class PickUp : MonoBehaviour
{

    AudioSource  audioSource;
   public enum PickupType
    {
        Life, 
        PowerupJump,
        Score
    }

    [SerializeField] private PickupType type;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (type)
            {
                case PickupType.Life:
                    GameManager.Instance.lives++;
                    break;
                case PickupType.Score:
                    Debug.Log("I should be changing some sort of variable!");
                    break;
                case PickupType.PowerupJump:
                    GameManager.Instance.PlayerInstance.PowerupValueChange(type);
                    Debug.Log("I should be doing power up things!");
                    break;
            }

            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, audioSource.clip.length);
            audioSource.Play();
        }
    }
}

