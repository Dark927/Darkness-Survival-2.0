using Characters.Player;
using Settings.Abstract;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Linq;

public sealed class PlayerManager : SingletonBase<PlayerManager>
{
    private List<Player> _players;

    [Inject]
    public void Construct(Player player)
    {
        _players = new List<Player>
        {
            player
        };
    }

    private void Awake()
    {
        InitInstance();
    }

    private void Start()
    {
        GameManager.Instance.SetPlayers(_players);
    }


    public ICharacterLogic GetCharacter()
    {
        // ToDo : implement the logic for several players in the future.
        return _players.FirstOrDefault().Character;
    }

    public Transform GetCharacterTransform()
    {
        Transform characterTransform = null;
        ICharacterLogic character = GetCharacter();

        if (character != null)
        {
            characterTransform = (character.Body != null) ? character.Body.transform : null;
        }

        return characterTransform;
    }
}
