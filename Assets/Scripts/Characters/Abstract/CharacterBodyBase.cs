
using System;
using Characters.Health;
using Characters.Interfaces;
using UnityEngine;
using Utilities.Characters;

public abstract class CharacterBodyBase : MonoBehaviour, IDisposable
{
    #region Fields

    private CharacterMovementBase _movement;
    private ICharacterView _view;
    private CharacterVisual _visual;
    private IHealth _characterHealth;
    public IInvincibility _characterInvincibility;



    #endregion


    #region Events

    public event Action OnBodyDeath;
    public event Action OnBodyDamaged;

    #endregion


    #region Properties

    public CharacterMovementBase Movement { get => _movement; protected set => _movement = value; }
    public ICharacterView View { get => _view; protected set => _view = value; }
    public CharacterVisual Visual { get => _visual; protected set => _visual = value; }
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
        InitView();

#if UNITY_EDITOR
        CharacterSettingsValidator.CheckCharacterBodyStatus(this);
#endif
    }

    protected virtual void Start()
    {
        SetReferences();
    }

    protected abstract void Init();

    protected virtual void InitMovement()
    {
        _movement = new CharacterStaticMovement();
    }

    protected virtual void InitView()
    {

    }

    protected virtual void SetReferences()
    {

    }

    protected virtual void UnsetReferences()
    {

    }

    #endregion

    public virtual void Dispose()
    {

    }

    protected void OnDestroy()
    {
        Dispose();
    }

    protected void RaiseOnBodyDeath()
    {
        OnBodyDeath?.Invoke();
    }

    protected void RaiseOnBodyDamaged()
    {
        OnBodyDamaged?.Invoke();
    }

    #endregion
}
