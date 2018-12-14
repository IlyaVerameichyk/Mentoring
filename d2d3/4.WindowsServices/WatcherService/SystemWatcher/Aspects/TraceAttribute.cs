using System;
using Newtonsoft.Json;
using NLog;
using PostSharp.Aspects;

namespace SystemWatcher.Aspects
{
    [Serializable]
    public sealed class TraceAttribute : OnMethodBoundaryAspect
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public override void OnEntry(MethodExecutionArgs args)
        {
            string serializedArguments;
            try
            {
                serializedArguments = JsonConvert.SerializeObject(args.Arguments);
            }
            catch (Exception e)
            {
                serializedArguments = "Not serializable";
                Logger.Error(e);
            }
            Logger.Trace($"Postsharp: Entering method \"{args.Method.Name}\". Arguments: {serializedArguments}");
            base.OnEntry(args);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Logger.Trace($"Postsharp: Exit method \"{args.Method.Name}\"");
            base.OnExit(args);
        }
    }
}