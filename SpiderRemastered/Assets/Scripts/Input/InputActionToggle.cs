using SFRemastered.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainbowArt.CleanFlatUI;

namespace SFRemastered
{
    public class InputActionToggle : MonoBehaviour
    {
        [SerializeField] private InputAction action;
        [SerializeField] private Switch _switch;
        [SerializeField] private KeyCode _keybind;

        private void Update()
        {
            if (_switch.IsOn)
                action.Pressing = true;
            else
                action.Pressing = false;

            if(Input.GetKeyDown(_keybind))
                _switch.IsOn = !_switch.IsOn;
        }
    }
}
