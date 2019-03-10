using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyingPortalController : MonoBehaviour
{
    [SerializeField] public GameObject CopiedPlayer;

    public bool IsActivated = true;
//    [SerializeField] private MoveController _moveController;
//
//    private void OnTriggerExit2D(Collider2D other)
//    {
//        var other_movable = other.gameObject.GetComponent<Movable>();
//        CopiedPlayer.SetActive(true);
//        CopiedPlayer.transform.position = new Vector3(transform.position.x - other_movable.MovableDirection.Horizontal,
//            transform.position.y - other_movable.MovableDirection.Vertical, CopiedPlayer.transform.position.z);
//        _moveController.SetDirection(CopiedPlayer.gameObject.GetComponent<Movable>());
//        gameObject.SetActive(false);
//    }
    public void ActivatePortal(Vector2 moveDirection)
    {
        CopiedPlayer.transform.position = new Vector3(transform.position.x - moveDirection.x, 
            transform.position.y-moveDirection.y, CopiedPlayer.transform.position.z);
        CopiedPlayer.SetActive(true);
        gameObject.SetActive(false);
        IsActivated = false;
        PlayerController playerController = CopiedPlayer.GetComponent<PlayerController>();
        playerController.JumpWithDirection(moveDirection);
    }
}
