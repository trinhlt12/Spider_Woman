using SFRemastered;

public abstract class ActionStateBase : StateBase
{
    protected ActionFSM _actionFSM;

    public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
    {
        base.InitState(fsm, blackBoard, isAIControlled);
        _actionFSM = fsm as ActionFSM;
    }
    
}