using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using Cinemachine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public bool isTeleporting;
    public Vector3 offset;
    public Animator anim;
    public int levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);
        anim = GetComponent<Animator>();
    }

    public void OnFadeOut(){
        Portal.instance.transform.position = PlayerMovement.instance.transform.position + offset;
        Portal.instance.OpeningPortal();
    }

    // Update is called once per framevoid Update()
      
    public void Change(int level){
        isTeleporting = true;
        Portal.instance.transform.position = PlayerMovement.instance.transform.position + offset;
        PlayerMovement.instance.canMove = false;
        Portal.instance.ClosingPortal(level);
    }
    public void LoadScene(){
        AsyncOperation async = SceneManager.LoadSceneAsync(levelToLoad);
        StartCoroutine(LoadingLevel(async));
    }

    IEnumerator LoadingLevel(AsyncOperation async){
        while(!async.isDone){
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        PlayerMovement.instance.canMove = false;
        anim.SetTrigger("FadeOut");
        isTeleporting = false;
    }
}
