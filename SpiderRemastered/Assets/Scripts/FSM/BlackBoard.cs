using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    public class BlackBoard : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        public CameraController cameraController;
        public AnimancerComponent animancer;
        public SFXManager sfxManager;
        public new Camera camera;
        public new Rigidbody rigidbody;
        public Vector3 moveDirection;
        public bool jump;
        public bool sprint;
        public bool isGrounded;
        public LayerMask groundLayers;
        public Transform shootPosition;
        public WebAttachPoint webAttachPoint;
        public bool _webAttached;
        public float _downForceMagnitude = 5f;

        [Header("Wall-Running Mechanic")] 
        public float wallDetectionRange = 1f;
        public LayerMask wallLayerMask;
        public float raycastHeight = 2f;
        public float wallRunSpeed = 50f;
        public bool isWallRunning;
        public Vector3 rayOrigin;
        public float stickToWallDistance = 2f;
        public Vector3 wallRunDirection;
        public Vector3 detectedWallNormal;
    }
}
