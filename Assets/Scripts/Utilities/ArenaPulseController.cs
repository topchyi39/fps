using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Utilities
{
    public class ArenaPulseController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float pulseTime;
        [SerializeField] private float maxEmission;
        [SerializeField] private Material material;
        [SerializeField] private Color onEnemyKillColor;
        [SerializeField] private Color onHitColor;

        private Player.Player _player;
        private Coroutine _pulseRoutine;

        private static readonly string GridColor = "_GridColor";
        private static readonly string EmissiveStrength = "_EmissiveStrengh";

        [Inject]
        private void Construct(Player.Player player)
        {
            _player = player;
            _player.OnEnemyKilled += PlayerOnEnemyKilled;
            _player.OnHit += PlayerOnHit;
        }

        private void Start()
        {
            material.SetFloat(EmissiveStrength, 0);
        }

        private void PlayerOnEnemyKilled()
        {
            material.SetVector(GridColor, onEnemyKillColor);
            Pulse();
        }
        
        private void PlayerOnHit()
        {
            material.SetVector(GridColor, onHitColor);
            Pulse();
        }

        private void Pulse()
        {
            if (_pulseRoutine != null) StopCoroutine(_pulseRoutine);

            _pulseRoutine = StartCoroutine(PulseRoutine());
        }

        private IEnumerator PulseRoutine()
        {
            var time = 0f;

            while (time <= pulseTime)
            {
                var value = curve.Evaluate(time / pulseTime) * maxEmission;
                
                Debug.Log(value);
                material.SetFloat(EmissiveStrength, value);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            material.SetFloat(EmissiveStrength, 0);
            _pulseRoutine = null;
        }
    }
}