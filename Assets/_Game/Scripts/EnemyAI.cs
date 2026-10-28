using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    public static bool stopAllEnemies = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (stopAllEnemies)
        {
            agent.isStopped = true;
            return;
        }

        agent.SetDestination(player.position);
    }
}