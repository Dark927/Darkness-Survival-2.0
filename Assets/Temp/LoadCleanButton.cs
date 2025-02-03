using Settings.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UI.Buttons;
using UnityEngine;

public class LoadCleanButton : ButtonBaseUI 
{
    public override void ClickListener()
    {
        GameSceneManager.Instance.CleanLoad();
    }
}
