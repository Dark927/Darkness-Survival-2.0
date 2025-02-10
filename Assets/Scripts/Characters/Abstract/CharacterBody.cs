
using UnityEngine;

public abstract class CharacterBody : MonoBehaviour
{
    #region Fields

    private ICharacterMovement _movement;
    private ICharacterView _view;
    private CharacterVisual _visual;

    #endregion


    #region Properties

    public ICharacterMovement Movement { get => _movement; protected set => _movement = value; }
    public ICharacterView View { get => _view; protected set => _view = value; }
    public CharacterVisual Visual { get => _visual; protected set => _visual = value; } 

    #endregion


    #region Methods 

    #region Init

    private void Awake()
    {
        Init();
        InitMovement();
        InitView();

        InitReferences();

#if UNITY_EDITOR
        CheckComponentsStatus();
#endif
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

    // ToDo : Move this debug logic to the separate component 

    // Debug
    private void CheckComponentsStatus()
    {
        if (View == null)
        {
            ErrorHandling.ErrorLogger.LogComponentIsNull(ErrorHandling.LogOutputType.Console, gameObject.name, nameof(View));
        }

        if (Movement == null)
        {
            ErrorHandling.ErrorLogger.LogComponentIsNull(ErrorHandling.LogOutputType.Console, gameObject.name, nameof(Movement));
        }

        if (Visual == null)
        {
            ErrorHandling.ErrorLogger.LogComponentIsNull(ErrorHandling.LogOutputType.Console, gameObject.name, nameof(Visual));
        }
    }


    #endregion
}
