using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [field: SerializeField] public Skins _skins;
    [field: SerializeField] public LossCounter _lossCounter { get; private set; }
    [field: SerializeField] public SpawnPoints _spawnPoints { get; private set; }
    [SerializeField] private PlayerCharacter _player;

    [SerializeField] private EnemyController _enemy;

    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async Task Connect()
    {

        _spawnPoints.GetPoint(UnityEngine.Random.Range(0, _spawnPoints.lenght), out Vector3 spawnPosition, out Vector3 spawnRotation);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"skins", _skins.lenght},
            {"points", _spawnPoints.lenght},
            {"speed", _player.speed},
            {"hp", _player.maxHealth},
            {"pX", spawnPosition.x},
            {"pY", spawnPosition.y},
            {"pZ", spawnPosition.z},
            {"rY", spawnRotation.y},
        };
        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        _room.OnStateChange += OnChage;

        _room.OnMessage<string>("Shoot", ApplyShoot);
        _room.OnMessage<string>("Change", ApplyChangeWeapon);

    }

    private void ApplyChangeWeapon(string jsonGunInfo)
    {
        GunInfo gunInfo = JsonUtility.FromJson<GunInfo>(jsonGunInfo);
        if (_enemies.ContainsKey(gunInfo.key) == false)
        {
            Debug.LogError("Enemy нет , а он пытался сменить оружие");
            return;
        }
        _enemies[gunInfo.key].ChangeGun(gunInfo.i);
    }

    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        if (_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("Enemy нет , а он пытался стрелять");
            return;
        }
        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnChage(State state, bool isFirstState)
    {
        if (isFirstState == false)
            return;
        state.players.ForEach(
            (key, player) =>
            {
                if (key == _room.SessionId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            }
        );

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;

    }

    private void CreatePlayer(Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        Quaternion rotation = Quaternion.Euler(0, player.rY, 0);
        var playerCharacter = Instantiate(_player, position, rotation);
        player.OnChange += playerCharacter.OnChange;
        _room.OnMessage<int>("Restart", playerCharacter.GetComponent<Controller>().Restart);
        playerCharacter.GetComponent<SetSkin>().Set(_skins.GetMaterial(player.skin));
    }


    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);
        _enemies.Add(key, enemy);
        enemy.GetComponent<SetSkin>().Set(_skins.GetMaterial(player.skin));
    }

    private void RemoveEnemy(string key, Player value)
    {
        if (_enemies.ContainsKey(key) == false) return;
        var enemy = _enemies[key];
        enemy.Destroy();
        _enemies.Remove(key);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room.Leave();
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {

        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionID() => _room.SessionId;
}
