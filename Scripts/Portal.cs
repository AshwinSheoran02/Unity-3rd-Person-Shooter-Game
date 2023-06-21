using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Portal instance;
    public float refreshTime = 0.05f;
    public float refreshRate = 0.01f;

    Material mat;


    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
        mat = GetComponent<MeshRenderer>().material;
    }

    void Start(){
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosingPortal(int level){
        instance.gameObject.SetActive(true);
        mat.SetFloat("_InProcess",1);
        StartCoroutine(Close(level));
    }

    public void OpeningPortal(){
        StartCoroutine(Open());
    }

    IEnumerator Close(int level){
        Vector2 a = mat.GetVector("_Offset");
        while(a.y < 0){
            a.y += refreshRate;
            a.y = Mathf.Clamp(a.y,-1,0);
            mat.SetVector("_Offset",a);
            yield return new WaitForSeconds(refreshTime);
        }
        SceneChanger.instance.anim.SetTrigger("FadeIn");
        SceneChanger.instance.levelToLoad = level;
    }

    IEnumerator Open(){
        Vector2 a = mat.GetVector("_Offset");
        while(a.y > -1){
            a.y -= refreshRate;
            a.y = Mathf.Clamp(a.y,-1,0);
            mat.SetVector("_Offset",a);
            yield return new WaitForSeconds(refreshTime);
        }
        mat.SetFloat("_InProcess",0);
        instance.gameObject.SetActive(false);
        PlayerMovement.instance.canMove = true;
    }
}
