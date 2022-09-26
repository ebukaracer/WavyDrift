﻿using UnityEngine;

internal class ObstacleController : MonoBehaviour
{
    private int _playerDistance;

    private int _playerDistanceIndex = -1;

    private Transform _player;

    [SerializeField] private ObstacleBunchSpawner obstacleBunchSpawner;

    private void Start()
    {
        _player = PlayerController.Instance.PlayerMovement.transform;
    }

    private void Update()
    {
        _playerDistance = (int)_player.transform.position.z / obstacleBunchSpawner.NextSpawnPos;

        //Debug.Log($"Player Distance: {playerDistance}, Player Distance Index: {playerDistanceIndex}");

        if (_playerDistanceIndex != _playerDistance)
        {
            obstacleBunchSpawner.SpawnObstacleBunch();

            _playerDistanceIndex = _playerDistance;
        }
    }
}
