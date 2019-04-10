using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class GreaterSpikesResultController : MonoBehaviour
{
    public GreaterSpikeTriggerController GreaterSpikeTriggerController;
    private SpriteRenderer _sprite;
    private BoxCollider2D _boxCollider2D;

    [SerializeField] private float _preAttackTime = 0.5f;
    [SerializeField] private float _attackTime = 1.0f;

    private void Start()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(_preAttackTime);
        UpdateRotation();
        _sprite.enabled = true;
        _boxCollider2D.enabled = true;
        yield return new WaitForSeconds(_attackTime);
        GreaterSpikeTriggerController.EnableTrigger();
        PoolManager.ReleaseObject(gameObject);
        _sprite.enabled = false;
        _boxCollider2D.enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(LifeCycle());
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
