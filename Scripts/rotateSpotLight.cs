using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSpotLight : MonoBehaviour
{
    public float rotationSpeed;
    public float detectionRadius;
    public LayerMask layer;
    public float spotlightAngle;
    public Light spotLight;
    public float maxDistance;
    public LayerMask obstacle;
    public Transform leftCheck;
    public Transform rightCheck;
    public float realtimeDetect;
    // Start is called before the first frame update
    void Start()
    {
        realtimeDetect = Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    // Update is called once per frame
    void Update()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius, layer);
        foreach(Collider c in col)
        {
            Vector3 dir = c.transform.position - transform.position;
            float angle = Vector3.Dot(spotLight.transform.forward, dir.normalized);
            if (angle > Mathf.Cos(Mathf.Deg2Rad * spotlightAngle*0.5f))
            {
                Vector3 left = leftCheck.position - transform.position;
                Vector3 right = rightCheck.position - transform.position;
                RaycastHit midhit;
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(transform.position, dir.normalized,out midhit,maxDistance,obstacle);
                Physics.Raycast(transform.position, left.normalized, out leftHit, maxDistance, obstacle);
                Physics.Raycast(transform.position, right.normalized, out rightHit, maxDistance, obstacle);
                if (midhit.collider != null && midhit.collider.tag == "Player")
                {
                    PlayerMovement.instance.currentDetection+=realtimeDetect;
                    //Debug.Log("Yes");
                }
                else if (leftHit.collider != null && leftHit.collider.tag == "Player")
                {
                    PlayerMovement.instance.currentDetection+=realtimeDetect;
                    //Debug.Log("Yes");
                }
                else if(rightHit.collider != null && rightHit.collider.tag == "Player")
                {
                    PlayerMovement.instance.currentDetection+=realtimeDetect;
                    //Debug.Log("Yes");
                }
            }
        }
        Quaternion currentRotation = transform.rotation;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = currentRotation * deltaRotation;
    }
}
