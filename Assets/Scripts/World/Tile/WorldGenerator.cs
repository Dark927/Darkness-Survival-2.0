using UnityEngine;
using World.Tile;
using Zenject;

public class WorldGenerator : MonoBehaviour
{
    private GenerationStrategy _generationStrategy;


    [Inject]
    public void Construct(GenerationStrategy generationStrategy)
    {
        _generationStrategy = generationStrategy;
    }

    private void Awake()
    {
        _generationStrategy.Init();
    }

    private void Start()
    {

        _generationStrategy.UpdateTilesOnScreen();
    }

    private void Update()
    {
        _generationStrategy.TryUpdateTilesOnScreen();
    }
}
