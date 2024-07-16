using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace SFRemastered.InputSystem
{
    public class InputActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private InputAction action;
        [SerializeField] private KeyCode keyBind;

        private void Update()
        {
            if(Input.GetKeyDown(keyBind))
            {
                OnPointerDown(null);
            }

            if(Input.GetKeyUp(keyBind))
            {
                OnPointerUp(null);
            }    
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            action.Down = true;
            action.Pressing = true;
            StartCoroutine(IE_ResetPointerDown(1));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            action.Up = true;
            action.Pressing = false;
            StartCoroutine(IE_ResetPointerUp(1));
        }

        private IEnumerator IE_ResetPointerDown(int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }

            action.Down = false;
        }

        private IEnumerator IE_ResetPointerUp(int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }

            action.Up = false;
        }

        public void Clear()
        {
            StopAllCoroutines();
            action.Clear();
        }
    }
}
