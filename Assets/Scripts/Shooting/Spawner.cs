using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform player;
    public GameObject missile;
    public GameObject item;

    [Range(0, 3.0f)]
    public float minTime = 1.5f;

    [Range(0, 3.0f)]
    public float maxTime = 2.5f;

    [Range(0, 1.0f)]
    public float missileRatio = 0.7f;

    public List<GameObject> spawnedObject = new List<GameObject>();

    float spawnTime = 0;
    float curTime = 0;

    void Start()
    {
        Debug.Assert(minTime <= maxTime, "최소 시간이 최대 시간보다 커욧!");
        spawnTime = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        if(curTime > spawnTime)
        {
            float draw = Random.Range(0.0f, 1.0f);
            GameObject spawnObj = draw < missileRatio ? missile : item;

            GameObject go = Instantiate(spawnObj, transform.position, Quaternion.identity);
            go.transform.GetComponent<ObjectMove>().spawner = this;
            spawnedObject.Add(go);

            curTime = 0;
            spawnTime = Random.Range(minTime, maxTime);
        }
        else
        {
            curTime += Time.deltaTime;
        }
    }

    public void DestroySpawnedObjects()
    {
        foreach(GameObject obj in spawnedObject)
        {
            Destroy(obj);
        }

        spawnedObject.Clear();
    }
}
