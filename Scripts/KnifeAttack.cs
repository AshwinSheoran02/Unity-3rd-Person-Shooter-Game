using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    public GameObject HitIpmact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.tag == "Enemy"){
            if(col != null){
                GameObject gm = Instantiate(HitIpmact,col.ClosestPoint(PlayerWeapon.instance.knife.transform.position),Quaternion.identity);
                Destroy(gm,2);
                col.GetComponent<Health>().TakeDamage(100);
            }
        }
    }
}
