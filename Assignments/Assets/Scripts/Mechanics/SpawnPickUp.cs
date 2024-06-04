using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickUp : MonoBehaviour
{
    public GameObject[] pickups;
    // Start is called before the first frame update
    void Start()
    {
        int randNum = Random.Range(0, pickups.Length);
        if (pickups[randNum] == null) return;

        Instantiate(pickups[randNum], transform.position, transform.rotation);
    }
}
