using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthUi _healthUi;
    [SerializeField] private int _max;
    [SerializeField] private int _current;

    public void SetMax(int value)
    {
        _max = value;
        UpdateHp();
    }

    public void SetCurrent(int value)
    {
        _current = value;
        UpdateHp();
    }

    public void ApplyDamage(int damage)
    {
        _current -= damage;
        UpdateHp();
    }
    
    private void UpdateHp()
    {
        _healthUi.UpdateHealth(_max, _current);
    }
}
