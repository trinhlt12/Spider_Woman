using UnityEngine;
using Animancer;
using System.Collections.Generic;
using System.Linq;

namespace SFRemastered
{
    public class ComboSystem
    {
        private ComboConfig _config;
        private int _currentComboIndex = -1;
        private float _comboStartTime;
        private bool _isComboActive = false;
        private float _lastAttackTime;
        private AnimancerState _currentAnimancerState;

        public ComboSystem(ComboConfig config)
        {
            _config = config;
        }

        public void StartCombo()
        {
            _currentComboIndex = 0;
            _comboStartTime = Time.time;
            _isComboActive = true;
            _lastAttackTime = Time.time;
        }

        public bool CanContinueCombo()
        {
            if (!_isComboActive) return false;
            
            float elapsedTime = Time.time - _comboStartTime;
            if (elapsedTime > _config.MaxComboTime)
            {
                EndCombo();
                return false;
            }

            if (_currentComboIndex >= _config.Attacks.Length - 1)
            {
                return false;
            }

            if (_currentAnimancerState == null)
            {
                return false;
            }

            AttackData currentAttack = _config.Attacks[_currentComboIndex];
            float normalizedTime = _currentAnimancerState.NormalizedTime;
            
            return normalizedTime >= currentAttack.ComboWindowStart &&
                   normalizedTime <= currentAttack.ComboWindowEnd;
        }

        public void AdvanceCombo()
        {
            if (CanContinueCombo())
            {
                _currentComboIndex++;
                _lastAttackTime = Time.time;
            }
        }

        public AttackData GetCurrentAttack()
        {
            return _isComboActive ? _config.Attacks[_currentComboIndex] : null;
        }

        public float GetComboDamageMultiplier()
        {
            return _config.ComboDamageMultiplier.Evaluate((float)_currentComboIndex / (_config.Attacks.Length - 1));
        }

        public void EndCombo()
        {
            _isComboActive = false;
            _currentComboIndex = -1;
            _currentAnimancerState = null;
        }

        public bool IsComboActive()
        {
            return _isComboActive;
        }

        public int GetCurrentComboCount()
        {
            return _currentComboIndex + 1;
        }

        public void SetCurrentAnimancerState(AnimancerState state)
        {
            _currentAnimancerState = state;
        }

        public bool TryResetCombo()
        {
            if (_config.AllowComboReset)
            {
                EndCombo();
                return true;
            }
            return false;
        }
    }
}