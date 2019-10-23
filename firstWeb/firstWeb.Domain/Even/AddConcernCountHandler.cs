using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class AddConcernCountHandler : IEvenHandler<ConcernSumbitEven>
    {
        private readonly IRepository<User> _userRepository;

        public AddConcernCountHandler(IRepository<User> user)
        {
            _userRepository = user;
        }

        public void Run(ConcernSumbitEven value)
        {
            var subscriber = _userRepository.Table.FirstOrDefault(u => u.ID == value.SubscriberId);

            var publisher = _userRepository.Table.FirstOrDefault(u => u.ID == value.publisherId);
            //关注方的关注人数+1
            publisher.Concern += 1;
            //被关注方的粉丝+=1
            subscriber.Fans += 1;
        }
    }
}
