// -----------------------------------------------------------------------
// <copyright file="IOCContainer.cs" company="FF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Ninject;
using Ninject.Modules;

namespace IOC_with_Ninject
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IOCContainer
    {
        private IKernel kernel = null;
        private static IOCContainer instance = null;

        public static IOCContainer Instance
        {
            get { return instance ?? (instance = new IOCContainer()); }
        }

        public bool IsInitialized { get { return (kernel != null); } }

        public void Initialize(NinjectSettings settings, params INinjectModule[] modules)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("The DI Container is already initialized.");
            }
            kernel = new StandardKernel(settings, modules);
        }

        public T Get<T>()
        {
            VerifyInitialization();

            return kernel.Get<T>();
        }

        public object Get(Type type)
        {
            VerifyInitialization();

            return kernel.Get(type);
        }

        private void VerifyInitialization()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("The DI Container is not initialized");
            }
        }
    }
}
