using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public GameObject OtherPortal;
    
    //todo do something with player
    public Player Player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           other.gameObject.transform.position = Player.PlayerDirection.GetOffsetAfterPortal(OtherPortal.transform.position);
        }
    }
}
