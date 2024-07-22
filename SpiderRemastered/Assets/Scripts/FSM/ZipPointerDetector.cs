using UnityEngine;
using UnityEngine.UI;

namespace SFRemastered
{
    public class ZipPointerDetector : MonoBehaviour
    {
        public BlackBoard BlackBoard;

        private Vector3 zipPoint = Vector3.zero;
        private bool zipPointDetected = false;

        private void Update()
        {
            DetectZipPoint();
            UpdateCrosshairPosition();
            DrawDebugLines();
        }

        private void DetectZipPoint()
        {
            if (Physics.Raycast(BlackBoard.playerCamera.position, BlackBoard.playerCamera.forward, out RaycastHit initialHit,
                    BlackBoard.maxZipDistance, BlackBoard.zipPointLayer))
            {
                Vector3 hitPoint = initialHit.point;
                Vector3 hitNormal = initialHit.normal;

                // Perform secondary line traces to detect edges
                Vector3[] directions = {
                    Vector3.up,
                    Vector3.down,
                    Vector3.left,
                    Vector3.right,
                    Vector3.forward,
                    Vector3.back
                };

                foreach (Vector3 direction in directions)
                {
                    if (Physics.Raycast(hitPoint + direction * 0.1f, -direction, out RaycastHit edgeHit, 0.2f, BlackBoard.zipPointLayer))
                    {
                        if (Vector3.Angle(hitNormal, edgeHit.normal) > 45f) // Arbitrary angle threshold for edge detection
                        {
                            zipPoint = edgeHit.point;
                            zipPointDetected = true;
                            Debug.Log("Edge detected at: " + zipPoint);

                            // Project the player's collision capsule to ensure it fits on the ledge
                            if (CanPlayerFit(zipPoint))
                            {
                                Debug.Log("Player can fit on the ledge.");
                            }
                            else
                            {
                                zipPointDetected = false;
                                Debug.Log("Player cannot fit on the ledge.");
                            }
                            break;
                        }
                    }
                }

                if (!zipPointDetected)
                {
                    Debug.Log("No edge detected.");
                }
            }
            else
            {
                zipPointDetected = false;
                Debug.Log("Initial hit not detected.");
            }
        }

        private void UpdateCrosshairPosition()
        {
            if (zipPointDetected)
            {
                Camera cam = BlackBoard.playerCamera.GetComponent<Camera>();
                Vector3 screenPoint = cam.WorldToScreenPoint(zipPoint);

                RectTransform crosshairRect = BlackBoard.crosshair.GetComponent<RectTransform>();
                crosshairRect.position = screenPoint;
                BlackBoard.crosshair.gameObject.SetActive(true);
            }
            else
            {
                BlackBoard.crosshair.gameObject.SetActive(false);
            }
        }

        private bool CanPlayerFit(Vector3 zipPoint)
        {
            Vector3 capsuleBottom = zipPoint + Vector3.up * BlackBoard.playerCollider.height / 2f;
            Vector3 capsuleTop = zipPoint + Vector3.up * BlackBoard.playerCollider.height;

            return !Physics.CheckCapsule(capsuleBottom, capsuleTop, BlackBoard.playerCollider.radius, BlackBoard.zipPointLayer);
        }

        private void DrawDebugLines()
        {
            if (BlackBoard == null || BlackBoard.playerCamera == null)
                return;

            Debug.DrawRay(BlackBoard.playerCamera.position, BlackBoard.playerCamera.forward * BlackBoard.maxZipDistance, Color.red);
        }

        private void OnDrawGizmos()
        {
            if (zipPointDetected)
            {
                Gizmos.color = Color.red;
                float sphereRadius = 0.5f; // Adjust this value to make the sphere bigger
                Gizmos.DrawSphere(zipPoint, sphereRadius);

                // Draw the player's collision capsule for fitting check
                Vector3 capsuleBottom = zipPoint + Vector3.up * BlackBoard.playerCollider.height / 2f;
                Vector3 capsuleTop = zipPoint + Vector3.up * BlackBoard.playerCollider.height;

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(capsuleBottom, BlackBoard.playerCollider.radius);
                Gizmos.DrawWireSphere(capsuleTop, BlackBoard.playerCollider.radius);
                Gizmos.DrawLine(capsuleBottom, capsuleTop);
            }
        }
    }
}
