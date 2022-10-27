using Player.Characteristics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Zenject;

namespace Enemies
{
    public enum EnemyType
    {
        Boss,
        Red,
    }
    
    public abstract class Enemy : MonoBehaviour
    {
        public class Factory<T>: PlaceholderFactory<T> where T : Enemy{ }

        [SerializeField] protected CharacteristicData characteristicData;
        [SerializeField] protected NavMeshAgent agent;
                
        [Space]
        [SerializeField] private UnityEvent onDie;
        
        protected Player.Player _player;
        
        public abstract EnemyType EnemyType { get; }
        
        public event UnityAction OnDie
        {
            add => onDie.AddListener(value);
            remove => onDie.RemoveListener(value);
        }
        
        private const float PathRadius = 10f;
 
        [Inject]
        public void Construct(Player.Player player)
        {
            _player = player;
            characteristicData.Initialize();
        }

        protected virtual void Start()
        {
        }

        private void Update()
        {
            TickAction();
        }
        
        public void Hit(int damage)
        {
            characteristicData.AddValue(CharacteristicType.HealthPoint, -damage);
            
            if (characteristicData.GetCurrentValue(CharacteristicType.HealthPoint) == 0)
                Die();
        }
        
        public void Die()
        {            
            onDie?.Invoke();
            Destroy(gameObject);
        }

        protected abstract bool TickAction();
        
        protected bool TryToSetNewRandomPath()
        {
            if (!agent.enabled) return false;
            
            if (Vector3.Distance(agent.transform.position, agent.destination) > 1f) return false;
            
            RandomNavmeshPath();
            return true;
        }
        
        [ContextMenu("new path")]
        protected void RandomNavmeshPath() 
        {
            var randomDirection = Random.insideUnitSphere * PathRadius;
            randomDirection += transform.position;
            
            var finalPosition = Vector3.zero;
            
            if (NavMesh.SamplePosition(randomDirection, out var hit, PathRadius, agent.areaMask)) {
                finalPosition = hit.position;            
            }

            var currentPath = new NavMeshPath();
            agent.CalculatePath(finalPosition, currentPath);
            agent.SetPath(currentPath);
        }
    }
}