using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public PlayerMove pm;

    void Start()
    {
        pm.originBallPos = transform.localPosition;

    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.CompareTag("Block"))
        {
            col.transform.gameObject.SetActive(false);
            pm.AddReward(0.5f);
            pm.catchBlock++;
        }
    }
}
