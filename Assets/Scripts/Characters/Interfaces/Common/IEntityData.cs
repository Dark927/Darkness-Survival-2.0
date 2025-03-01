namespace Characters.Stats
{
    public interface IEntityData
    {
        public int ID { get; }
        public string Name { get; }
        public CharacterStats Stats { get; }
    }
}
