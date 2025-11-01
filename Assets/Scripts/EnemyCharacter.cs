using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character


{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;
    private string _sessionId;
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;

    private RotatePredictor _rotatePredictorX = new RotatePredictor();
    private RotatePredictor _rotatePredictorY = new RotatePredictor();



    public void Init(string sessionID)
    {
        _sessionId = sessionID;
    }
    void Start()
    {
        targetPosition = transform.position;
    }


    void Update()
    {
        if (_velocityMagnitude > .1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        }
        else
        {
            transform.position = targetPosition;
        }

        _head.localEulerAngles = new Vector3(_rotatePredictorX.GetTargetRotation(_head.localEulerAngles.x), 0, 0);
        transform.localEulerAngles = new Vector3(0, _rotatePredictorY.GetTargetRotation(transform.localEulerAngles.y), 0);
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void SetMaxHP(int value)
    {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }


    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        targetPosition = position + velocity * Mathf.Clamp(averageInterval, 0, 0.2f);
        _velocityMagnitude = velocity.magnitude;

        this.velocity = velocity;
    }

    public void SetRotateX(float value)
    {
        Debug.Log("Set rotate X " + Time.time + " " + value);
        // _head.localEulerAngles = new Vector3(value, 0, 0);
        _rotatePredictorX.SetRotate(value);
    }

    public void SetRotateY(float value)
    {
        Debug.Log("Set rotate Y " + Time.time + " " + value);
        _rotatePredictorY.SetRotate(value);
        //transform.localEulerAngles = new Vector3(0, value, 0);
    }

    private class RotatePredictor
    {
        private float _lastRotate;
        private float _lastTime;
        private float _angularSpeed = 0;

        public void SetRotate(float currentRotation)
        {
            float time = Time.time;

            float deltaTime = time - _lastTime;
            if (deltaTime > 0.01f)
            {
                float deltaAngle = Mathf.DeltaAngle(_lastRotate, currentRotation);
                _angularSpeed = deltaAngle / deltaTime; //Расчитываем угловую скорость при повороте от предыдущего значения
            }

            _lastRotate = currentRotation;
            _lastTime = time;
        }

        public float GetTargetRotation(float currentRotation)
        {
            var predictionTime = 0.1f; //Предсказываем поворот на 100 мс
            float smoothSpeed = 180f; //Для сглаживания делаем скорость поворота 180 град/сек
            float predicted = _lastRotate + _angularSpeed * predictionTime;
            return Mathf.MoveTowardsAngle(currentRotation, predicted, smoothSpeed * Time.deltaTime);
        }
    }

    public void ApplyDamage(int damage)
    {
        _health.ApplyDamage(damage);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"id", _sessionId},
            {"value", damage}
        };
        MultiplayerManager.Instance.SendMessage("damage", data);
    }

    internal void RestoreHP(int newValue)
    {
        _health.SetCurrent(newValue);
    }
}
