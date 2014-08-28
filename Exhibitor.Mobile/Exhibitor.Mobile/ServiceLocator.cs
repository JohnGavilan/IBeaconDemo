using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibitor.Mobile
{
    public class ServiceLocator
    {
        static public ServiceLocator Current
        {
            get { return ServiceLocator._current; }
        }
        static private ServiceLocator _current = new ServiceLocator();

        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        private ServiceLocator() { }

        public void SetService<T>(T service)
        {
            this._services.Add(typeof(T), service);
        }

        public T GetService<T>()
        {
            return (T)this._services[typeof(T)];
        }
    }
}
