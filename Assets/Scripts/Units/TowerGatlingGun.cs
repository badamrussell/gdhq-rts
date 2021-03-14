using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQITP.Units;

namespace GameDevHQITP.Units
{
    /// <summary>
    /// This script will allow you to view the presentation of the Turret and use it within your project.
    /// Please feel free to extend this script however you'd like. To access this script from another script
    /// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
    /// "using GameDevHQ.FileBase.Gatling_Gun" without the quotes. 
    /// 
    /// For more, visit GameDevHQ.com
    /// 
    /// @authors
    /// Al Heck
    /// Jonathan Weinberger
    /// </summary>

    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class TowerGatlingGun : TowerBattleReady
    {
        [SerializeField] private Transform[] _gunBarrels; //Reference to hold the gun barrel
        [SerializeField] private GameObject[] _muzzleFlashes; //reference to the muzzle flash effect to play when firing
        [SerializeField] private ParticleSystem[] _bulletCasings; //reference to the bullet casing effect to play when firing

        public AudioClip fireSound; //Reference to the audio clip

        [SerializeField] private GameObject[] _tracerGO;
        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;
        //private bool _isAttacking = false;
        private GameObject _target;

        [SerializeField] private float _tracerScaleX = 0.1f;

        // Use this for initialization
        new void Start()
        {
            //_gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>(); //assigning the transform of the gun barrel to the variable

            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play

            StopAttack();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
            if (_isAttacking)
            {
                RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
                //bulletCasings.Emit(1); //Emit the bullet casing particle effect  
                AdjustRange();
            }
        }

        void AdjustRange()
        {
            Vector3 targetPos = _target.transform.position;
            Vector3 diff = targetPos - transform.position;
            //Debug.DrawRay(_tracerGO.transform.position, distScale, Color.cyan, 0.2f);
            float scaleValue = diff.magnitude * _tracerScaleX / 15f;
            for (int i = 0; i < _tracerGO.Length; i++)
            {
                _tracerGO[i].transform.localScale = new Vector3(scaleValue, 0.3f, 0.2f);
            }
        }

        // Method to rotate gun barrel 
        void RotateBarrel()
        {
            for (int i = 0; i < _gunBarrels.Length; i++)
            {
                _gunBarrels[i].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
                _bulletCasings[i].Emit(1); //Emit the bullet casing particle effect   
            }

        }

        protected override void StartAttack(GameObject target)
        {
            _target = target;
            StartCoroutine("SendDamage");
            //Muzzle_Flash.SetActive(true); //enable muzzle effect particle effect
            for (int i = 0; i < _muzzleFlashes.Length; i++)
            {
                _muzzleFlashes[i].SetActive(true); //enable muzzle effect particle effect
                //_bulletCasings[i].Emit(1); //Emit the bullet casing particle effect   
            }

            if (_startWeaponNoise == true) //checking if we need to start the gun sound
            {
                _audioSource.Play(); //play audio clip attached to audio source
                _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
            }

        }

        protected override void StopAttack()
        {
            _target = null;

            //Muzzle_Flash.SetActive(false); //turn off muzzle flash particle effect
            for (int i = 0; i < _muzzleFlashes.Length; i++)
            {
                _muzzleFlashes[i].SetActive(false); //enable muzzle effect particle effect
                //_bulletCasings[i].Emit(0); //Emit the bullet casing particle effect   
            }

            _audioSource.Stop(); //stop the sound effect from playing
            _startWeaponNoise = true; //set the start weapon noise value to true

            StopCoroutine("SendDamage");
        }

        private IEnumerator SendDamage()
        {
            while (_target != null)
            {
                yield return new WaitForSeconds(0.1f);
                TowerBattleReady.OnTakeDamage(_target, _damageRate);
            }
        }
    }

}
