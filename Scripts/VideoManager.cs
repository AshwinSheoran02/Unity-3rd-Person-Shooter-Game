using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class VideoManager : MonoBehaviour
{
    public UiManager manager;
    public VideoPlayer videoPlayer;
    public float time;
    public int scene;
    void Start()
    {
        manager.scene = scene;
        videoPlayer.Play();
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene(){
        yield return new WaitForSeconds(time);
        manager.OnClickStart();
    }
}
