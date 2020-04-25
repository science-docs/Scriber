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
            
            if (env == null)
            {
                throw new CommandInvocationException("Could not find environment named: " + name);
            }

            if (IsEnd)
            {
                state.Blocks.Pop();
                var cur = state.Blocks.Current;
                var objects = cur.Objects.ToArray();
                var envArgs = cur.Arguments.ToArray();
                return env.Execution(state, objects, envArgs);
            }
            else
            {
                var newEnv = new Block(name)
                {
                    Environment = env
                };
                newEnv.Arguments.AddRange(args);
                return newEnv;
            }
        }

        public static new EnvironmentInstruction Create(Element element)
        {
            return new EnvironmentInstruction();
        }
    }
}
