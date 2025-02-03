
using Characters.Health;
using Characters.Interfaces;
using System;
using UnityEngine;
using Utilities.Characters;

public abstract class CharacterBody : MonoBehaviour
{
    #region Fields

    private ICharacterMovement _movement;
    private ICharacterView _view;
    private CharacterVisual _visual;
    private IHealth _characterHealth;
    public IInvincibility _characterInvincibility;

    public event Action OnBodyDeath;
    public event Action OnBodyDamaged;

    #endregion


    #region Properties

    public ICharacterMovement Movement { get => _movement; protected set => _movement = value; }
    public ICharacterView View { get => _view; protected set => _view = value; }
    public CharacterVisual Visual { get => _visual; protected set => _visual = value; }
    public IHealth Health { get => _characterHealth; protected set => _characterHealth = value; }
    public IInvincibility Invincibility { get => _characterInvincibility; protected set => _characterInvincibility = value; }


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
        InitReferences();
    }

    protected abstract void Init();

    protected virtual void InitMovement()
    {
        _movement = new CharacterStaticMovement();
    }

    protected virtual void InitView()
    {

    }

    protected virtual void InitReferences()
    {

    }

    #endregion

    protected virtual void ClearReferences()
    {

    }

    protected void OnDestroy()
    {
        ClearReferences();
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
