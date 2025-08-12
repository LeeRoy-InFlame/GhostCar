using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ashsvp;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private GhostRecorder _playerRecorder;
    [SerializeField] private RaceTimer _raceTimer;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private Transform _startPoint;                // точка старта трассы (назначить в инспекторе)

    private SimcadeVehicleController _vehicleController;
    private List<RecordedFrame> _lastLapData;
    private GameObject _currentGhost;

    private bool _waitingForStart = true;
    private bool _raceInProgress = false;

    void Start()
    {
        _vehicleController = _playerRb.GetComponent<SimcadeVehicleController>();

        _uiManager.ShowStartText(true);
        _uiManager.ShowFinishPanel(false);

        // Сразу в стартовую позицию и "заморозить" машину до старта
        if (_startPoint != null)
        {
            _playerRb.transform.position = _startPoint.position;
            _playerRb.transform.rotation = _startPoint.rotation;
        }

        // полностью остановить физику и отключить контроллер до старта
        // (обнуление скоростей)
        _playerRb.linearVelocity = Vector3.zero;
        _playerRb.angularVelocity = Vector3.zero;
        _playerRb.isKinematic = true;

        if (_vehicleController != null) _vehicleController.enabled = false;
    }

    void Update()
    {
        if (_waitingForStart && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            StartRace();
        }
    }

    public void StartRace()
    {
        _raceTimer.StartTimer();
        _waitingForStart = false;
        _raceInProgress = true;
        _uiManager.ShowStartText(false);

        // включаем контроллер и разрешаем ехать
        if (_vehicleController != null)
        {
            _vehicleController.enabled = true;
            _vehicleController.CanDrive = true;
            _vehicleController.CanAccelerate = true;
            _vehicleController.isFinishing = false;
        }

        // начать запись
        _playerRb.isKinematic = false;
        _playerRb.linearVelocity = Vector3.zero;
        _playerRb.angularVelocity = Vector3.zero;

        _playerRecorder.StartRecording();

        // уничтожаем предыдущего призрака (если есть) и создаём нового из lastLapData
        if (_currentGhost != null)
        {
            Destroy(_currentGhost);
            _currentGhost = null;
        }

        if (_lastLapData != null && _lastLapData.Count > 0)
        {
            _currentGhost = Instantiate(_ghostPrefab, _lastLapData[0].Position, _lastLapData[0].Rotation);
            var gp = _currentGhost.GetComponent<GhostPlayback>();
            if (gp != null) gp.Initialize(_lastLapData);
        }
    }

    public void FinishRace()
    {
        _raceTimer.StopTimer();
        _raceInProgress = false;

        // скажем контроллеру тормозить
        if (_vehicleController != null)
        {
            _vehicleController.isFinishing = true;
            _vehicleController.CanDrive = false;
            _vehicleController.CanAccelerate = false;
        }

        // ждём пока машина сама затормозит
        StartCoroutine(WaitForStopAndShowFinish());
    }

    private IEnumerator WaitForStopAndShowFinish()
    {
        // ждём пока скорость станет маленькой
        while (_playerRb.linearVelocity.magnitude > 0.15f)
        {
            yield return null;
        }

        // окончательно остановим
        _playerRb.linearVelocity = Vector3.zero;
        _playerRb.angularVelocity = Vector3.zero;

        // остановка записи и сохранение данных для следующего призрака
        _playerRecorder.StopRecording();
        _lastLapData = new List<RecordedFrame>(_playerRecorder.RecordedFrames);

        // отключаем контроллер
        if (_vehicleController != null) _vehicleController.enabled = false;

        _uiManager.ShowFinishPanel(true);
    }

    // кнопка "Соревноваться с призраком"
    public void OnCompeteWithGhost()
    {
        _raceTimer.ResetTimer();
        _uiManager.ShowFinishPanel(false);
        _waitingForStart = true;
        _uiManager.ShowStartText(true);

        // удаляем старого призрака
        if (_currentGhost != null)
        {
            Destroy(_currentGhost);
            _currentGhost = null;
        }

        // телепортируем игрока на старт и полностью его "замораживаем" до старта
        if (_startPoint != null)
        {
            // отключаем контроллер если вдруг включён
            if (_vehicleController != null) _vehicleController.enabled = false;

            // обнуляем скорости
            _playerRb.linearVelocity = Vector3.zero;
            _playerRb.angularVelocity = Vector3.zero;

            _playerRb.transform.position = _startPoint.position;
            _playerRb.transform.rotation = _startPoint.rotation;

            _playerRb.isKinematic = true;
        }
    }
}