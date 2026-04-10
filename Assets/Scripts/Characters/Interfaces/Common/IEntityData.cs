namespace Characters.Common.Settings
{
    public interface IEntityData
    {
        public EntityInfo CommonInfo { get; }
        public CharacterStats Stats { get; }
    }
}
