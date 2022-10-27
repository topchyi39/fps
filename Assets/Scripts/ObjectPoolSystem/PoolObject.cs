using System;
using UnityEngine;
using Zenject;

namespace ObjectPoolSystem
{
    public abstract class PoolObject : MonoBehaviour
    {
        private Transform _defaultParent;
        private bool _activated = true;
        
        public event Action<PoolObject> OnEnabled;
        public event Action<PoolObject> OnDisable;

        private void Awake()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            _defaultParent = transform.parent;
            _activated = false;
            enabled = false;
            gameObject.SetActive(false);
        }

        public virtual void Enable(Transform parent)
        {
            if(_activated) return;
            
            _activated = true;
            enabled = true;
            gameObject.SetActive(true);
            transform.SetParent(parent);
            ResetTransform();
            OnEnabled?.Invoke(this);
        }

        public virtual void Disable()
        {
            if(!_activated) return;
            
            _activated = false;
            enabled = false;
            gameObject.SetActive(false);
            transform.SetParent(_defaultParent);
            ResetTransform();
            OnDisable?.Invoke(this);
        }

        private void ResetTransform()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}