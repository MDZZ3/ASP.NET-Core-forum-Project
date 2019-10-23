using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class EvenHandlerContainer
    {
        private readonly IServiceProvider _provider;

        private static Dictionary<string, List<Type>> _mapping = new Dictionary<string, List<Type>>();

        public EvenHandlerContainer(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">IEvenHandler泛型类型</typeparam>
        /// <typeparam name="THandler">实现了IEventHandler的类</typeparam>
        public static void Subscribe<T,THandler>(string EvenKey)where T:EvenBase where THandler : IEvenHandler<T>
        {
         
            if (!_mapping.ContainsKey(EvenKey))
            {
                _mapping.Add(EvenKey, new List<Type>());
            }

            _mapping[EvenKey].Add(typeof(THandler));
        }

        public static void UnSubscribe<T,THandler>(string EvenKey) where T: EvenBase where THandler : IEvenHandler<T>
        {
            
            if (_mapping.ContainsKey(EvenKey))
            {
                _mapping[EvenKey].Remove(typeof(THandler));
            }

            if (_mapping.ContainsKey(EvenKey) && _mapping[EvenKey].Count == 0)
            {
                _mapping.Remove(EvenKey);
            }
        }

        public void Publish<T>(string EvenKey,T value) where T:EvenBase
        {
            if (_mapping.ContainsKey(EvenKey))
            {
                foreach (var handler in _mapping[EvenKey])
                {
                    var service = (IEvenHandler<T>)_provider.GetService(handler);

                    service.Run(value);
                }
            }
        }

       
    }
}
