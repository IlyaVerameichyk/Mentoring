using System;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using NLog;

namespace Service.Interceptors
{
    public class TraceInterceptor : IInterceptor
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Intercept(IInvocation invocation)
        {
            string serializedArguments;
            try
            {
                serializedArguments = JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (Exception e)
            {
                serializedArguments = "Not serializable";
                Logger.Error(e);
            }
            Logger.Trace($"Castle.Core: Entering method \"{invocation.Method.Name}\"; Arguments: {serializedArguments}");
            invocation.Proceed();
        }
    }
}