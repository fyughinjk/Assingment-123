using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime;

    [HideInInspector]
    public float xVel;
    [HideInInspector]
    public float yVel;
    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0) lifetime = 1.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2 (xVel, yVel);
        Destroy (gameObject, lifetime);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy (gameObject);
    }
}