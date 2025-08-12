using UnityEngine;
using System.Collections.Generic;

public class TrafficManager : MonoBehaviour
{
    [Header("Настройки трафика")]
    [SerializeField] private TrafficCar[] _carPrefabs;
    [SerializeField] private int _poolSize = 30;

    [Header("Точки для полосы 1 (вперёд)")]
    [SerializeField] private Transform _spawnPointForward;
    [SerializeField] private Transform _despawnPointForward;

    [Header("Точки для полосы 2 (назад)")]
    [SerializeField] private Transform _spawnPointBackward;
    [SerializeField] private Transform _despawnPointBackward;

    [Header("Рандомизация")]
    [SerializeField] private float _minSpawnInterval = 1.5f;
    [SerializeField] private float _maxSpawnInterval = 3.5f;

    private List<TrafficCar> _pool = new List<TrafficCar>();
    private float _nextSpawnTime;

    void Start()
    {
        // создаём пул
        for (int i = 0; i < _poolSize; i++)
        {
            var prefab = _carPrefabs[Random.Range(0, _carPrefabs.Length)];
            var car = Instantiate(prefab, transform);
            car.gameObject.SetActive(false);
            _pool.Add(car);
        }

        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnCar();
            ScheduleNextSpawn();
        }
    }

    void SpawnCar()
    {
        foreach (var car in _pool)
        {
            if (!car.gameObject.activeSelf)
            {
                // выбираем случайную полосу
                bool forward = Random.value > 0.5f;
                Transform spawn = forward ? _spawnPointForward : _spawnPointBackward;
                Transform despawn = forward ? _despawnPointForward : _despawnPointBackward;

                car.Activate(spawn, despawn);
                break;
            }
        }
    }

    void ScheduleNextSpawn()
    {
        _nextSpawnTime = Time.time + Random.Range(_minSpawnInterval, _maxSpawnInterval);
    }
}


