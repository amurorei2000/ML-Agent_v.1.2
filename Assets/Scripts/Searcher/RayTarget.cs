using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTarget : MonoBehaviour
{
    public string[] detectionTags;

    [Range(0, 50)]
    public int rayCount = 5;

    [Range(0, 180)]
    public float rayRange = 90.0f;

    [Range(0.0f, 10.0f)]
    public float sphereRadius = 0.5f;

    [Range(0.0f, 100.0f)]
    public float rayDistance = 20.0f;

    public Color success;
    public Color failed;

    public float[] temp;

    public List<Vector3> successRay = new List<Vector3>();
    public List<Vector3> failedRay = new List<Vector3>();

    public float[] Perceive()
    {
        // 전체 레이의 갯수
        int allRayCount = rayCount * 2 + 1;

        // 레이 각도별 방향 벡터 배열
        Vector3[] rayEndPoints = new Vector3[allRayCount];

        for(int i = 0; i < allRayCount; i++)
        {
            float oneAngle = rayRange / rayCount;
            float addAngle = (90.0f - rayRange) + oneAngle * i;

            float h = Mathf.Cos(addAngle * Mathf.Deg2Rad);
            float v = Mathf.Sin(addAngle * Mathf.Deg2Rad);
            Vector3 destination = new Vector3(h, v, 0);
            //Vector3 destination = new Vector3(h, 0, v);

            rayEndPoints[i] = transform.TransformPoint(destination) - transform.position;
        }

        // 저장 배열 간격
        int bufferOffset = detectionTags.Length + 2;

        // 반환할 관찰 값 배열
        temp = new float[bufferOffset * allRayCount];

        successRay.Clear();
        failedRay.Clear();

        // 각도 별로 스피어 캐스트 발사하기
        for(int i= 0; i < allRayCount; i++)
        {
            Ray ray = new Ray(transform.position, rayEndPoints[i]);
            RaycastHit hitInfo;

            // 레이에 충돌한 대상이 있을 때
            if (Physics.SphereCast(ray, sphereRadius, out hitInfo, rayDistance))
            {
                for (int j = 0; j < detectionTags.Length; j++)
                {
                    // 충돌한 대상의 태그와 같으면 1, 다르면 0을 설정한다.
                    if (hitInfo.transform.CompareTag(detectionTags[j]))
                    {
                        temp[bufferOffset * i + j] = 1.0f;
                        temp[bufferOffset * i + detectionTags.Length + 1] = hitInfo.distance / rayDistance;
                        temp[bufferOffset * i + detectionTags.Length] = 0;
                        successRay.Add(hitInfo.point);
                    }
                }
            }
            // 레이에 충돌한 대상이 없을 때
            else
            {
                temp[bufferOffset * i] = 0;
                temp[bufferOffset * i + 1] = 0;
                temp[bufferOffset * i + detectionTags.Length] = 1.0f;
                temp[bufferOffset * i + detectionTags.Length + 1] = 1.0f;
                failedRay.Add(transform.position + (rayEndPoints[i] * rayDistance));
            }
        }

        return temp;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < successRay.Count; i++)
        {
            Gizmos.DrawLine(transform.position, successRay[i]);
            Gizmos.DrawWireSphere(successRay[i], sphereRadius);
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < failedRay.Count; i++)
        {
            Gizmos.DrawLine(transform.position, failedRay[i]);
            Gizmos.DrawWireSphere(failedRay[i], sphereRadius);
        }
    }
}
