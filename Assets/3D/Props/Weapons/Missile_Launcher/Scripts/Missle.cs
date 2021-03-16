using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;
using GameDevHQITP.ScriptableObjects;

namespace GameDevHQ.FileBase.Missle_Launcher.Missle
{
    [RequireComponent(typeof(Rigidbody))] //require rigidbody
    [RequireComponent(typeof(AudioSource))] //require audiosource
    public class Missle : MonoBehaviour
    {

        [SerializeField]
        private ParticleSystem _particle; //reference to the particle system

        [SerializeField] //fuse delay of the rocket
        private float _fuseDelay;

        private AudioSource _audioSource; //reference to the audiosource of the rocket
        
        private float _gravityOffset = 0;

        private GameObject _target;
        [SerializeField] GameObject _explosionPrefab;
        [SerializeField] ParticleSystem _exposionParticles;
        [SerializeField] GameObject _missileGO;
        [SerializeField] MissileConfig _missileConfig;

        // Use this for initialization
        IEnumerator Start()
        {
            _audioSource = GetComponent<AudioSource>(); //assign the audiosource component
            _audioSource.pitch = Random.Range(0.7f, 1.9f); //randomize the pitch of the rocket audio
            _particle.Play(); //play the particles of the rocket
            _audioSource.Play(); //play the rocket sound

            yield return new WaitForSeconds(_fuseDelay); //wait for the fuse delay

            _exposionParticles.Stop();
        }


        // Update is called once per frame
        void FixedUpdate()
        {

            if (!_missileGO.activeSelf) { return; }

            TrackTarget();
        }

        /// <summary>
        /// This method is used to assign traits to our missle assigned from the launcher.
        /// </summary>
        public void AssignMissleRules(float launchSpeed, float power, float fuseDelay, float destroyTimer, GameObject target)
        {
            _target = target;
            Destroy(this.gameObject, destroyTimer); //destroy the rocket after destroyTimer 
        }

        private void TrackTarget()
        {

            Vector3 newTargetPos = _target == null ? Vector3.forward : _target.transform.position;
            Vector3 dir = newTargetPos - transform.position;
            dir.y -= _gravityOffset;
            _gravityOffset += _missileConfig.gravityOffset;
            transform.Translate(Vector3.forward * Time.deltaTime * _missileConfig.missileSpeed);

            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _missileConfig.turningDamping);

            //Debug.DrawLine(transform.position, newTargetPos, Color.cyan, 0.2f);
            Debug.DrawRay(transform.position, Vector3.one * 0.1f, Color.magenta, 1f);
            //Debug.DrawRay(transform.position, dir, Color.yellow, 0.2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
            {
                _missileGO.SetActive(false);
                //Debug.DrawLine(gameObject.transform.position, other.gameObject.transform.position, Color.cyan, 5f);
                GameObject go = Instantiate(_explosionPrefab, this.transform);
                Vector3 newHit = transform.position + Vector3.forward * 0.5f;
                go.transform.position = newHit;

                Destroy(this.gameObject, 5f);
                if (other.CompareTag("Enemy"))
                {
                    TowerBattleReady.OnTakeDamage(other.gameObject, 50);
                }
            }
        }
    }
}

