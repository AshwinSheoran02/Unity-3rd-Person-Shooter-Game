using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    public int scene = 1;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart(){
        anim.SetTrigger("FadeIn");
        StartCoroutine(Load());
    }
    public void OnClickQuit(){
        Application.Quit();
    }

    IEnumerator Load(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
