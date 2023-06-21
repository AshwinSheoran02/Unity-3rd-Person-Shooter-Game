using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rotationSpeed;
    public float detectionRadius;
    public bool startDetecting;
    public LayerMask layerMask ;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void FixedUpdate()
    {
        if (startDetecting)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
            // Debug.Log(layerMask);
            foreach (Collider c in col)
            {
                Vector3 dir = (c.gameObject.transform.position - transform.position);
    Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir, Color.red); // visualize the direction of the ray

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                RaycastHit hit;
                // Debug.Log("Okay, this is weird");
    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir.normalized, out hit, detectionRadius, layerMask))
                {
                    // Debug.Log("Something is happening");
                    if(hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("Hit");
                    }
                    else
                    {
                        Debug.Log("Hit someother");
                    }

                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bullet.transform.LookAt(hit.point); // point bullet in the direction of the hit point
                    Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                    bulletRigidbody.velocity = bullet.transform.forward * bulletSpeed; // set the velocity of the bullet

                Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.green, 1f);
                Debug.DrawLine(bullet.transform.position, hit.point, Color.blue, 2f);

                }
            }
        }
    }




}
