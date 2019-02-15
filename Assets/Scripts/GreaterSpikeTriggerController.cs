using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterSpikeTriggerController : MonoBehaviour
{

    [SerializeField] private GameObject GreaterSpikeResultPrefab;

    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _boxCollider2D.enabled = false;
        var item = (GameObject) Instantiate(GreaterSpikeResultPrefab);
        item.transform.parent = transform.parent.parent;
        item.transform.position = transform.position;
        var spikesResult = item.gameObject.GetComponent<GreaterSpikesResultController>();
        spikesResult.GreaterSpikeTriggerController = this;
    }

    public void EnableTrigger()
    {
        _boxCollider2D.enabled = true;
    }
}
