using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ShootingAgent : Agent
{
    public float moveSpeed = 7;
    public Spawner[] spawners;

    Vector3 originPos;
    RayTarget rt;

    public override void Initialize()
    {
        originPos = transform.localPosition;
        rt = GetComponent<RayTarget>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if(MaxStep > 0)
        {
            AddReward(-1.0f / MaxStep);
        }

        float h = 0;
        float v = 0;

        switch (vectorAction[0])
        {
            case 1:
                h = -1.0f;
                break;
            case 2:
                h = 1.0f;
                break;
        }

        switch (vectorAction[1])
        {
            case 1:
                v = 1.0f;
                break;
            case 2:
                v = -1.0f;
                break;
        }

        Vector3 dir = transform.right * h + transform.up * v;
        dir.Normalize();

        transform.position += dir * moveSpeed * Time.fixedDeltaTime;
    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);

        if(Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 1.0f;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 2.0f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[1] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[1] = 2.0f;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (rt)
        {
            float[] observations = rt.Perceive();
            sensor.AddObservation(observations);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = originPos;

        foreach(Spawner sp in spawners)
        {
            sp.DestroySpawnedObjects();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.gameObject.CompareTag("Missile"))
        {
            AddReward(-10.0f);
            EndEpisode();
        }
        else if (col.transform.gameObject.CompareTag("Item"))
        {
            AddReward(5.0f);
        }
        else if (col.transform.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.5f);
        }
    }
}
