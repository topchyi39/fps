using Enemies;
using Game;
using Player.Characteristics;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacteristicData characteristics;
        [SerializeField] private CharacterController controller;
        [SerializeField] private EnemyKilledData enemyKilledData;
        
        private readonly UnityEvent _onTeleported = new UnityEvent();
        private readonly UnityEvent _onEnemyKilled = new UnityEvent();
        private readonly UnityEvent _onHit = new UnityEvent();

        private GameState _gameState;
        private PlayerUI _ui;
        
        private GameStatistic _gameStatistic = new();

        public int Health => characteristics.GetCurrentValue(CharacteristicType.HealthPoint);
        public bool CanUltimate => characteristics.GetMaxValue(CharacteristicType.StrengthPoint) <= characteristics.GetCurrentValue(CharacteristicType.StrengthPoint);
        public int Damage => characteristics.GetCurrentValue(CharacteristicType.Damage);

        public IGameStatistic GameStatistic => _gameStatistic;
        
        public event UnityAction OnTeleported
        {
            add => _onTeleported.AddListener(value);
            remove => _onTeleported.RemoveListener(value);
        }
        
        public event UnityAction OnEnemyKilled
        {
            add => _onEnemyKilled.AddListener(value);
            remove => _onEnemyKilled.RemoveListener(value);
        }
        
        public event UnityAction OnHit
        {
            add => _onHit.AddListener(value);
            remove => _onHit.RemoveListener(value);
        }
        
        
        [Inject]
        private void Construct(GameState gameState, PlayerUI playerUI)
        {
            _gameState = gameState;
            _ui = playerUI;
            characteristics.Initialize();
            InitializeUI();
        }

        private void InitializeUI()
        {
            _ui.HealthBar.Initialize(characteristics.GetMaxValue(CharacteristicType.HealthPoint),
                characteristics.GetCurrentValue(CharacteristicType.HealthPoint));
            _ui.StrengthBar.Initialize(characteristics.GetMaxValue(CharacteristicType.StrengthPoint),
                characteristics.GetCurrentValue(CharacteristicType.StrengthPoint));
            characteristics.SubscribeOnModify(CharacteristicType.HealthPoint, _ui.HealthBar.UpdateValue);
            characteristics.SubscribeOnModify(CharacteristicType.StrengthPoint, _ui.StrengthBar.UpdateValue);
        }

        public void Hit(int damage)
        {
            characteristics.AddValue(CharacteristicType.HealthPoint, -damage);
            _onHit?.Invoke();
            CheckForDie();
        }

        public void StealStrength(int value)
        {
            characteristics.AddValue(CharacteristicType.StrengthPoint, -value);
        }

        public void TeleportToPosition(Vector3 teleportPosition)
        {
            controller.enabled = false;
            transform.position = teleportPosition;
            _onTeleported?.Invoke();
            controller.enabled = true;
        }
        
        public void OnEnemyKill(bool withRebound, EnemyType enemyType)
        {
            enemyKilledData.OnEnemyKilled(withRebound, enemyType, characteristics);
            _gameStatistic.OnEnemyKilled();
            _ui.UpdateEnemyKilledCounter(_gameStatistic.EnemyKilled);
            _onEnemyKilled?.Invoke();
        }

        private void CheckForDie()
        {
            if (characteristics.GetCurrentValue(CharacteristicType.HealthPoint) <= 0)
                _gameState.GameOver();

        }
    }
}