using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelFailed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col){
        if(col.gameObject.name == "Terrain"){
            HUDManager.instance.Failed();
        }
    }
}
