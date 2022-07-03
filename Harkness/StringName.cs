namespace Harkness
{
    public class StringName : IName
    {
        private readonly string name;

        public StringName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
