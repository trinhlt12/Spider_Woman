using NodeCanvas.Framework;
using SFRemastered.BehaviorTree.BaseActionTask;
using SFRemastered.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace SFRemastered
{
    public class PatrolAction : BaseActionTask<NavMeshAgent>
    {
        public BBParameter<EnemyConfigSO> enemyConfig;
        private EnemyController _enemyController;
        private NavMeshAgent _agent;
        private Vector3 _destination;
        private float stuckTime; // Track how long the agent has been stuck
        private float stuckThreshold = 3.0f; // Time in seconds before considering the agent stuck
        private Vector3 lastPosition; // Track the agent's last position to detect stuck situations

        protected override void OnExecute()
        {
            base.OnExecute();
            _enemyController = agent.GetComponent<EnemyController>();
            _agent = agent;
            stuckTime = 0;
            lastPosition = _agent.transform.position;

            if (_enemyController == null || _agent == null)
            {
                Debug.LogError("NavMeshAgent or EnemyController is null. Ensure the object has the necessary components.");
                EndAction(false);
                return;
            }

            SetNextDestination();
        }

        private void SetNextDestination()
        {
            float patrolRadius = enemyConfig.value.patrolRadius;
            _destination = GetRandomPoint(agent.transform.position, patrolRadius);
            _agent.SetDestination(_destination);
        }

        private Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            Vector3 randomPos = Random.insideUnitSphere * radius;
            randomPos += center;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, radius, 1);
            return hit.position;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // Check if the agent is moving or stuck
            if (!_agent.pathPending)
            {
                // Check if the agent is stuck or moving too slowly
                if (Vector3.Distance(_agent.transform.position, lastPosition) < 0.1f)
                {
                    stuckTime += Time.deltaTime;

                    if (stuckTime >= stuckThreshold)
                    {
                        Debug.LogWarning("Agent seems to be stuck. Recalculating destination.");
                        SetNextDestination();
                        stuckTime = 0; // Reset stuck time
                    }
                }
                else
                {
                    stuckTime = 0; // Reset if the agent is moving
                }

                lastPosition = _agent.transform.position;

                // Check if the agent has reached the destination
                if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.hasPath)
                {
                    Debug.Log("Agent has reached the destination.");
                    EndAction(true); // Patrol action ends successfully
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_agent != null)
            {
                _agent.ResetPath();
            }
        }
    }
}
