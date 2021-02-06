using System.IO;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System;
using System.Xml;

namespace Scriber.Bibliography.Styling.Tests
{
    public class CslTest
    {
        [Theory]
        [MemberData(nameof(CollectTests))]
        public void TestCslSuite(string testSuitePath)
        {
            var fileContent = File.ReadAllText(testSuitePath).Replace('\r', '\n').Replace("\n\n", "\n");
            var suite = CslTestSuiteParser.Parse(fileContent);
            try
            {
                var result = suite.Evaluate();
                if (suite.Result != result)
                {
                    Console.WriteLine("Csl Test failed!");
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (NotImplementedException)
            {
                Console.WriteLine("Csl Test not implemented.");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Csl Test not supported.");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        public static IEnumerable<object[]> CollectTests()
        {
            var tempName = Path.Combine("Resources", "Csl", "processor-tests", "humans");
            if (!Directory.Exists(tempName))
            {
                return Array.Empty<object[]>();
            }
            return Directory.GetFiles(tempName).Select(e => e.Replace('\\', '/')).Except(Excludes).Select(e => new object[] { e });
        }

        private static readonly string[] Excludes = new string[]
        {
            "Resources/Csl/processor-tests/humans/bugreports_GreekStyleProblems.txt",
            "Resources/Csl/processor-tests/humans/bugreports_GreekStyleTwoEditors.txt",
            "Resources/Csl/processor-tests/humans/collapse_TrailingDelimiter.txt",
            "Resources/Csl/processor-tests/humans/disambiguate_YearSuffixMixedDates.txt",
            "Resources/Csl/processor-tests/humans/fullstyles_ABdNT.txt",
            "Resources/Csl/processor-tests/humans/label_MissingReturnsEmpty.txt",
            "Resources/Csl/processor-tests/humans/name_InstitutionDecoration.txt",
            "Resources/Csl/processor-tests/humans/name_NoNameNode.txt",
            "Resources/Csl/processor-tests/humans/sort_VariousNameMacros1.txt",
            "Resources/Csl/processor-tests/humans/sort_VariousNameMacros2.txt",
            "Resources/Csl/processor-tests/humans/sort_VariousNameMacros3.txt",
            "Resources/Csl/processor-tests/humans/textcase_LocaleUnicode.txt",
            // Date Literals are not supported in CSL 1.0.1
            "Resources/Csl/processor-tests/humans/date_OtherAlone.txt",
        };
    }
}
