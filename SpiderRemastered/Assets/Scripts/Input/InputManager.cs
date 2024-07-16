using SFRemastered.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace SFRemastered.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        public float camsensitivity = 1f;

        [Header("Input modules")]
        public VariableJoystick joystickMove;
        public GameObject lookPanel;

        [Header("Input value(Readonly)")]
        public float timeScale;
        public Vector2 move;
        public Vector2 look;
        public bool isLooking;

        [Header("Input Actions")]
        public InputAction jump;
        public InputAction sprint;

        public bool disableInput;

        private Vector2 cachedTouchPos;
        private int lookFingerID;
        private Vector2 targetLook;

        private float pcCamSenmultiplier = 1;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void DisableInput(bool disable)
        {
            disableInput = disable;
        }

        private void Start()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Cursor.lockState = CursorLockMode.Locked;
#endif
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.H))
                Debug.Break();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            look = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
            look *= (camsensitivity * pcCamSenmultiplier);

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Cursor.lockState == CursorLockMode.None)
            {
                move = Vector2.zero;
                look = Vector2.zero;
            }

            if (Input.GetKeyDown(KeyCode.PageUp))
                pcCamSenmultiplier += 0.2f;
            if (Input.GetKeyUp(KeyCode.PageDown))
            {
                pcCamSenmultiplier -= 0.2f;
                if (pcCamSenmultiplier <= 0)
                    pcCamSenmultiplier = 0.2f;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                TimeManager.instance.focus = true;
            }
            else
            {
                TimeManager.instance.focus = false;
            }

#endif

            if (disableInput)
            {
                if (joystickMove.gameObject.activeInHierarchy)
                {
                    joystickMove.gameObject.SetActive(false);
                    joystickMove.OnPointerUp(null);
                    lookPanel.SetActive(false);
                }
                move = Vector2.zero;
                look = Vector2.zero;
                jump.Cancel();
                sprint.Cancel();
                return;
            }

            if (!joystickMove.gameObject.activeInHierarchy)
            {
                joystickMove.gameObject.SetActive(true);
                lookPanel.SetActive(true);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
        move = new Vector2(joystickMove.Horizontal, joystickMove.Vertical);

        if(Input.touchCount == 0)
        {
            isLooking = false;
            lookFingerID = -1;
            targetLook = Vector2.zero;
        }
        else
        {
            if (isLooking && lookFingerID != -1)
            {
                int index = -1;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).fingerId == lookFingerID)
                    {
                        index = i;
                        break;
                    }
                }

                if(index >= 0)
                {
                    Vector2 touchDelta = Input.GetTouch(index).position - cachedTouchPos;
                    cachedTouchPos = Input.GetTouch(index).position;
                    touchDelta = touchDelta / Time.deltaTime;
                    touchDelta /= (140f / camsensitivity);
                    touchDelta = Vector3.ClampMagnitude(touchDelta, 7f);
                    targetLook.x = Mathf.Abs(touchDelta.y) > 0.3f ? touchDelta.y : 0f;
                    targetLook.y = Mathf.Abs(touchDelta.x) > 0.3f ? touchDelta.x : 0f;
                }
                else
                {
                    targetLook = Vector2.zero;
                }
            }
            else
            {
                targetLook = Vector2.zero;
            }
        }

        look = Vector2.Lerp(look, targetLook, 15 * Time.deltaTime);

        if (Mathf.Abs(look.x) < 0.01f)
        { 
            look.x = 0; 
        }
        if (Mathf.Abs(look.y) < 0.01f)
        {
            look.y = 0;
        }

        if (TimeManager.instance.pause)
        {
            look = Vector2.zero;
            targetLook = Vector2.zero;
        }

#endif
        }

        public void LookPressed(BaseEventData eventData)
        {
            if (disableInput) return;
            if (isLooking) return;
            isLooking = true;
            PointerEventData pointerEventData = (PointerEventData)eventData;
            cachedTouchPos = pointerEventData.position;
            lookFingerID = -1;
            foreach (var touch in Input.touches)
            {
                if (touch.position == cachedTouchPos)
                    lookFingerID = touch.fingerId;
            }
        }

        public void LookReleased()
        {
            if (disableInput) return;
            foreach (var touch in Input.touches)
            {
                if (touch.fingerId == lookFingerID)
                {
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        isLooking = false;
                        lookFingerID = -1;
                        targetLook = Vector2.zero;
                    }
                    break;
                }
            }
        }
    }
}

