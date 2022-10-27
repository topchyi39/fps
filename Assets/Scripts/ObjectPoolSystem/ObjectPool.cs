using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectPoolSystem
{
    public interface IObjectPool<T> where T : Object
    {
        TObjectType Instantiate<TObjectType>() where TObjectType : T;
        TObjectType Instantiate<TObjectType>( Transform parent) where TObjectType : T;
        TObjectType Instantiate<TObjectType>(Vector3 position, Quaternion rotation, Transform parent) where TObjectType : T;
        void Destroy(T destroyObject);
    }
    
    public class ObjectPool : MonoBehaviour, IObjectPool<PoolObject>
    {
        [Serializable]
        private class ObjectContainer
        {
            [SerializeField] private PoolObject poolObject;
            [SerializeField] private int countOnStart;

            public PoolObject Object => poolObject;
            public int Count => countOnStart;
        }
        
        [SerializeField] private ObjectContainer[] objects;

        private readonly Dictionary<Type, PoolObject> _prefabs = new Dictionary<Type, PoolObject>();
        private readonly Dictionary<Type, List<PoolObject>> _availableObject = new Dictionary<Type, List<PoolObject>>();
        private readonly Dictionary<Type, List<PoolObject>> _activatedObjects = new Dictionary<Type, List<PoolObject>>();
        
        private void Awake()
        {
            foreach (var objectContainer in objects)
            {
                var type = objectContainer.Object.GetType();

                if (!_prefabs.ContainsKey(type))
                {
                    _prefabs.Add(type, objectContainer.Object);
                    PrepareObject(type, objectContainer.Count);
                }
            }
        }

        private void PrepareObject(Type type, int count)
        {
            var prefab = _prefabs[type];
            
            var parent = new GameObject(type.Name);
            parent.transform.SetParent(transform);

            if (_availableObject.ContainsKey(type))
            {
                _availableObject[type] = new List<PoolObject>();
            }
            else
            {
                _availableObject.Add(type, new List<PoolObject>());
            }

            var list = _availableObject[type];
            for (var i = 0; i < count; i++)
            {
                var poolObject = Instantiate(prefab, parent.transform);
                poolObject.OnDisable += Destroy;
                list.Add(poolObject);
            }
        }
        
        public TObjectType Instantiate<TObjectType>() where TObjectType : PoolObject
        {
            var type = typeof(TObjectType);
            if (!_availableObject.ContainsKey(type)) return null;

            var list = _availableObject[type];
            if(list.Count == 0) PrepareObject(type, 1);
            var poolObject = _availableObject[type][0];
            
            if (!_activatedObjects.ContainsKey(type)) _activatedObjects.Add(type, new List<PoolObject>());

            _availableObject[type].Remove(poolObject);
            _activatedObjects[type].Add(poolObject);
            
            poolObject.Enable(null);

            return (TObjectType) poolObject;
        }

        public TObjectType Instantiate<TObjectType>(Transform parent) where TObjectType : PoolObject
        {
            var poolObject = Instantiate<TObjectType>();
            poolObject.transform.SetParent(parent);

            return poolObject;
        }

        public TObjectType Instantiate<TObjectType>(Vector3 position, Quaternion rotation, Transform parent) where TObjectType : PoolObject
        {
            var poolObject = Instantiate<TObjectType>();
            poolObject.transform.SetParent(parent);
            poolObject.transform.localPosition = position;
            poolObject.transform.localRotation = rotation;

            return poolObject;
        }

        public void Destroy(PoolObject destroyObject)
        {
            var type = destroyObject.GetType();
            if(!_activatedObjects.TryGetValue(type, out var list)) return;
            
            if(!list.Contains(destroyObject)) return;

            list.Remove(destroyObject);

            if (!_availableObject.TryGetValue(type, out var availableList))
            {
                availableList = new List<PoolObject>();
                _activatedObjects.Add(type, availableList);
            }
            
            availableList.Add(destroyObject);
            destroyObject.Disable();
        }
    }
}