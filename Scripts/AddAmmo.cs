using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    public int ammoVal;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,rotationSpeed*Time.deltaTime,0,Space.Self);
    }

    public void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            col.GetComponent<Shooting>().addAmmo(ammoVal);
            Destroy(this.gameObject);
        }
    }
}
