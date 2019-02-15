using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    private Rigidbody2D _body;
    public bool IsMoving { get; private set; } = false;
    public bool IsGrounded { get; private set; } = false;
    
    public MovableDirection MovableDirection { get; } = new MovableDirection();
    
    
    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"isGrounded = {IsGrounded}, isMoving = {IsMoving}");
        if (IsMoving) return;
        IsMoving = MovableDirection.Vertical != 0 || MovableDirection.Horizontal != 0;
        if (IsMoving)
        {
            transform.rotation = MovableDirection.RotateAngleZ;
            _body.velocity = MovableDirection.Direction * 40.0f;
            StartCoroutine(WaitForLoseGround());
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        IsGrounded = true;
        IsMoving = false;
        if (other.gameObject.CompareTag("Spike"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        IsGrounded = false;
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
        }
    }
    
    IEnumerator WaitForLoseGround()
    {
        yield return new WaitForSeconds(3*Time.deltaTime);
        IsMoving = !IsGrounded;
        if (!IsMoving)
        {
            MovableDirection.Vertical = 0;
            MovableDirection.Horizontal = 0;
        }
        yield return null;
    }
}
