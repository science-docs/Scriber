using Scriber.Bibliography.Styling.Specification;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography.Styling.Renderer
{
    public class NameGroup
    {
        public NameGroup(string? variable, TermName? term, IEnumerable<IName> names)
        {
            // init
            Variable = variable;
            Term = term;
            Names = names?.ToArray();
        }

        public string? Variable
        {
            get;
            private set;
        }
        public TermName? Term
        {
            get;
            private set;
        }
        public IName[]? Names
        {
            get;
            private set;
        }

        public static bool AreNamesEqual(IName name1, IName name2)
        {
            // type?
            if (name1 is IPersonalName pname1 && name2 is IPersonalName pname2)
            {
                // done
                return string.Compare(pname1.FamilyName, pname2.FamilyName, true) == 0 &&
                    string.Compare(pname1.GivenNames, pname2.GivenNames, true) == 0 &&
                    string.Compare(pname1.DroppingParticles, pname2.DroppingParticles, true) == 0 &&
                    string.Compare(pname1.NonDroppingParticles, pname2.NonDroppingParticles, true) == 0 &&
                    string.Compare(pname1.Suffix, pname2.Suffix, true) == 0;
            }
            else if (name1 is IInstitutionalName iname1 && name2 is IInstitutionalName iname2)
            {
                // done
                return string.Compare(iname1.Name, iname2.Name, true) == 0;
            }
            else
            {
                return false;
            }
        }
    }
}
