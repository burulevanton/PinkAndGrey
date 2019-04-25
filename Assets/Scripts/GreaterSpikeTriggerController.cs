using ObjectPool;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class GreaterSpikeTriggerController : MonoBehaviour
{

    [SerializeField] private GreaterSpikesResultController GreaterSpikeResult;

    private BoxCollider2D _boxCollider2D;

    [SerializeField] private Vector2 direction;

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private void Start()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        GreaterSpikeResult.GreaterSpikeTriggerController = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _boxCollider2D.enabled = false;
        //var item = (GameObject) Instantiate(GreaterSpikeResultPrefab);
//        var item = PoolManager.SpawnObject(GreaterSpikeResultPrefab);
//        item.transform.parent = transform;
//        item.transform.position = transform.position;
//        var spikesResult = item.gameObject.GetComponent<GreaterSpikesResultController>();
        GreaterSpikeResult.gameObject.SetActive(true);
    }

    public void EnableTrigger()
    {
        _boxCollider2D.enabled = true;
    }
}
