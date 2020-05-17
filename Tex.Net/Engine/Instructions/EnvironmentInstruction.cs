namespace Tex.Net.Engine.Instructions
{
    public class EnvironmentInstruction : EngineInstruction
    {
        public bool IsEnd { get; set; }

        public override object? Execute(CompilerState state, object[] args)
        {
            if (args.Length == 0)
            {
                throw new CommandInvocationException("An environment needs an argument specifying its name");
            }

            var firstArg = args[0];
            var converter = ElementConverters.Find(firstArg.GetType(), typeof(string));

            if (converter == null)
            {
                throw new CommandInvocationException("Could not create string converter for type " + firstArg.GetType().Name);
            }

            if (!(converter.Convert(firstArg, typeof(string), state) is string name))
            {
                throw new CommandInvocationException("Could not create environment name from argument");
            }

            var env = EnvironmentCollection.Find(name);
            
            if (env == null)
            {
                throw new CommandInvocationException("Could not find environment: " + name);
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
                var newEnv = new Block(name);
                newEnv.Arguments.AddRange(args);
                return newEnv;
            }
        }

        public static EnvironmentInstruction Create()
        {
            return new EnvironmentInstruction();
        }
    }
}
