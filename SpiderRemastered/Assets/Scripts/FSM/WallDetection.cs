using UnityEngine;

namespace SFRemastered
{
    public class WallDetection : MonoBehaviour
    {
        public BlackBoard _blackBoard;
        public Vector3 detectedWallNormal { get; private set; }

        public bool IsWallDetected()
        {
            RaycastHit hit;
            _blackBoard.rayOrigin = _blackBoard.transform.position + Vector3.up * _blackBoard.raycastHeight;
            Vector3 rayDirection = _blackBoard.transform.forward; // Use the forward direction of the player

            if (Physics.Raycast(_blackBoard.rayOrigin, rayDirection, out hit, _blackBoard.wallDetectionRange, _blackBoard.wallLayerMask))
            {
                detectedWallNormal = hit.normal;
                Debug.DrawRay(_blackBoard.rayOrigin, rayDirection * _blackBoard.wallDetectionRange, Color.green);
                Debug.Log("Wall detected at: " + hit.point);
                return true;
            }

            detectedWallNormal = Vector3.zero;
            Debug.DrawRay(_blackBoard.rayOrigin, rayDirection * _blackBoard.wallDetectionRange, Color.red);
            Debug.Log("No wall detected");
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_blackBoard != null)
            {
                _blackBoard.rayOrigin = _blackBoard.transform.position + Vector3.up * _blackBoard.raycastHeight;
                Vector3 rayDirection = _blackBoard.transform.forward; 
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(_blackBoard.rayOrigin, rayDirection * _blackBoard.wallDetectionRange);
            }
        }
    }
}