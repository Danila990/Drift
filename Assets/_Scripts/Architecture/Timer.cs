using UnityEngine;

namespace _Project
{
    public class Timer
    {
        public Timer(float duration)
        {
            _timeDuration = duration;
            _timeEnd = Time.time - 1;
        }

        public bool IsTimerEnd => Time.time > _timeEnd;

        private float _timeEnd;
        private float _timeDuration;

        public void StartTime()
        {
            _timeEnd = Time.time + _timeDuration;
        }
    }
}