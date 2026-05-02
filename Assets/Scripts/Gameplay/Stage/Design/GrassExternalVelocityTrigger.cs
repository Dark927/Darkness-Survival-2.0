using System.Collections;
using Characters.Common;
using UnityEngine;

namespace Gameplay.Stage.Design
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GrassExternalVelocityTrigger : MonoBehaviour
    {
        #region Fields 

        private GrassVelocityController _grassVelocityController;
        private Material _material;

        private bool _easeInCoroutineRunning;
        private bool _easeOutCoroutineRunning;

        private float _startingVelocityX;
        private float _velocityLastFrame;

        #endregion


        #region Methods

        #region Init

        private void Start()
        {
            _grassVelocityController = GetComponentInParent<GrassVelocityController>();
            _material = GetComponent<SpriteRenderer>().material;
            _startingVelocityX = _material.GetFloat(GrassVelocityController.ExternalInfluencePropID);
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out ICharacterLogic characterLogic))
            {
                return;
            }

            float playerVelocityX = characterLogic.Body.Physics.Rigidbody2D.velocity.x;

            if (!_easeInCoroutineRunning
                && (Mathf.Abs(playerVelocityX) > Mathf.Abs(_grassVelocityController.VelocityThreshold)))
            {
                StartCoroutine(EaseInRoutine(playerVelocityX * _grassVelocityController.ExternalInfluenceMult));
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out ICharacterLogic characterLogic))
            {
                return;
            }

            float playerVelocityX = characterLogic.Body.Physics.Rigidbody2D.velocity.x;
            float playerVelocityXAbs = Mathf.Abs(playerVelocityX);
            float grassVelocityThresholdAbs = Mathf.Abs(_grassVelocityController.VelocityThreshold);
            float velocityLastFrameAbs = Mathf.Abs(_velocityLastFrame);


            if (!_easeOutCoroutineRunning
                && (velocityLastFrameAbs > grassVelocityThresholdAbs)
                && (playerVelocityXAbs < grassVelocityThresholdAbs))
            {
                StartCoroutine(EaseOutRoutine());
            }
            else if (!_easeInCoroutineRunning
                && (velocityLastFrameAbs < grassVelocityThresholdAbs)
                && (playerVelocityXAbs > grassVelocityThresholdAbs))
            {
                StartCoroutine(EaseInRoutine(playerVelocityX * _grassVelocityController.ExternalInfluenceMult));
            }

            _velocityLastFrame = playerVelocityX;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out ICharacterLogic characterLogic))
            {
                return;
            }

            if (!_easeOutCoroutineRunning)
            {
                StartCoroutine(EaseOutRoutine());
            }
        }


        private IEnumerator EaseInRoutine(float velocityX)
        {
            _easeInCoroutineRunning = true;

            float elapsedTime = 0f;

            while (elapsedTime < _grassVelocityController.EaseInTimeSec)
            {
                elapsedTime += Time.deltaTime;

                float lerpedAmount = Mathf.Lerp(_startingVelocityX, velocityX, (elapsedTime / _grassVelocityController.EaseInTimeSec));
                _grassVelocityController.InfluenceGrass(_material, lerpedAmount);

                yield return null;
            }

            _easeInCoroutineRunning = false;
        }

        private IEnumerator EaseOutRoutine()
        {
            _easeOutCoroutineRunning = true;
            float currentInfluenceX = _material.GetFloat(GrassVelocityController.ExternalInfluencePropID);

            float elapsedTime = 0f;

            while (elapsedTime < _grassVelocityController.EaseOutTimeSec)
            {
                elapsedTime += Time.deltaTime;

                float lerpedAmount = Mathf.Lerp(currentInfluenceX, _startingVelocityX, (elapsedTime / _grassVelocityController.EaseOutTimeSec));
                _grassVelocityController.InfluenceGrass(_material, lerpedAmount);

                yield return null;
            }

            _easeOutCoroutineRunning = false;
        }

        #endregion
    }
}
