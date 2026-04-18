using System;
using Characters.Player.Controls;
using Settings.Global.Audio;

namespace Settings.Global
{
    [Serializable]
    public class GameSettingsData
    {
        public AudioSaveData Audio = new AudioSaveData();
        public InputSaveData Input = new InputSaveData();
        public GraphicsSaveData Graphics = new GraphicsSaveData();
    }
}
