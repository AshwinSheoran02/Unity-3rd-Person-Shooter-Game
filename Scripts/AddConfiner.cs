using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AddConfiner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneChanger.instance.isTeleporting){
            GetComponent<CinemachineConfiner>().enabled = true;
            GetComponent<CinemachineConfiner>().m_BoundingVolume = Portal.instance.GetComponent<Collider>();
        }
        else{
            GetComponent<CinemachineConfiner>().enabled = false;
        }
    }
}
