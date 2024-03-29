﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;
using Unity.Netcode;

namespace Pantheon {
  public class GlobalContext : Singleton<GlobalContext> {
    public const float MaxPlayerMoveSpeed = 6;

    public string MechanicPath { get; set; }

    public ReadOnlyCollection<NetworkPlayer> Players {
      get { return _players.AsReadOnly(); }
    }

    public NetworkPlayer LocalPlayer {
      get { return _localPlayer; }
    }

    public event Action<NetworkPlayer> OnPlayerAdded;
    public event Action<NetworkPlayer> OnPlayerRemoved;

    private Dictionary<int, EnemyController> _enemies = new Dictionary<int, EnemyController>();
    private List<NetworkPlayer> _players = new List<NetworkPlayer>();
    private NetworkPlayer _localPlayer;

    [SerializeField]
    private GameObject _playerPrefab;

    public EnemyController GetEnemyById(int id) {
      return _enemies.GetValueOrDefault(id, null);
    }

    public void RegisterEnemy(int id, EnemyController enemy) {
      _enemies.Add(id, enemy);
    }

    public void UnregisterEnemy(int id) {
      _enemies.Remove(id);
    }

    public void RegisterPlayer(NetworkPlayer player) {
      _players.Add(player);
      if (player.IsLocalPlayer) {
        _localPlayer = player;
      }
      OnPlayerAdded?.Invoke(player);
    }

    public void UnregisterPlayer(NetworkPlayer player) {
      _players.Remove(player);
      if (player == _localPlayer) {
        _localPlayer = null;
      }
      OnPlayerRemoved?.Invoke(player);
    }

    public NetworkPlayer SpawnAiPlayer() {
      var player = Instantiate(_playerPrefab).GetComponent<NetworkPlayer>();
      player.gameObject.AddComponent<AutoPilot>();
      player.GetComponent<NetworkObject>().Spawn(true);
      return player;
    }
  }
}
