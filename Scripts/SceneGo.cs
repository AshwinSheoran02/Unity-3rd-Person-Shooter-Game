using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneGo : MonoBehaviour
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
        if(col.tag == "Vehicle"){
            col.gameObject.GetComponent<CarController>().enabled = false;
            HUDManager.instance.reached = true;
            SceneManager.LoadScene(5);
        }
    }
}
