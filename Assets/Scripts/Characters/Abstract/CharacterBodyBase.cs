
using Characters.Common.Movement;
using Characters.Common.Visual;
using Characters.Health;
using Characters.Interfaces;
using System;
using UnityEngine;
using Utilities.Characters;

public abstract class CharacterBodyBase : MonoBehaviour, ICharacterBody
{
    #region Fields

    private CharacterMovementBase _movement;
    private ICharacterView _view;
    private EntityVisualBase _visual;
    private IHealth _characterHealth;
    public IInvincibility _characterInvincibility;

    #endregion


    #region Events

    public event Action OnBodyDies;
    public event Action OnBodyDied;
    public event Action OnBodyDamaged;

    #endregion


    #region Properties

    public Transform Transform => transform;
    public CharacterMovementBase Movement { get => _movement; protected set => _movement = value; }
    public ICharacterView View { get => _view; protected set => _view = value; }
    public EntityVisualBase Visual { get => _visual; protected set => _visual = value; }
    public IHealth Health { get => _characterHealth; protected set => _characterHealth = value; }
    public IInvincibility Invincibility { get => _characterInvincibility; protected set => _characterInvincibility = value; }
    public bool IsDead => (Health != null) && Health.IsEmpty;


    #endregion


    #region Methods 

    #region Init

    protected virtual void Awake()
    {
        Init();
        InitMovement();
        Visual.Initialize();
        InitView();
        PostInit();

#if UNITY_EDITOR
        CharacterSettingsValidator.CheckCharacterBodyStatus(this);
#endif
    }

    protected virtual void Start()
    {

    }

    protected abstract void Init();

    protected virtual void InitMovement()
    {
        _movement = new CharacterStaticMovement();
    }

    protected virtual void InitView()
    {

    }

    protected virtual void PostInit()
    {

    }

    public virtual void ConfigureEventLinks()
    {
        Movement?.ConfigureEventLinks();
    }

    public virtual void RemoveEventLinks()
    {
        Movement?.RemoveEventLinks();
    }

    public virtual void Dispose()
    {

    }

    protected void OnDestroy()
    {
        Dispose();
    }

    #endregion

    public virtual void ResetState()
    {
        Health.ResetState();
    }

    protected void RaiseOnBodyDies()
    {
        OnBodyDies?.Invoke();
    }

    protected void RaiseOnBodyCompletelyDied()
    {
        OnBodyDied?.Invoke();
    }

    protected void RaiseOnBodyDamaged()
    {
        OnBodyDamaged?.Invoke();
    }

    #endregion
}
