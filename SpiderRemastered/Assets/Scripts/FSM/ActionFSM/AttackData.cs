using System.Collections.Generic;
using Animancer;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat/Attack Data")]
    public class AttackData : ScriptableObject
    {
        public string AttackName;
        public List<ClipTransition> AnimationClip;
        public float Damage = 10f;
        [Range(0, 1)] public float ComboWindowStart = 0.6f;
        [Range(0, 1)] public float ComboWindowEnd = 0.9f;
        public float AttackRange = 1.5f;
        public Vector3 HitboxSize = new Vector3(1, 1, 1);
        public Vector3 HitboxOffset = Vector3.zero;
        public float KnockbackForce = 5f;
        public AnimationCurve DamageCurve = AnimationCurve.Linear(0, 1, 1, 1);
        
        //
    }
}