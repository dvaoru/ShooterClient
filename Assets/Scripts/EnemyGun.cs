using UnityEngine;

public class EnemyGun : Gun
{
    public override void ImplementSettings(GunSettings gunSettings)
    {
        _bulletPrefab = gunSettings.BulletPrefab;
    }

    public void Shoot(Vector3 position, Vector3 velocity)
    {
        Instantiate(_bulletPrefab, position, Quaternion.identity).Init(velocity);
        shoot?.Invoke();
    }
}
