using UnityEngine;

public class HealthUi : MonoBehaviour
{
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defoaltWidth;

    
    void OnValidate()
    {
        _defoaltWidth = _filledImage.sizeDelta.x;
    }
    public void UpdateHealth(float max, int current)
    {
        float percent = current / max;
        _filledImage.sizeDelta = new Vector2(_defoaltWidth * percent, _filledImage.sizeDelta.y);
    }
}
