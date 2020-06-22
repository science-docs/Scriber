using System.Linq;

namespace Scriber.Bibliography
{
    public class PersonalName : Name, IPersonalName
    {
        public PersonalName(string familyName, string givenNames)
            : this(familyName, givenNames, null, false, null, null)
        {
        }
        public PersonalName(string familyName, string givenNames, string? suffix, bool precedeSuffixByComma, string? droppingParticles, string? nonDroppingParticles)
        {
            FamilyName = familyName;
            GivenNames = givenNames;
            Suffix = suffix;
            PrecedeSuffixByComma = precedeSuffixByComma;
            DroppingParticles = droppingParticles;
            NonDroppingParticles = nonDroppingParticles;
        }

        public string FamilyName
        {
            get;
            private set;
        }
        public string GivenNames
        {
            get;
            private set;
        }

        public string? Suffix
        {
            get;
            private set;
        }
        public bool PrecedeSuffixByComma
        {
            get;
            private set;
        }

        public string? DroppingParticles
        {
            get;
            private set;
        }
        public string? NonDroppingParticles
        {
            get;
            private set;
        }

        public override bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(FamilyName) && string.IsNullOrWhiteSpace(GivenNames) && string.IsNullOrWhiteSpace(NonDroppingParticles) && string.IsNullOrWhiteSpace(DroppingParticles) && string.IsNullOrWhiteSpace(Suffix);
            }
        }

        public override string ToString()
        {
            // init
            var parts = new string?[] { GivenNames, DroppingParticles, NonDroppingParticles, FamilyName, Suffix };

            // done
            return string.Join(" ", parts.Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}
