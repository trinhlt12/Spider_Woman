using UnityEngine;

namespace SFRemastered
{
    public interface IHittable
    {
        void TakeHit(float damage, Vector3 hitPoint, Vector3 impactDirection);
    }
}