using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private int _damage;

    [SerializeField] private int _headDamage = 10;
    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rigidbody.linearVelocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Head"))
        {
            var enemyHead = collision.collider.GetComponentInParent<EnemyCharacter>();
            if (enemyHead != null)
            {
                enemyHead.ApplyDamage(_headDamage, collision.GetContact(0).point, true);
            }
        }
        else
        if (collision.collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(_damage, collision.GetContact(0).point, false);
        }
        Destroy();
    }
}
