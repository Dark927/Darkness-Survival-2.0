
using Materials;
using Materials.DarkEntityFX;
using Settings.Global;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class NeroSwordVisual : MonoBehaviour, IInitializable
    {
        #region Fields 

        private SpriteRenderer _spriteRenderer;
        private DarkEntityFXComponent _darkMainFXComponent;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _darkMainFXComponent = GetComponent<DarkEntityFXComponent>();
            Deactivate();
        }

        #endregion

        public void Activate()
        {
            _spriteRenderer.enabled = true;
        }

        public void Deactivate()
        {
            _spriteRenderer.enabled = false;
        }

        public void SetAura(ScriptableMaterialPropsBase auraFxData, ParametricProps properties)
        {
            if (_darkMainFXComponent == null)
            {
                return;
            }

            _darkMainFXComponent.MaterialPropContainer.ConstMaterialProps = auraFxData;
            _darkMainFXComponent.MaterialPropContainer.Properties = properties;
        }


        #endregion
    }
}
