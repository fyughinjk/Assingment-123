using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUp : MonoBehaviour
{
   public enum PickupType
    {
        Life, 
        PowerupSpeed,
        PowerupJump,
        Score
    }

    [SerializeField] private PickupType type;

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
                case PickupType.PowerupSpeed:
                case PickupType.PowerupJump:
                    GameManager.Instance.PlayerInstance.PowerupValueChange(type);
                    Debug.Log("I should be doing power up things!");
                    break;
            }

            Destroy(gameObject);
        }
    }
}

