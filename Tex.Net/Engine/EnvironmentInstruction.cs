using Tex.Net.Language;

namespace Tex.Net.Engine
{
    public class EnvironmentInstruction : EngineInstruction
    {
        public bool IsEnd { get; set; }

        public override object Execute(CompilerState state, object[] args)
        {
            if (args.Length == 0)
            {
                throw new CommandInvocationException("An environment needs an argument specifying its name");
            }

            var firstArg = args[0];
            var converter = ElementConverters.Find(firstArg.GetType(), typeof(string));
            var name = converter.Convert(firstArg, typeof(string), state) as string;

            var env = EnvironmentCollection.Find(name);
            

            if (IsEnd)
            {
                var cur = state.Environments.Current;
                var objects = cur.Objects.ToArray();
                var envArgs = cur.Arguments.ToArray();
                state.Environments.Pop();
                return env.Execution(state, objects, envArgs);
            }
            else
            {
                var cur = state.Environments.Push(name);
                cur.Instance = env;
                cur.Arguments.AddRange(args);
                return null;
            }
        }

        public static new EnvironmentInstruction Create(Element element)
        {
            return new EnvironmentInstruction();
        }
    }
}
