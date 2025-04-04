using System;
using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Common.Settings;
using Cysharp.Threading.Tasks;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityControllerBase
    {
        #region Fields 

        private PlayerInput _input;
        private bool _canAttack = true;
        [SerializeField] private EntityBaseData _characterData;

        #endregion


        #region Properties

        public ICharacterLogic CharacterLogic => EntityLogic as ICharacterLogic;
        public PlayerInput Input => _input;

        #endregion


        #region Events

        public event Action<PlayerCharacterController> OnCharacterDeathEnd;
        public event Action<PlayerCharacterController> OnCharacterDies;

        #endregion


        #region Methods

        #region Init 

        private void Awake()
        {
            Initialize(_characterData);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            InitInput();
            ConfigureCharacter(data);
            InitFeaturesAsync().Forget();
        }

        protected override void Start()
        {
            base.Start();
            ServiceLocator.Current.Get<PlayerService>()?.AddPlayer(this);

            ConfigureEventLinks();
        }

        private void ConfigureCharacter(IEntityData data)
        {
            EntityLogic.Initialize(data);
            EntityLogic.ConfigureEventLinks();
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            PlayerCharacterVisual visual = (EntityLogic.Body.Visual as PlayerCharacterVisual);

            EntityLogic.ConfigureEventLinks();
            EntityLogic.Body.OnBodyDies += RaiseCharacterDies;
            EntityLogic.Body.OnBodyDiedCompletely += RaiseCharacterCompletelyDied;
        }

        private void InitInput()
        {
            _input = GetComponent<PlayerInput>();
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();
            EntityLogic.RemoveEventLinks();
            EntityLogic.Body.OnBodyDies -= RaiseCharacterDies;

            EntityLogic.Body.OnBodyDiedCompletely -= RaiseCharacterCompletelyDied;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion


        public void OnMovement(InputValue value)
        {
            if (CharacterLogic.Body.Movement == null)
            {
                return;
            }

            Vector2 direction = value.Get<Vector2>();

            if (direction.sqrMagnitude > 0)
            {
                CharacterLogic.Body.Movement.MoveAsync(direction).Forget();
            }
            else
            {
                CharacterLogic.Body.Movement.Stop();
            }
        }


        public void SetCanAttackFlag(bool value)
        {
            _canAttack = value;
        }

        public void OnAttacks(InputValue value)
        {
            if (!_canAttack)
            {
                return;
            }

            if (value.isPressed)
            {
                int contextValue = (int)value.Get<float>();
                CharacterLogic.PerformBasicAttack((BasicAttack.LocalType)contextValue);
            }
        }

        private void RaiseCharacterDies()
        {
            CharacterLogic.Body.Physics.SetStatic();
            OnCharacterDies?.Invoke(this);
        }

        private void RaiseCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke(this);
        }

        #endregion
    }
}
