using System;
using System.Collections;
using Player.Characteristics;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class JumpAndHitActionParams
    {
        [SerializeField] private float actionRange;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpTime;
        [SerializeField] private float delayForHit;
        [SerializeField] private float hitSpeed;

        public float ActionRange => actionRange;
        public float JumpHeight => jumpHeight;
        public float JumpTime => jumpTime;
        public float DelayForHit => delayForHit;
        public float HitSpeed => hitSpeed;
    }

    public class RedEnemy : Enemy
    {
        [SerializeField] private JumpAndHitActionParams actionParams;

        private bool _inProgress;
        private bool _canHitAndDie;

        public override EnemyType EnemyType => EnemyType.Red;

        protected override void Start()
        {
            base.Start();
            TickAction();
        }

        protected override bool TickAction()
        {
            if (_inProgress) return true;
            
            agent.enabled = false;
            _inProgress = true;
            StartCoroutine(InvokeRoutine());

            return true;
        }

        private IEnumerator InvokeRoutine()
        {
            yield return new WaitForSeconds(actionParams.DelayForHit);
            var jumpPosition = transform.position + new Vector3(0, actionParams.JumpHeight, 0);
            yield return MoveTo(transform.transform, jumpPosition, actionParams.JumpTime);
            yield return new WaitForSeconds(actionParams.DelayForHit);
            _canHitAndDie = true;
            yield return MoveTo(transform.transform, _player.transform, Vector3.up * 1f, actionParams.HitSpeed);
        }

        private IEnumerator MoveTo(Transform target, Vector3 position, float duration)
        {
            var startPosition = target.position;

            var time = 0f;
            while (time <= duration)
            {
                target.position = Vector3.Lerp(startPosition, position, time / duration);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator MoveTo(Transform target, Transform to, Vector3 offset, float speed)
        {
            while (this)
            {
                target.position = target.position + (to.position + offset - target.position).normalized * (speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent<Player.Player>(out var player)) return;
            if(!_canHitAndDie) return;

            _player.Hit(characteristicData.GetCurrentValue(CharacteristicType.Damage));
            Die();
        }
    }
}