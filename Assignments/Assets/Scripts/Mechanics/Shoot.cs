using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;

    [Range(0f, 10f)]
    public float xVel;
    [Range(0f, 10f)]
    public float yVel;


    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    
    public Projectile projectilePrefab; 
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (xVel == 0 && yVel== 0) xVel = 7f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            Debug.Log("Please set default values on the shoot script " + gameObject.name);
    }

    public void Fire()
    {
        if (!sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            curProjectile.xVel = xVel;
            curProjectile.yVel = yVel;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.xVel = -xVel;
            curProjectile.yVel = yVel;
            
        }
    }
   
}
