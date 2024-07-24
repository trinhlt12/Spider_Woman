using UnityEngine;

namespace SFRemastered
{
    public class WallDetection : MonoBehaviour
    {
        public BlackBoard _blackBoard;
        public Vector3 detectedWallNormal { get; private set; }
        public bool debugMode = true;

        public bool IsWallDetected()
        {
            RaycastHit hit;
            _blackBoard.rayOrigin = _blackBoard.transform.position + Vector3.up * _blackBoard.raycastHeight;
            
            // Check multiple directions for wall detection
            Vector3[] directions = {
                _blackBoard.transform.right,
                -_blackBoard.transform.right,
                _blackBoard.transform.forward,
                -_blackBoard.transform.forward
            };

            foreach (Vector3 direction in directions)
            {
                if (Physics.Raycast(_blackBoard.rayOrigin, direction, out hit, _blackBoard.wallDetectionRange, _blackBoard.wallLayerMask))
                {
                    detectedWallNormal = hit.normal;
                    if (debugMode)
                    {
                        Debug.DrawRay(_blackBoard.rayOrigin, direction * hit.distance, Color.green);
                        Debug.DrawRay(hit.point, hit.normal, Color.blue);
                        Debug.Log($"Wall detected. Normal: {detectedWallNormal}, Direction: {direction}");
                    }
                    return true;
                }
            }

            detectedWallNormal = Vector3.zero;
            if (debugMode)
            {
                foreach (Vector3 direction in directions)
                {
                    Debug.DrawRay(_blackBoard.rayOrigin, direction * _blackBoard.wallDetectionRange, Color.red);
                }
                Debug.Log("No wall detected.");
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_blackBoard != null)
            {
                _blackBoard.rayOrigin = _blackBoard.transform.position + Vector3.up * _blackBoard.raycastHeight;
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_blackBoard.rayOrigin, 0.1f);
                
                Vector3[] directions = {
                    _blackBoard.transform.right,
                    -_blackBoard.transform.right,
                    _blackBoard.transform.forward,
                    -_blackBoard.transform.forward
                };

                foreach (Vector3 direction in directions)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(_blackBoard.rayOrigin, direction * _blackBoard.wallDetectionRange);
                }
            }
        }
    }
}