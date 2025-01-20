using System;
using UnityEngine;
using World.Components;
using World.Tile;
using Zenject;

public class WorldGenerator : MonoBehaviour
{
    #region Fields 

    private GenerationStrategy _generationStrategy;

    #endregion


    #region Methods

    [Inject]
    public void Construct(GenerationStrategy generationStrategy)
    {
        _generationStrategy = generationStrategy;
    }

    private void Awake()
    {
        try
        {
            _generationStrategy.Init();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {

        _generationStrategy.UpdateTilesOnScreen();
    }

    private void Update()
    {
        _generationStrategy.TryUpdateTilesOnScreen();
    }

    #endregion
}
