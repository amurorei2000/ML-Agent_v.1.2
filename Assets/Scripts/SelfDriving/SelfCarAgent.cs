using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;

public class SelfCarAgent : Agent
{
    public float accelPower = 6.0f;
    public Transform[] roads;

    Quaternion originRot;

    public override void Initialize()
    {
        // 초기 회전값 저장
        originRot = transform.rotation;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // 추락하면 게임 오버 + 감점
        if(transform.localPosition.y < -1.0f)
        {
            AddReward(-10.0f);
            EndEpisode();
        }

        // 지속적인 감점
        if(MaxStep > 0)
        {
            AddReward(-1.0f / (float)MaxStep);
        }

        // 전달받은 방향으로 회전하기
        transform.Rotate(transform.up, vectorAction[0] - 1.0f);

        // 무조건 직진!
        transform.position += transform.forward * accelPower * Time.deltaTime;
    }

    public override void Heuristic(float[] actionsOut)
    {
        Array.Clear(actionsOut, 0, actionsOut.Length);

        actionsOut[0] = 1;

        if(Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 0;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.rotation = originRot;

        // 랜덤하게 도로를 선택하여 활성화하기
        int selection = UnityEngine.Random.Range(0, roads.Length);

        transform.position = roads[selection].position;
        
    }
}
