using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class soundParticle : MonoBehaviour {
    public AudioClip spawnSound;
    
    private ParticleSystem _particleSystem;
    private int _currentCount = 0;

    private void Start() {
        _particleSystem = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

        if(_particleSystem.particleCount > _currentCount) {
            AudioSource.PlayClipAtPoint(spawnSound, new Vector3(5, 1, 2));
        }

        _currentCount = _particleSystem.particleCount;
    }
}
