using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public GameObject hitImpact;
    public string tagName;
    public RaycastHit location;
    public float Damage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetLocation(RaycastHit hit){
        location = hit;
    }
    void OnTriggerEnter(Collider col){
        if(location.point == null){
            return;
        }
        //Debug.Log(col.gameObject.name);
        GameObject vfx = Instantiate(hitImpact,transform.position,Quaternion.identity);
        vfx.transform.up = location.normal;
        Destroy(vfx,1);
        if(col.gameObject.tag == tagName){
            col.gameObject.GetComponent<Health>().TakeDamage(Damage);
        }
        Destroy(gameObject);
    }
}
