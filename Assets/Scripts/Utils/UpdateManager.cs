using System;
using UnityEngine;


namespace ExampleGB
{
    public class UpdateManager : MonoBehaviour
    {
        private static event Action OnUpdateEvent;
        private static event Action OnFixedUpdateEvent;
        private static event Action OnLateUpdateEvent;

        private static bool _isPaused = false;

        public static void Pause()
        {
            _isPaused = !_isPaused;
            Time.timeScale = (_isPaused == true) ? 0 : 1;
        }

        public static void SubscribeToUpdate(Action callback)
        {
            OnUpdateEvent += callback;
        }

        public static void SubscribeToFixedUpdate(Action callback)
        {
            OnFixedUpdateEvent += callback;
        }

        public static void SubscribeToLateUpdate(Action callback)
        {
            OnLateUpdateEvent += callback;
        }

        public static void UnsubscribeFromUpdate(Action callback)
        {
            OnUpdateEvent -= callback;
        }

        public static void UnsubscribeFromFixedUpdate(Action callback)
        {
            OnFixedUpdateEvent -= callback;
        }

        public static void UnsubscribeFromLateUpdate(Action callback)
        {
            OnLateUpdateEvent -= callback;
        }

        private void Update()
        {
            if (OnUpdateEvent != null && !_isPaused) OnUpdateEvent.Invoke();
        }

        private void FixedUpdate()
        {
            if (OnFixedUpdateEvent != null && !_isPaused) OnFixedUpdateEvent.Invoke();
        }

        private void LateUpdate()
        {
            if (OnLateUpdateEvent != null && !_isPaused) OnLateUpdateEvent.Invoke();
        }
    }
}
