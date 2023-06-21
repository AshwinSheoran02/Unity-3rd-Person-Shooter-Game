using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject hitImpact;
    public float impactRadius;
    public float force;
    public Vector3 target;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = target - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        
        // Smoothly turn towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        
    }

    public void OnTriggerEnter(Collider col){
        if(col.tag == "Vehicle"){
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in colliders)
        {
            if(hit.GetComponent<Destructible>() != null){
                hit.GetComponent<Destructible>().ishit = true;
            }
            if(hit.GetComponent<Health>() != null){
                hit.GetComponent<Health>().TakeDamage(100);
            }
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            if (hitRb)
            {
                hitRb.AddExplosionForce(force, transform.position, impactRadius, 3.0f);
            }
        }     
        GameObject temp = Instantiate(hitImpact,transform.position,Quaternion.identity);
        Destroy(temp,2);
        Destroy(gameObject,0.1f);
    }
}
