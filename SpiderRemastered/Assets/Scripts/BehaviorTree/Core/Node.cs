using SFRemastered.BehaviorTree.ScriptableObjects;
using UnityEngine;

namespace SFRemastered.BehaviorTree.Core
{
    public abstract class Node : ScriptableObject
    {
        protected BlackBoardSO blackBoard;
        
        public virtual void Initialize(BlackBoardSO blackBoard)
        {
            this.blackBoard = blackBoard;
            OnInitialize();
        }
        
        protected virtual void OnInitialize(){ }

        public abstract NodeState Evaluate();
    }
}

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}