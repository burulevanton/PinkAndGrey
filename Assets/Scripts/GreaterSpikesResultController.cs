using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class GreaterSpikesResultController : MonoBehaviour
{
    public GreaterSpikeTriggerController GreaterSpikeTriggerController;
    private BoxCollider2D _boxCollider2D;

    [SerializeField] private float _preAttackTime = 0.5f;
    [SerializeField] private float _attackTime = 1.0f;
    private bool _isAttacked = false;
    private float _timer;

    private void Start()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        if (_timer >= 0.0f)
            _timer -= Time.deltaTime;
        else
        {
            if (_isAttacked)
            {
                GreaterSpikeTriggerController.EnableTrigger();
//                PoolManager.ReleaseObject(gameObject);
                gameObject.SetActive(false);
                _boxCollider2D.enabled = false;
            }
            else
            {
                UpdateRotation();
                _boxCollider2D.enabled = true;
                _isAttacked = true;
                _timer = _attackTime;
            }
        }
    }

//    IEnumerator LifeCycle()
//    {
//        yield return new WaitForSeconds(_preAttackTime);
//        UpdateRotation();
//        _sprite.enabled = true;
//        _boxCollider2D.enabled = true;
//        yield return new WaitForSeconds(_attackTime);
//        GreaterSpikeTriggerController.EnableTrigger();
//        PoolManager.ReleaseObject(gameObject);
//        _sprite.enabled = false;
//        _boxCollider2D.enabled = false;
//    }

    private void OnEnable()
    {
        _timer = _preAttackTime;
        _isAttacked = false;
        //StartCoroutine(LifeCycle());
    }

    private void UpdateRotation()
    {
        if (GreaterSpikeTriggerController.Direction.x != 0.0f)
        {
            this.transform.eulerAngles = new Vector3(0.0f, 0.0f, -90*GreaterSpikeTriggerController.Direction.x);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0.0f, 0.0f, GreaterSpikeTriggerController.Direction.y > 0 ? 0.0f : 180.0f);
        }
    }
}
