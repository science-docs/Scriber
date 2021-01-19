using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scriber.Bibliography
{
    public class NamesVariable : INamesVariable
    {
        private readonly List<IName> items;

        public NamesVariable(IEnumerable<IName> names)
        {
            // init
            items = names.ToList();
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }
        public IName this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public IEnumerator<IName> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<IName> IEnumerable<IName>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static NamesVariable Parse(string value, string outerSplitter, string innerSplitter)
        {
            var names = new List<IName>();
            outerSplitter = " " + outerSplitter.Trim() + " ";
            var outerSplits = value.Split(outerSplitter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var fullName in outerSplits)
            {
                var trimmedName = fullName.Trim();
                var nameSplits = trimmedName.Split(innerSplitter, StringSplitOptions.RemoveEmptyEntries);

                if (nameSplits.Length == 1)
                {
                    names.Add(new InstitutionalName(nameSplits[0].Trim()));
                }
                else if (nameSplits.Length == 2)
                {
                    names.Add(new PersonalName(nameSplits[0].Trim(), nameSplits[1].Trim()));
                }
            }

            return new NamesVariable(names);
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(x => x.ToString()));
        }
    }
}
