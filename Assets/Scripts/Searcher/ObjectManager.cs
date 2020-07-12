using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject[] objs;

    [SerializeField]
    float minDistance = 2.0f;
    [SerializeField]
    float maxDistance = 4.5f;

    public void AllocateObjects()
    {
        for(int i = 0; i < objs.Length; i++)
        {
            bool isSet = false;

            while (!isSet)
            {
                float dist = Random.Range(minDistance, maxDistance);
                float angle = Random.Range(0.0f, 360.0f);

                float h = Mathf.Cos(angle * Mathf.Deg2Rad) * dist;
                float v = Mathf.Sin(angle * Mathf.Deg2Rad) * dist;
                Vector3 pos = new Vector3(h, 0.25f, v);

                //int layerMask = LayerMask.GetMask("Enemy", "Bomb");
                int layerMask = (1 << 8) | (1 << 9);
                Collider[] cols = Physics.OverlapSphere(pos, 0.5f, layerMask);
                
                if(cols.Length == 0)
                {
                    isSet = true;
                    objs[i].transform.localPosition = pos;
                }
            }
        }
    }


}
