namespace SFRemastered
{
    public abstract class ActionStateBase : StateBase
    {
        protected global::SFRemastered.ActionFSM _actionFSM;

        public override void InitState(FSM fsm, BlackBoard blackBoard, bool isAIControlled)
        {
            base.InitState(fsm, blackBoard, isAIControlled);
            _actionFSM = fsm as global::SFRemastered.ActionFSM;
        }
    
    }
}