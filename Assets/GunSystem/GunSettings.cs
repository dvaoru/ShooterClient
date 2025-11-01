using UnityEngine;
public class GunSettings : MonoBehaviour
{
    [SerializeField] public float ShootDelay = 0.5f;
    [SerializeField] public int Damage = 10;
    [SerializeField] public float BulletSpeed = 20f;
    [SerializeField] public Bullet BulletPrefab;

    [SerializeField] public Transform BulletPoint;

    [SerializeField] public Animator GunAnimator;

}

