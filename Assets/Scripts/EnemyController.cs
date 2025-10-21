using System;
using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    internal void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;
        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "x":
                    position.x = (float)dataChange.Value;
                    break;
                case "y":
                    position.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.LogWarning("Не обрабатывается поле " + dataChange.Field);
                    break;
            }
        }

        transform.position = position;
    }
}
