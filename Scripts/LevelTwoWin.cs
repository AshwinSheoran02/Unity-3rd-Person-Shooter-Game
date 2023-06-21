using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoWin : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            Debug.Log("Level 2 ho gya ");
        }
    }
}
