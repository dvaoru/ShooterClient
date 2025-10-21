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

        //transform.position = position;
        //Обновляем текущюю позицию, предыдущюю записываем
        if (currentPositionAndTime != null)
            prevPositionAndTime = currentPositionAndTime;
        currentPositionAndTime = new PositionAndTime(position, Time.time);
    }

    private PositionAndTime prevPositionAndTime = null;
    private PositionAndTime currentPositionAndTime = null;

    public void Update()
    {
        if (currentPositionAndTime == null) //Если текущей позиции нет, то пропускаем
            return;
        if (prevPositionAndTime == null) //Если предыдущей позиции нет, то выставляем в текущюю позицию
        {
            transform.position = currentPositionAndTime.position;
        }

        if (currentPositionAndTime.isFresh) //Если текущая позиция ни разу не использовалась, выставляем в нее, и помечаем, что она использована
        {
            transform.position = currentPositionAndTime.position;
            currentPositionAndTime.isFresh = false;
            return;
        }

        var dTime = Time.time - currentPositionAndTime.time;
        if (dTime > 0.2f) // Если более чем 200мс нет новой позиции то возвращаемся на последнюю извесную
        {
            transform.position = currentPositionAndTime.position;
            return;
        }

        //Расчитываем направление и скорость на основе предыдущей позиции
        //var deltaVector = currentPositionAndTime.position - prevPositionAndTime.position;
        var dX = currentPositionAndTime.position.x - prevPositionAndTime.position.x;
        var dY = currentPositionAndTime.position.y - prevPositionAndTime.position.y;
        Debug.Log("delta " + dX + " : " + dY);
        var deltaTime = currentPositionAndTime.time - prevPositionAndTime.time;
        Debug.Log("deltaTime = " + deltaTime);
        var speedX = 0f;
        var speedY = 0f;
        if (deltaTime > 0.01)
        {
            speedX = dX / deltaTime;
            speedY = dY / deltaTime;
        }

        var newPosition =
            currentPositionAndTime.position + new Vector3(speedX * dTime, 0, speedY * dTime);
        transform.position = newPosition;
    }

    private class PositionAndTime
    {
        public Vector3 position;
        public float time;

        public bool isFresh = true;

        public PositionAndTime(Vector3 position, float time)
        {
            this.position = position;
            this.time = time;
        }
    }
}
