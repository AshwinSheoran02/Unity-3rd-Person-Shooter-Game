using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterM : MonoBehaviour
{
    public GameObject showMssg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            if(showMssg.activeInHierarchy){
                showMssg.SetActive(false);
                SceneChanger.instance.Change(3);
            }
        }
    }

    public void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            showMssg.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            showMssg.SetActive(false);
        }
    }
}
