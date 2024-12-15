
using UnityEngine;

public abstract class CharacterBody : MonoBehaviour
{
    #region Fields

    private ICharacterMovement _movement;
    private ICharacterView _view;

    #endregion


    #region Properties

    public ICharacterMovement Movement { get => _movement; protected set => _movement = value; }
    public ICharacterView View { get => _view; protected set => _view = value; }

    #endregion


    #region Methods 

    #region Init

    private void Awake()
    {
        Init();
        InitMovement();
        InitView();

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

    protected void OnDisable()
    {
        ClearReferences();
    }

    #endregion
}
