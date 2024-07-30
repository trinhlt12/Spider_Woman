using System;
using SFRemastered.BehaviorTree.ScriptableObjects;
using UnityEngine;

namespace SFRemastered.BehaviorTree.Core
{
    public class BehaviorTree : MonoBehaviour
    {
        public BehaviorTreeSO tree;
        public BlackBoardSO blackBoard;

        private Node rootNode;

        private void Start()
        {
            rootNode = tree.rootNode.CreateNode();
            rootNode.Initialize(blackBoard);
        }

        private void Update()
        {
            if (rootNode != null)
            {
                rootNode.Evaluate();
            }
        }
    }
}