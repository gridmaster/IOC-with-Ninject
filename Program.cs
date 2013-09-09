using System;
using System.Diagnostics;
using Ninject;
using Ninject.Modules;

namespace IOCNinject
{
    class Program
    {
        public interface IGuitar
        {
            void Play();
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

        public class BaseService
        {
            private ILogger Log;

            public BaseService(ILogger log)
            {
                this.Log = log;
            }
        }

        public interface ILogger
        {
            void WriteLog(string message);
        }

        public class Log : ILogger
        {
            public void WriteLog(string message)
            {
                Trace.WriteLine("Logging message: " + message);
            }
        }

        public class GuitarService : BaseService
        {
            private readonly IGuitar _guitar;

            public GuitarService(IGuitar guitar, ILogger logger)
                : base(logger)
            {
                this._guitar = guitar;
                logger.WriteLog("We're playing our " + _guitar.ToString());
            }

            public void Play()
            {
                _guitar.Play();
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
                Trace.WriteLine("I'm just fakin' it: " + message);
            }
        }

        public class BindModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IGuitar>().To<Fender>();
                Bind<ILogger>().To<Log>();
            }
        }

        public class MockModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IGuitar>().To<MockGuitar>();
                Bind<ILogger>().To<MockLogger>();
            }
        }

        private static void InitializeDiContainer()
        {
            NinjectSettings settings = new NinjectSettings
            {
                LoadExtensions = false
            };

            IOCContainer.Instance.Initialize(settings, new MockModule());
        }

        static void Main(string[] args)
        {
            var gserve = new GuitarService(new Gibson(), new Log());
            gserve.Play();

            InitializeDiContainer();

            IOCContainer.Instance.Get<GuitarService>().Play();

            Console.ReadLine();
        }
    }
}