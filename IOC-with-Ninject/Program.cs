using System;
using System.Diagnostics;
using Ninject;
using Ninject.Modules;

namespace IOC_with_Ninject
{
    internal class Program
    {
        #region contracts
        // these are the interfaces we are coding to and which
        // Ninject will use to bind implementations to.
        // they are deliberately simple to focus on the pattern
        public interface IGuitar
        {
            void Play();
        }

        public interface IGuitarService
        {
            void Play();
        }

        public interface ILogger
        {
            void WriteLog(string message);
        }
        #endregion contracts

        #region models
        // here are our Models. In the real world
        // they would likely have properties as well.
        public class Guitar : IGuitar
        {
            private string _name ;
            public Guitar(string name)
            {
                _name = name;
            }

            public void Play()
            {
                Console.WriteLine("I'm strummin' my {0}!", this.ToString());
            }

            public override string ToString()
            {
                return _name;
            }
        }

        public class Fender : IGuitar
        {
            public void Play()
            {
                Console.WriteLine("I'm strummin' my Fender!");
            }

            public override string ToString()
            {
                return "Fender";
            }
        }

        public class Gibson : IGuitar
        {
            public void Play()
            {
                Console.WriteLine("I'm strummin' my Gibson!");
            }

            public override string ToString()
            {
                return "Gibson";
            }
        }
        #endregion Guitars

        #region base classes
        // Only one to demonstrate the power of 
        // DI and IOC
        public class BaseService
        {
            public ILogger logger;

            public BaseService(ILogger log)
            {
                this.logger = log;
            }
        }
        #endregion base classes

        #region logging
        // simple but necessary for any serious code.
        public class Log : ILogger
        {
            public void WriteLog(string message)
            {
                Console.WriteLine("\n" + DateTime.Now + ": " + message);
            }
        }
        #endregion Logging

        #region services
        // again only one, but serves our purposes
        public class GuitarService : BaseService, IGuitarService
        {
            private readonly IGuitar _guitar;

            public GuitarService(IGuitar guitar, ILogger logger)
                : base(logger)
            {
                this._guitar = guitar;
                logger.WriteLog("Creating a GuitarService");
            }

            public void Play()
            {
                _guitar.Play();
                logger.WriteLog("Logging that we played a " + _guitar.ToString());
            }
        }
        #endregion services

        #region mocks
        // Mocks here - these are used to demonstarte how you can test or 
        // completely refactor your code with a simple change
        public class MockGuitarService : BaseService, IGuitarService
        {
            private readonly IGuitar _guitar;

            public MockGuitarService(IGuitar guitar, ILogger logger)
                : base(logger)
            {
                this._guitar = guitar;
                logger.WriteLog("Creating a MockGuitarService");
            }

            public void Play()
            {
                _guitar.Play();
                logger.WriteLog("Logging that we played an air " + _guitar.ToString());
            }
        }

        public class MockGuitar : IGuitar
        {
            public void Play()
            {
                Console.WriteLine("I'm playing air guitar");
            }

            public override string ToString()
            {
                return "Air Guitar";
            }
        }

        public class MockLogger : ILogger
        {
            public void WriteLog(string message)
            {
                Trace.WriteLine(DateTime.Now + ": " + message);
            }
        }
        #endregion Mocks

        #region bind modules for ninject
        // Swap between these to modules to refactor code.
        // NOTE: both must implement NinjectModule and 
        //  override the Load method
        public class BindModule : NinjectModule
        {
            public override void Load()
            {
                Bind<ILogger>().To<Log>();
                Bind<IGuitar>().To<Guitar>().WithConstructorArgument("name", "Ding-Dong");
                Bind<IGuitarService>().To<GuitarService>();
            }
        }

        public class MockModule : NinjectModule
        {
            public override void Load()
            {
                Bind<ILogger>().To<MockLogger>();
                Bind<IGuitar>().To<MockGuitar>();
                Bind<IGuitarService>().To<MockGuitarService>();
            }
        }
        #endregion Bind Modules for Ninject

        #region initialize DI container
        // This initializes the IOC Container and implements
        // the singleton pattern.
        private static void InitializeDiContainer()
        {
            NinjectSettings settings = new NinjectSettings
                {
                    LoadExtensions = false
                };

            // change this to BindModule to run other implementation
            IOCContainer.Instance.Initialize(settings, new BindModule());
        }
        #endregion Initialize DI Container

        // the "get 'er done" module...
        // The 'Old way' has 3 'new' statements. Yikes.
        // The 'New way' has none. Sweet!
        private static void Main(string[] args)
        {
            Console.WriteLine("Old way...");
            var gserve = new GuitarService(new Guitar("Gibson"), new Log());
            gserve.Play();

            var gservice = new GuitarService(new Guitar("Martin"), new Log());
            gservice.Play();


            Console.WriteLine("\nNew way...");
            InitializeDiContainer();

            IOCContainer.Instance.Get<IGuitarService>().Play();

            Console.ReadLine();
        }
    }
}
