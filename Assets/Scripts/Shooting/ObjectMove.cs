using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public Spawner spawner;
    public float moveSpeed = 6;

    [Range(0, 1.0f)]
    public float tracingRatio = 0.7f;

    Vector3 dir;

    void Start()
    {
        float action = Random.Range(0, 1.0f);
        dir = action < tracingRatio ? transform.up * -1.0f : (spawner.player.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        transform.position += dir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        spawner.spawnedObject.Remove(gameObject);
        Destroy(gameObject);
    }
}
