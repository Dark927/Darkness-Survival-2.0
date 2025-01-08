using UnityEngine;

public class AnimatorController
{
    #region Fields 

    private IAnimatorParameters _parameters;
    private Animator _animator;

    #endregion


    #region Properties

    public Animator Animator { get => _animator; private set => _animator = value; }
    public IAnimatorParameters Parameters { get => _parameters; private set => _parameters = value; }

    #endregion


    #region Methods

    #region Init

    public AnimatorController(Animator characterAnimator, IAnimatorParameters parameters)
    {
        Animator = characterAnimator;
        Parameters = parameters;
    }

    #endregion

    #endregion
}
