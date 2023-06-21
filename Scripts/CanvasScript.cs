using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CanvasScript : MonoBehaviour
{
    public UiManager manager;
    public int timer = 5;
    public GameObject notify;
    // Start is called before the first frame update
    void Start()
    {
        notify.SetActive(false);
        StartCoroutine(activateNotify());
    }

    // Update is called once per frame
    void Update()
    {
        if(notify.activeInHierarchy && Input.GetKeyDown(KeyCode.X)){
            manager.scene = 2;
            manager.OnClickStart();
        }
    }

    IEnumerator activateNotify(){
        yield return new WaitForSeconds(timer);
        notify.SetActive(true);
    }

}
