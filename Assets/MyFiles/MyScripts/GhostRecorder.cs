using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RecordedFrame
{
    public Vector3 Position;
    public Quaternion Rotation;
    public float Time;

    public RecordedFrame(Vector3 pos, Quaternion rot, float t)
    {
        Position = pos;
        Rotation = rot;
        Time = t;
    }
}

public class GhostRecorder : MonoBehaviour
{
    [SerializeField] private float _recordInterval = 0.05f; // Частота записи
    private float _timer;
    private float _startTime;
    private bool _isRecording;

    public List<RecordedFrame> RecordedFrames = new List<RecordedFrame>();

    public void StartRecording()
    {
        RecordedFrames.Clear();
        _startTime = Time.time;
        _timer = 0f;
        _isRecording = true;
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    void Update()
    {
        if (!_isRecording) return;

        _timer += Time.deltaTime;
        if (_timer >= _recordInterval)
        {
            float t = Time.time - _startTime;
            RecordedFrames.Add(new RecordedFrame(transform.position, transform.rotation, t));
            _timer = 0f;
        }
    }
}

