using NaughtyAttributes;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private Gun _gun;
    [SerializeField] private GunAnimation _gunAnimation;
    [SerializeField] private GunsList _gunsList;

    [SerializeField] private GameObject _gunPlace;

    private int _currentIndex = 0;
    [Button]
    public void SetGun(int gunIndex)
    {

        var oldGun = _gunPlace.transform.GetChild(0).gameObject;
        var newGunPrefab = _gunsList.list[gunIndex];
        var newGunInstance = Instantiate(newGunPrefab, oldGun.transform.position, oldGun.transform.rotation, _gunPlace.transform);
        var gunSettings = newGunInstance.GetComponent<GunSettings>();
        _gun.ImplementSettings(gunSettings);
        _gunAnimation.SetAnimator(gunSettings.GunAnimator);
        Destroy(oldGun);
    }

    public int UseNextGun()
    {
        _currentIndex++;
        if (_currentIndex > _gunsList.list.Count - 1)
        {
            _currentIndex = 0;
        }
        SetGun(_currentIndex);
        return _currentIndex;
    }

    void Start()
    {
        SetGun(_currentIndex);
    }
}
