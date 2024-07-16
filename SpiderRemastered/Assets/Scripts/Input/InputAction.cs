using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered.InputSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/InputAction")]
    public class InputAction : ScriptableObject
    {
        public bool Pressing 
        {  
            get 
            {  
                return _pressing; 
            } 
            set
            {
                _pressing = value;
            }
        }
        public bool Down
        {
            get
            {
                return _down;
            }
            set
            {
                _down = value;
            }
        }
        public bool Up
        {
            get
            {
                return _up;
            }
            set
            {
                _up = value;
            }
        }

        private bool _pressing;
        private bool _down;
        private bool _up;

        public void Clear()
        {
            Pressing = false;
            Down = false;
            Up = false;
        }

        public void Cancel()
        {
            InputManager.instance.StartCoroutine(IE_Cancel());
        }

        private IEnumerator IE_Cancel()
        {
            Pressing = false;
            Down = false;
            Up = true;
            yield return null;
            Up = false;
        }
    }
}
