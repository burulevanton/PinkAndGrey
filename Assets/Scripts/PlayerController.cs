using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enum;
using ObjectPool;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    
    private Vector2 _moveDirection = new Vector2(0.0f, 0.0f);
    private Vector2 nextSwipeDirection = Vector2.zero;
    private float moveSpeed = 30f;
    private float rotationZ;
    private float scaleX = 1f;
    private float nextSwipeTimeout;
    private float tileSize = 1.0f;

    public MovingPlatform OnMovingPlatform;


    [SerializeField] private bool _isReverse;
    private Transform _parent;
    private GameController gc;
    private bool _tutorialPass;
    
    private void OnEnable()
    {
        OnMovingPlatform = null;
        transform.parent = _parent;
        if (_tutorialPass)
        {
            SwipeController.OnSwipe += new Action<EDirection>(this.ProcessSwipe);
        }
        else
        {
            TutorialController.OnSwipe += new Action<EDirection>(this.ProcessSwipe);
        }
    }

    private void OnDisable()
    {
        if (_tutorialPass)
        {
            SwipeController.OnSwipe -= new Action<EDirection>(this.ProcessSwipe);
        }
        else
        {
            TutorialController.OnSwipe -= new Action<EDirection>(this.ProcessSwipe);
        }
    }

    public bool Stopped => this._moveDirection == Vector2.zero;

    public bool Moving => !this.Stopped;

    private Animator _animator;

    private bool _alive = true;

    void Awake()
    {
        _tutorialPass = GameData.Instance.TutorialPass;
        if (_tutorialPass)
        {
            GameController.Instance.PlayerController = this;
        }
        UpdateScaleRotation(1.0f,0.0f);
        _parent = transform.parent;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void IsAlive()
    {
//        if(!_alive)   
//            _animator.SetTrigger("IsAlive");
//        _animator.ResetTrigger("Fly");
        _animator.Play("Player_Idle");
        _moveDirection = Vector2.zero;
        OnMovingPlatform = null;
    }

    private void FixedUpdate()
    {
        if (_tutorialPass && GameController.Instance.IsPaused)
            return;
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
                            StopByTransform(raycastHit2D.point);
                            break;
                        case "MovingPlatform":
                            isStopped = true;
                            StopByMovingTransform(raycastHit2D.point, raycastHit2D.collider.transform);
                            break;
                        case "BreakingPlatform":
                            isStopped = true;
                            StopByTransform(raycastHit2D.point);
                            break;
                        case "LaserTrapPlatform":
                            isStopped = true;
                            StopByTransform(raycastHit2D.point, bigTransform:true);
                            break;
                        case "TimerWall":
                            var timerWallController = raycastHit2D.collider.gameObject.GetComponent<TimerWallController>();
                            if (timerWallController.IsActivated)
                            {
                                timerWallController.PlayerLand();
                                isStopped = true;
                                StopByTransform(raycastHit2D.point);
                                break;
                            }
                            break;
                }
            }

            if (isStopped)
            {
                position = (Vector2) this.transform.position;
            }
            else
                position += vector2;
        }

        this.transform.position = (Vector3) position;
        if (this.OnMovingPlatform != null && !this.OnMovingPlatform.IsPause )
        {
            Vector2 vectorSpeedPlatform = this.OnMovingPlatform.MoveSpeed * Time.deltaTime * this.OnMovingPlatform.Direction;
            float distancePlatform =
            Mathf.Abs((double) vectorSpeedPlatform.x == 0.0 ? vectorSpeedPlatform.y : vectorSpeedPlatform.x);
            var raycastHit2d = Physics2D.Raycast(position+this.OnMovingPlatform.Direction*0.5f,
            this.OnMovingPlatform.Direction, distancePlatform);
            if (raycastHit2d.collider != null)
            {
                if (OnMovingPlatform.IsEndOfJourneyPlayer(raycastHit2d.point))
                    return;
                Debug.Log(raycastHit2d.collider);
                switch (raycastHit2d.collider.tag)
                {
                    case "Wall":
                        StopFromMovingPlatform(raycastHit2d.point);
                        break;
                    case "BreakingPlatform":
                        StopFromMovingPlatform(raycastHit2d.point);
                        break;
                    case "LaserTrapPlatform":
                        StopFromMovingPlatform(raycastHit2d.point);
                        break;
                    case "TimerWall":
                        var timerWallController = raycastHit2d.collider.gameObject.GetComponent<TimerWallController>();
                        if (timerWallController.IsActivated)
                        {
                            timerWallController.PlayerLand();
                            StopFromMovingPlatform(raycastHit2d.point);
                            break;
                        }
                        break;
                }
            }
        }
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
    
    public bool JumpWithDirection(Vector2 direction)
    {
        direction = _isReverse ? -direction : direction;
        if (!this.CanJumpWithDirection(direction))
            return false;
        this.transform.parent = _parent;
        this.OnMovingPlatform = null;
        //this.transform.parent = gc.transform; //todo сделать что-то с этим
        this.SetMoveDirection(direction);
        return true;
    }
    
    private void SetMoveDirection(Vector2 direction)
    {
        this._moveDirection = direction;
        if (!this.Moving)
            return;
        _animator.SetTrigger("Fly");
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

    private Vector2 SetStopPosition(Vector2 PointPosition)
    {
        PointPosition = new Vector2(
                        Mathf.Round(PointPosition.x * 10f) / 10f,
                        Mathf.Round(PointPosition.y * 10f) / 10f);
        if ((PointPosition.x / 0.5f % 2f == 0.0f || PointPosition.x % 0.5f != 0.0f) && _moveDirection.y !=0.0f)
            return new Vector2((float)Math.Truncate(PointPosition.x) + _moveDirection.y*0.5f, PointPosition.y);
        if ((PointPosition.y / 0.5f % 2f == 0.0f  || PointPosition.y % 0.5f != 0.0f) && _moveDirection.x !=0.0f)
            return new Vector2(PointPosition.x, (float)Math.Truncate(PointPosition.y) + _moveDirection.x*0.5f);
        return PointPosition;
    }

    private void StopFromMovingPlatform(Vector2 PointPosition)
    {
        this.transform.parent = _parent;
        PointPosition = SetStopPosition(PointPosition);
        PointPosition -= this.OnMovingPlatform.Direction * 0.5f;
        this.transform.position = (Vector3) PointPosition;
        var rotation = this.OnMovingPlatform.Direction.x != 0
        ? this.OnMovingPlatform.Direction.x * 90f
        : this.OnMovingPlatform.Direction.y > 0
        ? 180f : 0f;
        this.UpdateScaleRotation(this.scaleX, rotation);
        this.OnMovingPlatform = null;
        _animator.SetTrigger("Fall");
        _animator.ResetTrigger("Fly");
    }

    private void StopByTransform(Vector2 Pointposition, bool setStopperPosition = false, bool bigTransform = false)
    {
        if (this.Stopped)
            return;
//            Pointposition -= this._moveDirection * (bigTransform ? this.tileSize * 0.5f : this.tileSize);
        this.transform.parent = _parent;
        Pointposition = SetStopPosition(Pointposition);
        Pointposition -= this._moveDirection * 0.5f;
        this.transform.position = (Vector3) Pointposition;    
        this.OnMovingPlatform = null;
        this.UpdateScaleRotation(this.scaleX, this.rotationZ + 180f);
        this._moveDirection = Vector2.zero;
        _animator.SetTrigger("Fall");
        _animator.ResetTrigger("Fly");
        if ((double) this.nextSwipeTimeout <= 0.0)
            return;
        this.JumpWithDirection(this.nextSwipeDirection);
        this.nextSwipeTimeout = 0.0f;
    }

    private void StopByMovingTransform(Vector2 pointPosition, Transform transform)
    {
        if (this.Stopped)
            return;
//        pointPosition = SetStopPosition(pointPosition);
//        Vector2 position = (Vector2) pointPosition - this._moveDirection * 0.5f;
//        this.transform.position = (Vector3) position;
        this.OnMovingPlatform = transform.gameObject.GetComponent<MovingPlatform>();
        this.UpdateScaleRotation(this.scaleX, this.rotationZ + 180f);
        this.transform.parent = transform;
        this.transform.localPosition = -(Vector3) this._moveDirection;
        this._moveDirection = Vector2.zero;
        _animator.SetTrigger("Fall");
        _animator.ResetTrigger("Fly");
        if(this.nextSwipeTimeout <= 0.0f)
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
                case "LaserTrap":
                    DamageTaken(other.gameObject);
                    break;
                case "Enemy":
                    DamageTaken(other.gameObject);
                    break;
                case "Projectile":
                    DamageTaken(other.gameObject);
                    break;
                case "MovingChangingPlatform":
                    if(Stopped)
                        break;
                    ChangeDirectionWithMovingChangingPlatform(other.gameObject);
                    break;
                case "LevelEnd":
                    LevelEnd();
                    break;
                case "LaserTrapTrigger":
                    var laserTrapController = other.gameObject.GetComponentInParent<LaserTrapController>();
                    laserTrapController.ActivateLaserTrap();
                    break;
                case "Spike":
                    DamageTaken(other.gameObject);
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
                {
                    timerWallController.PlayerExit();
                    break;
                }
                timerWallController.ActivateWall();
                break;
            case "CopyingPortal":
                CopyingPortalController copyingPortalController =
                    other.gameObject.GetComponent<CopyingPortalController>();
                if (copyingPortalController.IsActivated)
                {
                    copyingPortalController.ActivatePortal(_moveDirection);
                }
                break;
            case "BreakingPlatform":
                BreakingPlatformController breakingPlatformController =
                    other.gameObject.GetComponent<BreakingPlatformController>();
                breakingPlatformController.BreakPlatform();
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
        GameController.Instance.Pause();
        _alive = false;
        _moveDirection = Vector2.zero;
        _animator.SetTrigger("Death");
        _animator.ResetTrigger("Fly");
        _animator.ResetTrigger("Fall");
    }

    private void EndDamageAnimation()
    {
        GameController.Instance.PlayerDeath();
    }

    private void ChangeDirectionWithMovingChangingPlatform(GameObject movingChangingPlatform)
    {
        transform.position = movingChangingPlatform.transform.position;
        MovingChangingPlatform movingChangingP =
            movingChangingPlatform.gameObject.GetComponent<MovingChangingPlatform>();
        SetMoveDirection(movingChangingP.Direction);
    }

    private void LevelEnd()
    {
        this._moveDirection = Vector2.zero;
        _animator.Play("PlayerLevelPass");
    }

    public void LevelPassedAnimation()
    {
//        _animator.SetTrigger("Fall");
//        _animator.ResetTrigger("Fly");
        if (_tutorialPass)
        {
            GameUIController.Instance.LevelPassed();
        }
        else
        {
            GameData.Instance.PassTutorial(1);
            SceneManager.LoadScene("Menu");
        }
    }
}
