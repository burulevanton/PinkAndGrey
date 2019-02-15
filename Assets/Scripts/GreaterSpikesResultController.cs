using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(_preAttackTime);
        _sprite.enabled = true;
        _boxCollider2D.enabled = true;
        yield return new WaitForSeconds(_attackTime);
        GreaterSpikeTriggerController.EnableTrigger();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
