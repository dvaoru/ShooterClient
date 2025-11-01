using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GunsList", menuName = "Guns List")]

public class GunsList : ScriptableObject
{
    [SerializeField] public List<GunSettings> list = new List<GunSettings>();
}
