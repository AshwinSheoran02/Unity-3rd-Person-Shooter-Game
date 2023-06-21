using UnityEngine;
using Cinemachine;
public class CarSwitch : MonoBehaviour
{
    public GameObject playerObject;
    public CinemachineFreeLook mainCamera;
    public CinemachineFreeLook carCamera;
    // public Transform minimapFollowObject;
    // public Transform minimapCameraTransform;
    // public GameObject MainPlayer;
    public GameObject MiniMapPrefab;


    private bool switched = false;



// void ActivateObject()
// {
//     MainPlayer.SetActive(true);
//     Debug.Log("Activated");
// }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)){
             playerObject.SetActive(false);

            // Enable the car controller script
            GetComponent<CarController>().enabled = true;

            // Set the car camera's priority higher than the main camera's priority
            carCamera.Priority = mainCamera.Priority + 1;

            switched = true;
            // Invoke("ActivateObject", 5f);
            MiniMapPrefab.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F) && !switched && playerObject.GetComponent<KeyPickUp>().isPicked)
        {
            // Disable the player object
            playerObject.SetActive(false);

            // Enable the car controller script
            GetComponent<CarController>().enabled = true;

            // Set the car camera's priority higher than the main camera's priority
            carCamera.Priority = mainCamera.Priority + 1;

            switched = true;
            // Invoke("ActivateObject", 5f);
            MiniMapPrefab.SetActive(false);
        }
    }

    // private void LateUpdate()
    // {
    //     // Update the position of the minimap camera to follow the minimap follow object
    //     minimapCameraTransform.position = new Vector3(minimapFollowObject.position.x, minimapCameraTransform.position.y, minimapFollowObject.position.z);
    // }
}
