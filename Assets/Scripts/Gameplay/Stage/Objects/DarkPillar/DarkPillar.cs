using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Stage.Objects
{

    public class DarkPillar : MonoBehaviour
    {
        #region Fields 

        private List<PillarPart> _pillarParts;
        private ActivatableAnimatorController _animatorController;
        private bool _isActive = false;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            Animator animator = GetComponentInChildren<Animator>();
            _animatorController = new ActivatableAnimatorController(animator, new ActivatableAnimatorParameters());

            _pillarParts = GetComponentsInChildren<PillarPart>().ToList();

            foreach (var part in _pillarParts)
            {
                part.Initialize();
            }
        }

        private void Update()
        {
            // ToDo : use night/day events instead of this
            if (Input.GetKeyDown(KeyCode.E))
            {
                Activate();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Deactivate();
            }
        }

        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _animatorController.TriggerActivation();

            foreach (var part in _pillarParts)
            {
                part.Activate();
            }

            _isActive = true;
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }

            _animatorController.TriggerDeactivation();

            foreach (var part in _pillarParts)
            {
                part.Deactivate();
            }

            _isActive = false;
        }

        #endregion

        #endregion
    }
}
