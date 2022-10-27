using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class BulletData
    {
        [SerializeField] private int bulletSpeed;
        [SerializeField] private float changeToRebound;
        [SerializeField] private float reboundRange;

        public int BulletSpeed => bulletSpeed;
        public float ChangeToRebound => changeToRebound;
        public float ReboundRange => reboundRange;

    }

    [CreateAssetMenu (fileName = "ShootData", menuName = "Entities/ShootData")]
    public class ShootData : ScriptableObject
    {
        [SerializeField] private float fireRate;
        [SerializeField] private BulletData bulletData;
        [SerializeField] private AudioClip shootClip;
        
        
        public float FireRate => fireRate;
        public BulletData BulletData => bulletData;
        public AudioClip Clip => shootClip;
    }
}