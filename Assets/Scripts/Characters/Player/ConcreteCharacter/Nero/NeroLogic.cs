using System.Collections.Generic;
using System.Linq;
using Characters.Common.Combat.Weapons;
using Characters.Player.Animation;
using Characters.Player.Weapons;
using Cysharp.Threading.Tasks;
using Settings.Global;
using Settings.Global.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class NeroLogic : PlayerCharacterLogic
    {
        public List<AudioClip> TestingAudioClips;

        private bool isAltQPressed = false;
        private bool isPPressed = false;

        private void Update()
        {
            var audioProvider = ServiceLocator.Current.Get<GameAudioService>();
            var SoundsPlayer = audioProvider.SoundsPlayer;

            // Handling Alt + Q key combination
            if (Keyboard.current.altKey.isPressed && Keyboard.current.qKey.isPressed)
            {
                if (!isAltQPressed)
                {
                    Debug.Log("press combination");
                    if (TestingAudioClips == null)
                    {
                        return;
                    }
                    SoundsPlayer.Play3DSound(TestingAudioClips.ElementAt(Random.Range(0, TestingAudioClips.Count())));
                    isAltQPressed = true;  // Set the flag to true to prevent further triggers until released
                }
            }
            else
            {
                isAltQPressed = false;  // Reset the flag when the keys are released
            }

            // Handling P key press
            if (Keyboard.current.pKey.isPressed)
            {
                if (!isPPressed)
                {
                    Debug.Log("press single");
                    if (TestingAudioClips == null)
                    {
                        return;
                    }
                    SoundsPlayer.Play3DSound(TestingAudioClips.ElementAt(Random.Range(0, TestingAudioClips.Count())), position: transform.position, maxDistance: 10f);
                    isPPressed = true;  // Set the flag to true to prevent further triggers until released
                }
            }
            else
            {
                isPPressed = false;  // Reset the flag when the key is released
            }
        }

        #region Fields

        private CharacterAnimatorController _animatorController;
        private bool _hasConfiguredLinks = false;

        #endregion


        #region Properties


        #endregion


        #region Methods

        #region Init

        protected override void InitBasicAttacks()
        {
            SetBasicAttacks(new NeroBasicAttacks(this, Weapons.ActiveOnesDict.Values));
            base.InitBasicAttacks();
        }

        public override void ConfigureEventLinks()
        {
            if (_hasConfiguredLinks)
            {
                return;
            }

            base.ConfigureEventLinks();

            if (BasicAttack != null)
            {
                BasicAttack.ConfigureEventLinks();
            }
            else
            {
                OnBasicAttacksReady += ConfigureBasicAttacksEventListener;
            }

            _hasConfiguredLinks = true;
        }

        private void ConfigureBasicAttacksEventListener(BasicAttack attack)
        {
            OnBasicAttacksReady -= ConfigureBasicAttacksEventListener;
            attack.ConfigureEventLinks();
        }

        public override void RemoveEventLinks()
        {
            if (!_hasConfiguredLinks)
            {
                return;
            }

            base.RemoveEventLinks();
            BasicAttack?.RemoveEventLinks();

            _hasConfiguredLinks = false;
        }

        protected override void SetReferences()
        {
            base.SetReferences();
            _animatorController = Body.Visual.GetAnimatorController() as CharacterAnimatorController;
        }

        public override void Dispose()
        {
            base.Dispose();
            RemoveEventLinks();
            _animatorController = null;
        }

        #endregion

        public override void ListenNewWeaponGiven(IWeapon weapon)
        {
            base.ListenNewWeaponGiven(weapon);

            if (weapon is CharacterSword weaponSword)
            {
                ((NeroBasicAttacks)BasicAttacks).SetSword(weaponSword);
            }
        }

        #endregion
    }
}

