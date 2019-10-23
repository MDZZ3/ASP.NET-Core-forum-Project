using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;

namespace firstWeb.Domain.Services.contern
{
    public class ConternService : IConternService
    {
        private readonly IRepository<Contern> _contern;

        public ConternService(IRepository<Contern> contern)
        {
            _contern = contern;
        }

        public async Task CreateConternAsync(string ConternID, string fanID)
        {
            Contern contern = new Contern()
            {
                ID = $"{fanID}{ConternID}",
                ConternAccountID = ConternID,
                FansAccountID = fanID
            };
            await _contern.Table.AddAsync(contern);
            await _contern._db.SaveChangesAsync();
        }

        public Contern GetContern(string ConternID, string fanID)
        {
            return _contern.Table.FirstOrDefault(c => c.ConternAccountID == ConternID && c.FansAccountID == fanID);
        }

        public bool IsContern(string ConternID, string fanID)
        {
            var contern = GetContern(ConternID, fanID);

            return contern == null ? false : true;
        }

        public async Task RemoveConternAsync(string ConternID, string fanID)
        {
            
           Contern contern = GetContern(ConternID, fanID);
           _contern.Table.Remove(contern);
           await  _contern._db.SaveChangesAsync();
        }
    }
}
