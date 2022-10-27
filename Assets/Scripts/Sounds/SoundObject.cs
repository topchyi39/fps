using System;
using ObjectPoolSystem;
using UnityEngine;

namespace Sounds
{
    public class SoundObject : PoolObject
    {
        [SerializeField] private AudioSource source;

        public bool IsPlaying => source.isPlaying;
        
        public void Play(AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }

        private void Update()
        {
            if(enabled && !source.isPlaying)
                Disable();
        }
    }
}