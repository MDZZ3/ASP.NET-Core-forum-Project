using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class RemoveConcernHandler : IEvenHandler<RemoveConcernSubmitEven>
    {
        private readonly IRepository<User> _userRepository;

        public RemoveConcernHandler(IRepository<User> user)
        {
            _userRepository = user;
        }

        public void Run(RemoveConcernSubmitEven value)
        {
            var subscriber = _userRepository.Table.FirstOrDefault(u => u.ID == value.SubscriberId);

            var publisher = _userRepository.Table.FirstOrDefault(u => u.ID == value.publisherId);

            subscriber.Fans -= 1;

            publisher.Concern -= 1;

            _userRepository._db.SaveChanges();
        }
    }
}
