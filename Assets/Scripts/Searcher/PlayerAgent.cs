using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class PlayerAgent : Agent
{
    public ObjectManager objManager;
    public float rotSpeed = 5.0f;
    public RayTarget rayTarget;

    Vector3 playerOrigin;

    public override void Initialize()
    {
        playerOrigin = transform.position;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if(MaxStep > 0)
        {
            AddReward(-1.0f / (float)MaxStep);
        }

        float rotDir = 0;

        switch (vectorAction[0])
        {
            case 1:
                rotDir = 1.0f;
                break;
            case 2:
                rotDir = -1.0f;
                break;
        }

        transform.Rotate(transform.up, rotDir * rotSpeed);
        transform.position += transform.forward * vectorAction[1] * Time.fixedDeltaTime;
    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);

        if(Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 2.0f;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 1.0f;
        }

        if(Input.GetKey(KeyCode.W))
        {
            actionsOut[1] = 1.0f;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float[] observaitions = rayTarget.Perceive();
        sensor.AddObservation(observaitions);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = playerOrigin;
        objManager.AllocateObjects();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Enemy"))
        {
            AddReward(10.0f);
            EndEpisode();
        }
        else if(col.transform.CompareTag("Bomb"))
        {
            AddReward(-10.0f);
            EndEpisode();
        }
        else if(col.transform.CompareTag("Wall"))
        {
            AddReward(-1.0f);
        }
    }
}
