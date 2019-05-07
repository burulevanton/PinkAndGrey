using System;
using ObjectPool;
using UnityEngine;

public class EchoController : MonoBehaviour
{
  private PlayerController _playerController;
  public GameObject EchoPrefab;
  private float _timer;
  public float TimeBtwSpawn;  

  void Awake()
  {
    _playerController = GetComponent<PlayerController>();
  }
  private void Update()
  {
    if (_playerController.Moving)
    {
      if (_timer <= 0)
      {
        PoolManager.SpawnObject(EchoPrefab, transform.position, transform.rotation);
        _timer = TimeBtwSpawn;
      }
      else
        _timer -= Time.deltaTime;
    }
  }
}