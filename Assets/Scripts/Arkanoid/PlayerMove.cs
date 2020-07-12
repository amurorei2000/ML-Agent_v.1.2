using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using System;

public class PlayerMove : Agent
{
    public float speed = 7.0f;
    public Vector3 originBallPos;
    public Transform ball;
    public GameObject[] blocks;
    public float shootPower = 10.0f;
    public int catchBlock = 0;

    Vector3 originPos;
    Rigidbody ballRB;

    float timePenalty = 0;

    public override void Initialize()
    {
        //print("초기화!");
        
        // 초기 위치 저장
        originPos = transform.localPosition;

        if (MaxStep > 0)
        {
            timePenalty = -1.0f / (float)MaxStep;
        }

        ballRB = ball.GetComponent<Rigidbody>();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if(catchBlock >= blocks.Length)
        {
            AddReward(10.0f);
            EndEpisode();
        }

        // 시간 경과에 따른 페널티 부여
        AddReward(timePenalty);

        // 이동 방향 벡터 초기화
        Vector3 dir = Vector3.zero;

        switch(vectorAction[0])
        {
            case 1:
                dir = transform.right * -1.0f;
                break;
            case 2:
                dir = transform.right;
                break;
        }

        transform.localPosition += dir * speed * Time.deltaTime;

        Vector3 confirmPos = transform.localPosition;
        confirmPos.x = Mathf.Clamp(confirmPos.x, -7.5f, 7.5f);
        transform.localPosition = confirmPos;

        // 공 놓치면 학습 종료
        if(ball.localPosition.y < transform.localPosition.y)
        {
            AddReward(-10.0f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 공의 위치
        //sensor.AddObservation(ball.localPosition.x);
        //sensor.AddObservation(ball.localPosition.y);

        // 공을 바라보는 방향
        Vector3 dir = ball.transform.localPosition - transform.localPosition;

        float[] vals = new float[2];
        if(dir.x > 0 )
        {
            vals = new float[2] { 0, 1.0f };
        }
        else if(dir.x < 0)
        {
            vals = new float[2] { 1.0f, 0 };
        }
        else
        {
            vals = new float[2] { 0, 0 };
        }
        sensor.AddObservation(vals);
        //sensor.AddObservation(dir);

        // 공과의 거리
        sensor.AddObservation(Vector3.Distance(ball.transform.position, transform.position));
    }

    public override void Heuristic(float[] actionsOut)
    {
        // 액션 값 비우기
        Array.Clear(actionsOut, 0, actionsOut.Length);

        // 사용자의 입력 값에 따라 액션 값을 설정하기
        if(Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 2.0f;
        }
    }

    public override void OnEpisodeBegin()
    {
        //print("에피소드 시작! ");

        // 플레이어와 공 위치 초기화
        transform.localPosition = originPos;
        ball.localPosition = originBallPos;

        // 모든 블록 활성화하기
        foreach(GameObject block in blocks)
        {
            block.SetActive(true);
        }
        catchBlock = 0;

        // 공을 다시 발사
        ballRB.velocity = Vector3.zero;

        float h = UnityEngine.Random.Range(-1.0f, 1.0f);
        ballRB.AddForce(new Vector3(h, 1, 0) * shootPower, ForceMode.VelocityChange);

        //print("에피소드 " + Academy.Instance.EpisodeCount);
    }
}
