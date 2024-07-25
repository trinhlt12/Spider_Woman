using System.Collections.Generic;
using UnityEngine;

namespace SFRemastered
{
    [CreateAssetMenu(fileName = "NewComboConfig", menuName = "Combat/Combo Config")]
    public class ComboConfig : ScriptableObject
    {
        public AttackData[] Attacks;
        public float MaxComboTime = 4f;
        public bool AllowComboReset = true;
        public AnimationCurve ComboDamageMultiplier = AnimationCurve.Linear(0, 1, 1, 2);
    }
}