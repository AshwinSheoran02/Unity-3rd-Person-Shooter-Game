using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstances : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneChanger.instance != null){
            Destroy(SceneChanger.instance.gameObject);
        }
        if(Portal.instance != null){
            Destroy(Portal.instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
