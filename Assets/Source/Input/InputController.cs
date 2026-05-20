using UnityEngine;

namespace kuRomek.SimpleVG
{
    public class InputController : Installer
    {
        private static InputController _instance;
        private InputActions _actions;
        private bool _isInputBlocked;

        public static PlayerInput Current { get; private set; }

        protected override void Install()
        {
            if (_instance != null)
            {
                Debug.LogError($"Multiple instances of {nameof(InputController)} detected. Leaving the last instantiated one.");
                Destroy(_instance.gameObject);
            }

            _instance = this;

            _instance.gameObject.name = nameof(InputController);
        }

        private void Start()
        {
            Enable();

            _actions = new();
            _actions.Enable();
        }

        private void Update()
        {
            if (_isInputBlocked)
                return;

            Current = InputInterpreter.ReadInputValues(_actions);
        }

        private void OnDestroy()
        {
            _actions.Disable();
        }

        public static void Enable()
        {
            _instance._isInputBlocked = false;
        }

        public static void Disable()
        {
            _instance.ResetInputValues();
            _instance._isInputBlocked = true;
        }

        public void ResetInputValues()
        {
            Current = default;
        }
    }
}
