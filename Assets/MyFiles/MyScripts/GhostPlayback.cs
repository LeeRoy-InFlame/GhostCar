using System.Collections.Generic;
using UnityEngine;

public class GhostPlayback : MonoBehaviour
{
    private List<RecordedFrame> _frames;
    private float _startTime;
    private bool _isPlaying;

    public void Initialize(List<RecordedFrame> recorded)
    {
        if (recorded == null || recorded.Count < 2)
        {
            Debug.LogWarning("Недостаточно данных для призрака!");
            return;
        }

        _frames = new List<RecordedFrame>(recorded);
        transform.position = _frames[0].Position;
        transform.rotation = _frames[0].Rotation;

        _startTime = Time.time;
        _isPlaying = true;
    }

    void Update()
    {
        if (!_isPlaying || _frames == null) return;

        float elapsed = Time.time - _startTime;

        // Если доехали до конца — стоп
        if (elapsed >= _frames[_frames.Count - 1].Time)
        {
            transform.position = _frames[_frames.Count - 1].Position;
            transform.rotation = _frames[_frames.Count - 1].Rotation;
            _isPlaying = false;
            return;
        }

        // Находим два кадра в данный момент
        int i = 0;
        while (i < _frames.Count - 1 && _frames[i + 1].Time < elapsed) i++;

        var a = _frames[i];
        var b = _frames[i + 1];

        float t = Mathf.InverseLerp(a.Time, b.Time, elapsed);
        transform.position = Vector3.Lerp(a.Position, b.Position, t);
        transform.rotation = Quaternion.Slerp(a.Rotation, b.Rotation, t);
    }
}

