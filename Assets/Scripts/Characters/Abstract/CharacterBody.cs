
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
        InitMovement();
        InitView();
        InitAnimation();

        InitReferences();
    }

    protected abstract void InitView();
    protected abstract void InitMovement();
    protected abstract void InitAnimation();
    protected abstract void InitReferences();

    #endregion

    #endregion
}
