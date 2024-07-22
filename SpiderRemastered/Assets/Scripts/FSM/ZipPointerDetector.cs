using UnityEngine;
using UnityEngine.UI;

namespace SFRemastered
{
    public class ZipPointerDetector : MonoBehaviour
    {
        public BlackBoard BlackBoard;

        private void Update()
        {
            DetectZipPoint();
            DrawDebugLines();
        }

        private void DetectZipPoint()
        {
            Debug.Log("Detect Zip point method called");
            RaycastHit initialHit;
            if (Physics.Raycast(BlackBoard.playerCamera.position, BlackBoard.playerCamera.forward, out initialHit,
                    BlackBoard.maxZipDistance, BlackBoard.zipPointLayer))
            {
                Vector3 hitPoint = initialHit.point;
                Vector3 hitNormal = initialHit.normal;

                // Perform secondary line traces
                RaycastHit aboveHit, forwardHit;
                bool aboveHitValid = Physics.Raycast(hitPoint + Vector3.up * 1f, Vector3.down, out aboveHit, 2f,
                    BlackBoard.zipPointLayer);
                bool forwardHitValid = Physics.Raycast(hitPoint + hitNormal * 0.1f, hitNormal, out forwardHit, 2f,
                    BlackBoard.zipPointLayer);
                if (aboveHitValid && forwardHitValid)
                {
                    Vector3 aboveNormal = aboveHit.normal;
                    Vector3 forwardNormal = forwardHit.normal;

                    if (Vector3.Angle(hitNormal, aboveNormal) > 45f || Vector3.Angle(hitNormal, forwardNormal) > 45f)
                    {
                        Vector3 zipPoint = forwardHit.point;

                        if (CanPlayerFit(zipPoint) && IsPointVisible(zipPoint) && IsClosestPoint(zipPoint))
                        {
                            ShowZipHUD(zipPoint);
                            Debug.Log("Show hud ui");
                        }
                    }
                }
            }
            else
            {
                HideZipHUD();
            }
        }

        private void ShowZipHUD(Vector3 zipPoint)
        {
            Vector3 screenPoint = BlackBoard.playerCamera.GetComponent<Camera>().WorldToScreenPoint(zipPoint);
            Debug.Log("Screen point for zip point: " + screenPoint);
            BlackBoard.hudWidget.position = screenPoint;  // Update the position of the HUD widget
            BlackBoard.hudWidget.gameObject.SetActive(true);  // Make sure the HUD widget is active
            Debug.Log("HUD widget position set to: " + BlackBoard.hudWidget.position);
        }

        private void HideZipHUD()
        {
            BlackBoard.hudWidget.gameObject.SetActive(false);
        }

        private bool IsClosestPoint(Vector3 zipPoint)
        {
            float distanceToPlayer = Vector3.Distance(BlackBoard.playerCamera.position, zipPoint);
            RaycastHit[] hits = Physics.SphereCastAll(BlackBoard.playerCamera.position, 1f,
                BlackBoard.playerCamera.forward, BlackBoard.maxZipDistance, BlackBoard.zipPointLayer);

            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Distance(BlackBoard.playerCamera.position, hit.point) < distanceToPlayer)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPointVisible(Vector3 zipPoint)
        {
            Vector3 screenPoint = BlackBoard.playerCamera.GetComponent<Camera>().WorldToScreenPoint(zipPoint);
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 &&
                   screenPoint.y < Screen.height;
        }

        private bool CanPlayerFit(Vector3 zipPoint)
        {
            Vector3 capsuleBottom = zipPoint + Vector3.up * BlackBoard.playerCollider.height / 2f;
            Vector3 capsuleTop = zipPoint + Vector3.up * BlackBoard.playerCollider.height;

            return !Physics.CheckCapsule(capsuleBottom, capsuleTop, BlackBoard.playerCollider.radius,
                BlackBoard.zipPointLayer);
        }

        private void DrawDebugLines()
        {
            if (BlackBoard == null || BlackBoard.playerCamera == null)
                return;

            Debug.DrawRay(BlackBoard.playerCamera.position, BlackBoard.playerCamera.forward * BlackBoard.maxZipDistance, Color.red);

            RaycastHit initialHit;
            if (Physics.Raycast(BlackBoard.playerCamera.position, BlackBoard.playerCamera.forward, out initialHit, BlackBoard.maxZipDistance, BlackBoard.zipPointLayer))
            {
                Vector3 hitPoint = initialHit.point;
                Vector3 hitNormal = initialHit.normal;

                Debug.DrawRay(hitPoint, Vector3.up * 1f, Color.green);

                RaycastHit aboveHit;
                if (Physics.Raycast(hitPoint + Vector3.up * 1f, Vector3.down, out aboveHit, 2f, BlackBoard.zipPointLayer))
                {
                    Debug.DrawRay(aboveHit.point, aboveHit.normal * 1f, Color.blue);
                }

                Debug.DrawRay(hitPoint + hitNormal * 0.1f, hitNormal * 2f, Color.yellow);

                RaycastHit forwardHit;
                if (Physics.Raycast(hitPoint + hitNormal * 0.1f, hitNormal, out forwardHit, 2f, BlackBoard.zipPointLayer))
                {
                    Debug.DrawRay(forwardHit.point, forwardHit.normal * 1f, Color.cyan);
                }
            }
        }
    }
}
