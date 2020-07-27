using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Transform[] blocks;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AllocateObjects();
        }

        else if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(AllocOver());
        }
    }


    void AllocateObjects()
    {
        foreach(Transform block in blocks)
        {
            block.position = Vector3.zero;
        }

        List<Vector3> locPos = new List<Vector3>();

        for (int i = 0; i < blocks.Length; i++)
        {
            float dist = Random.Range(3.0f, 7.5f);
            float theta = Random.Range(0, 360.0f);

            float h = Mathf.Cos(theta * Mathf.Deg2Rad) * dist;
            float v = Mathf.Sin(theta * Mathf.Deg2Rad) * dist;

            Vector3 myPos = new Vector3(h, 0, v);

            int num = 0;

            for (int j = 0; j < locPos.Count; j++)
            {
                if (Vector3.Distance(locPos[j], myPos) < 1.5f)
                {
                    num++;
                }
            }

            if(num > 0)
            {
                i--;
            }
            else
            {
                blocks[i].position = myPos;
                locPos.Add(myPos);
            }
        }
    }

    IEnumerator AllocOver()
    {
        foreach (Transform block in blocks)
        {
            block.position = Vector3.zero;
        }

        yield return new WaitForFixedUpdate();

        //Collider[] cols = Physics.OverlapSphere(transform.position, 0.6f, 1 << LayerMask.NameToLayer("Bomb"));

        //if(cols.Length > 0)
        //{
        //    print("충돌 " + cols.Length + "개");
        //}

        for (int i = 0; i < blocks.Length; i++)
        {
            float dist = Random.Range(3.0f, 7.5f);
            float theta = Random.Range(0, 360.0f);

            float h = Mathf.Cos(theta * Mathf.Deg2Rad) * dist;
            float v = Mathf.Sin(theta * Mathf.Deg2Rad) * dist;

            Vector3 myPos = new Vector3(h, 0, v);

            Collider[] cols = Physics.OverlapSphere(myPos, 0.6f, 1 << LayerMask.NameToLayer("Bomb"));

            if (cols.Length > 0)
            {
                i--;
            }
            else
            {
                blocks[i].position = myPos;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
