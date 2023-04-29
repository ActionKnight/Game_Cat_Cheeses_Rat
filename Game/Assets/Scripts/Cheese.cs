using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.instance.SpawnNew();
            PlayerMovement.instance.Eat();
            gameObject.SetActive(false);
        }
    }
}
