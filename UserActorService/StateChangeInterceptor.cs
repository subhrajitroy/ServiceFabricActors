using System;
using Castle.DynamicProxy;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace UserActorService
{
    public class StateChangeInterceptor : IInterceptor
    {
        private readonly IActorStateManager stateManager;

        public StateChangeInterceptor(IActorStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            var invocationMethod = invocation.Method;
            if (invocationMethod.Name.StartsWith("set_",StringComparison.OrdinalIgnoreCase))
            {
                var invocationInvocationTarget = invocation.InvocationTarget;
                var stateAsync = stateManager.SetStateAsync(nameof(invocationInvocationTarget), invocationInvocationTarget);
//                stateAsync.ContinueWith()
            }

        }
    }

    public class ProxyFactory<T>
    {
        public T For<T>(T t, IActorStateManager stateManager) where T : class
        {
            var classProxyWithTarget = new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(t, new StateChangeInterceptor(stateManager));
            return classProxyWithTarget;
        }
    }
}