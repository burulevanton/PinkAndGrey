using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public GameObject OtherPortal;
   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            other.gameObject.transform.position = OtherPortal.gameObject.transform.position;
        }
    }
}
