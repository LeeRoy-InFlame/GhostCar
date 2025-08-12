using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _despawnPoint;
    [SerializeField] private float _despawnThreshold = 1f; // расстояние для деспавна

    private bool _isActive = false;
    private Vector3 _moveDirection;
    private Vector3 _spawnPosition;

    public void Activate(Transform spawnPoint, Transform targetPoint)
    {
        transform.position = spawnPoint.position;
        // игнорируем Y, чтобы машина не "смотрела" в небо при разной высоте
        Vector3 dir = (targetPoint.position - spawnPoint.position);
        dir.y = 0f;
        _moveDirection = dir.normalized;

        // поставить ориентацию машины по направлению движения
        if (_moveDirection.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(_moveDirection);

        _despawnPoint = targetPoint;
        _spawnPosition = spawnPoint.position;
        _isActive = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!_isActive) return;

        transform.position += _moveDirection * _speed * Time.deltaTime;

        // Быстрая проверка на деспавн (по квадрату расстояния)
        float sqrDist = (transform.position - _despawnPoint.position).sqrMagnitude;
        if (sqrDist <= _despawnThreshold * _despawnThreshold)
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}



