namespace Scriber.Bibliography
{
    public class InstitutionalName : Name, IInstitutionalName
    {
        public InstitutionalName(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public override bool IsEmpty => string.IsNullOrWhiteSpace(Name);

        public override string ToString()
        {
            // done
            return Name;
        }
    }
}
