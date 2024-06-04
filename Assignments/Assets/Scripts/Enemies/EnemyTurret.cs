using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] private float distThreshold;
    [SerializeField] private float projectileFireRate;
    private float timeSinceLastFire = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2;

        if (distThreshold <= 0)
            distThreshold = 2;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        sr.flipX = (GameManager.Instance.PlayerInstance.transform.position.x < transform.position.x) ? true : false;



        float distance = Vector3.Distance(transform.position, GameManager.Instance.PlayerInstance.transform.position);

        if (distance <= distThreshold)
        {
            sr.color = Color.red;
            if (curPlayingClips[0].clip.name == "Idle")
            {
                if (Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Fire");
                    timeSinceLastFire = Time.time;
                }
            }
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
