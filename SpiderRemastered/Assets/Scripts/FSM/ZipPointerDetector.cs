using UnityEngine;
using UnityEngine.UI;

namespace SFRemastered
{
    public class ZipPointerDetector : MonoBehaviour
    {
        public BlackBoard BlackBoard;

        private Vector3 zipPoint = Vector3.zero;

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
                        if (Vector3.Angle(hitNormal, edgeHit.normal) > 45f)
                        {
                            zipPoint = edgeHit.point;
                            BlackBoard.zipPoint = zipPoint;
                            BlackBoard.zipPointDetected = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                BlackBoard.zipPointDetected = false;
            }
        }

        private void UpdateCrosshairPosition()
        {
            if (BlackBoard.zipPointDetected)
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
            if (BlackBoard.zipPointDetected)
            {
                Gizmos.color = Color.red;
                float sphereRadius = 0.5f;
                Gizmos.DrawSphere(zipPoint, sphereRadius);

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
