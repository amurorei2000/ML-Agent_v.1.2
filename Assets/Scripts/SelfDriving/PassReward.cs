using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassReward : MonoBehaviour
{
    public SelfCarAgent agent;

    public bool isFinal = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isFinal)
        {
            // 최종 트리거에 도달하면 게임 클리어 + 상점 부여
            agent.AddReward(10.0f);
            agent.EndEpisode();
        }
        else
        {
            // 중간 트리거에 도달하면 상점 부여
            agent.AddReward(2.0f);
        }
    }
}
