using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class CharacterVisual : MonoBehaviour
{
    #region Fields

    private SpriteRenderer _spriteRenderer;

    #endregion

    public SpriteRenderer Renderer => _spriteRenderer;
    public Sprite CharacterSprite { get => _spriteRenderer.sprite; set { _spriteRenderer.sprite = value; } }
    public bool HasAnimation { get => GetAnimatorController() != null; }
    public bool IsVisibleForCamera { get => _spriteRenderer.isVisible; }


    #region Methods
    public abstract AnimatorController GetAnimatorController();
    public abstract T GetAnimatorController<T>() where T : AnimatorController;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion
}
