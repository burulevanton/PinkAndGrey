using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    
    private Vector2 _moveDirection = new Vector2(0.0f, 0.0f);
    private Vector2 nextSwipeDirection = Vector2.zero;
    private float moveSpeed = 40f;
    private float rotationZ;
    private float scaleX = 1f;
    private float nextSwipeTimeout;
    private float tileSize = 1.0f;

    public TimerWallController OnTimerWall;
    
    
    private GameController gc;
    
    private void OnEnable()
    {
        GameController.OnSwipe += new Action<EDirection>(this.ProcessSwipe);
    }

    private void OnDisable()
    {
        GameController.OnSwipe -= new Action<EDirection>(this.ProcessSwipe);
    }

    public bool Stopped => this._moveDirection == Vector2.zero;

    public bool Moving => !this.Stopped;

    void Start()
    {
        gc = GameController.instance;
        gc.PlayerController = this;
        UpdateScaleRotation(1.0f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 position = (Vector2) this.transform.position;
        Vector2 vector2 = this.moveSpeed * Time.deltaTime * this._moveDirection;
        float distance = Mathf.Abs((double) vector2.x == 0.0 ? vector2.y : vector2.x);
        if (this.Moving)
        {
            bool isStopped = false;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(position + this._moveDirection * this.tileSize * 0.5f, this._moveDirection, distance);
            if (raycastHit2D.collider != null)
            {
                switch (raycastHit2D.collider.gameObject.tag)
                {
                        case "Wall":
                            isStopped = true;
                            StopByTransform(raycastHit2D.transform);
                            break;
                        case "Spike":
                            isStopped = true;
                            DamageTaken(raycastHit2D.collider.gameObject);
                            break;
                }
            }

            if (isStopped)
                position = (Vector2) this.transform.position;
            else
                position += vector2;
        }

        this.transform.position = (Vector3) position;
    }

    private void UpdateScaleRotation(float nScaleX, float nRotationZ)
    {
        this.scaleX = nScaleX;
        this.rotationZ = nRotationZ;
        this.transform.localScale = new Vector3(this.scaleX, 1f, 1f);
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, nRotationZ);
    }
    
    private bool CanJumpWithDirection(Vector2 direction)
    {
        return Mathf.Approximately(direction.y, 1f) && !Mathf.Approximately(this.transform.eulerAngles.z, 180f) || Mathf.Approximately(direction.x, -1f) && !Mathf.Approximately(this.transform.eulerAngles.z, 270f) || (Mathf.Approximately(direction.x, 1f) && !Mathf.Approximately(this.transform.eulerAngles.z, 90f) || Mathf.Approximately(direction.y, -1f) && !Mathf.Approximately(this.transform.eulerAngles.z, 0.0f));
    }
    
    private bool JumpWithDirection(Vector2 direction)
    {
        if (!this.CanJumpWithDirection(direction))
            return false;
        this.OnTimerWall = null;
        this.SetMoveDirection(direction);
        return true;
    }
    
    private void SetMoveDirection(Vector2 direction)
    {
        this._moveDirection = direction;
        if (!this.Moving)
            return;
        float nRotationZ = (double) this._moveDirection.x == 0.0 ? (float) (90.0 * (double) this._moveDirection.y - 90.0) : -90f * this._moveDirection.x;
        this.UpdateScaleRotation((double) this._moveDirection.x == 0.0 ? this.scaleX : this._moveDirection.x, nRotationZ);
    }

    private void ProcessSwipe(EDirection swipe)
    {
        Vector2 zero = Vector2.zero;
        switch (swipe)
        {
            case EDirection.Up:
                zero.y = 1f;
                break;
            case EDirection.Left:
                zero.x = -1f;
                break;
            case EDirection.Right:
                zero.x = 1f;
                break;
            case EDirection.Down:
                zero.y = -1f;
                break;
        }

        if (this.Stopped)
        {
            this.JumpWithDirection(zero);
        }
        else
        {
            this.nextSwipeTimeout = 0.1f;
            this.nextSwipeDirection = zero;
        }
    }

    private void StopByTransform(Transform transform, bool setStopperPosition = false)
    {
        if(this.Stopped)
            return;
        Vector2 position = (Vector2) transform.position;
        if (!setStopperPosition)
            position -= this._moveDirection * this.tileSize;
        this.transform.position = (Vector3) position;
        this.UpdateScaleRotation(this.scaleX, this.rotationZ + 180f);
        this._moveDirection = Vector2.zero;
        if ((double) this.nextSwipeTimeout <= 0.0)
            return;
        this.JumpWithDirection(this.nextSwipeDirection);
        this.nextSwipeTimeout = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
                case "Portal":
                    Teleport(other.gameObject);
                    break;
                case "GreaterSpike":
                    DamageTaken(other.gameObject);
                    break;
                case "Enemy":
                    DamageTaken(other.gameObject);
                    break;
                case "Projectile":
                    DamageTaken(other.gameObject);
                    break;
                case "TimerWall":
                    OnTimerWall = other.gameObject.GetComponent<TimerWallController>();
                    if (OnTimerWall.IsActivated)
                    {
                        StopByTransform(other.gameObject.transform);
                        break;
                    }
                    OnTimerWall = (TimerWallController) null;
                    break;
                case "MovingChangingPlatform":
                    if(Stopped)
                        break;
                    ChangeDirectionWithMovingChangingPlatform(other.gameObject);
                    break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "TimerWall":
                TimerWallController timerWallController = other.gameObject.GetComponent<TimerWallController>();
                if (timerWallController.IsActivated)
                    break;
                timerWallController.ActivateWall();
                break;
        }
    }

    private void Teleport(GameObject portal)
    {
        PortalController portalController = portal.GetComponent<PortalController>();
        this.transform.position = (Vector2) portalController.OtherPortal.transform.position + this._moveDirection * this.tileSize;
    }

    private void DamageTaken(GameObject damage)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ChangeDirectionWithMovingChangingPlatform(GameObject movingChangingPlatform)
    {
        MovingChangingPlatform movingChangingP =
            movingChangingPlatform.gameObject.GetComponent<MovingChangingPlatform>();
        SetMoveDirection(movingChangingP.Direction);
    }
}
