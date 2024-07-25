using SFRemastered;
using SFRemastered.InputSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActionStates/ActionIdleState")]
public class ActionIdleState : ActionStateBase
{
    [SerializeField] private ComboAttackState _comboAttackState;

    public override StateStatus UpdateState()
    {
        StateStatus baseStatus = base.UpdateState();
        if (baseStatus != StateStatus.Running)
        {
            return baseStatus;
        }

        if (InputManager.instance.attack.Down)
        {
            _actionFSM.ChangeState(_comboAttackState);
            return StateStatus.Success;
        }

        return StateStatus.Running;
    }
}