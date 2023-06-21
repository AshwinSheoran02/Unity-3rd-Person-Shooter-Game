using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{

    public bool isPicked = false;
    public GameObject CarPointer;
    public float delay = 5.0f;
    public GameObject KeyInstructions;

    private void Start()
    {
        KeyInstructions.SetActive(false);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            isPicked = true;
            Debug.Log("Key Picked Up");
            CarPointer.SetActive(true);
            KeyInstructions.SetActive(true);
            StartCoroutine(DeactivateGameObject());
        }
    }

    IEnumerator DeactivateGameObject(){
        yield return new WaitForSeconds(delay);
        KeyInstructions.SetActive(false);
    }
}
