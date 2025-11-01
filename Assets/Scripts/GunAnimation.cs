using System;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string shoot = "Shoot";
    [SerializeField] private Gun _gun;
    [SerializeField] private Animator _animator;

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    void Start()
    {
        _gun.shoot += Shoot;
    }

    void OnDestroy()
    {
        _gun.shoot -= Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(shoot);
    }



}
