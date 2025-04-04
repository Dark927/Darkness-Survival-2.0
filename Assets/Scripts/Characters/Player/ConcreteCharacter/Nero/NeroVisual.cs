
using System;
using Materials;
using Materials.DarkEntityFX;
using UnityEngine;

namespace Characters.Player
{
    public class NeroVisual : PlayerCharacterVisual
    {
        [SerializeField] private float _startEmissionAmount = 0.25f;
        private NeroSwordVisual _swordVisual;

        [SerializeField] private bool _setSwordVisualSame = true;

        public override void Initialize()
        {
            base.Initialize();
            _swordVisual = GetComponentInChildren<NeroSwordVisual>(true);
            _swordVisual.Initialize();

            var properties = EntityFXComponent.MaterialPropContainer.Properties;

            properties.RendMode = Materials.DarkMainFX.RendererMode.IGNORE;
            properties.EmissionAmount = _startEmissionAmount;
            EntityFXComponent.MaterialPropContainer.Properties = properties;

            if (_setSwordVisualSame && EntityFXComponent.MaterialPropContainer.ConstMaterialProps)
            {
                SetSwordAura(EntityFXComponent.MaterialPropContainer.ConstMaterialProps, properties);
            }
        }

        public void SetAura(ScriptableMaterialPropsBase auraFxData, float emissionAmount = 0.5f)
        {
            EntityFXComponent.MaterialPropContainer.ConstMaterialProps = auraFxData;

            var properties = EntityFXComponent.MaterialPropContainer.Properties;
            properties.EmissionAmount = emissionAmount;
            EntityFXComponent.MaterialPropContainer.Properties = properties;

            SetSwordAura(auraFxData, properties);
        }

        public void EnableSword()
        {
            DoSwordAction(_swordVisual.Activate);
        }

        public void DisableSword()
        {
            DoSwordAction(_swordVisual.Deactivate);
        }

        private void DoSwordAction(Action action)
        {
            if (_swordVisual == null)
            {
                return;
            }

            action();
        }

        private void SetSwordAura(ScriptableMaterialPropsBase auraFxData, ParametricProps properties)
        {
            if (_swordVisual == null)
            {
                return;
            }

            _swordVisual.SetAura(auraFxData, properties);
        }
    }
}
