using firstWeb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace firstWeb.Domain.Services.contern
{
    public interface IConternService
    {
        /// <summary>
        /// 通过关注人ID和被关注人ID来查找
        /// </summary>
        /// <param name="ConternID">被关注人ID</param>
        /// <param name="fanID">关注人ID</param>
        /// <returns></returns>
        Contern GetContern(string ConternID, string fanID);

        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <param name="ConternID">被关注人ID</param>
        /// <param name="fanID">关注人ID</param>
        /// <returns></returns>
        bool IsContern(string ConternID, string fanID);

        /// <summary>
        /// 添加一个关注
        /// </summary>
        /// <param name="ConternID">被关注人ID</param>
        /// <param name="fanID">关注人ID</param>
        Task CreateConternAsync(string ConternID, string fanID);

        /// <summary>
        /// 移除一个关注
        /// </summary>
        /// <param name="ConternID">被关注人ID</param>
        /// <param name="fanID">关注人ID</param>
        /// <returns></returns>
        Task RemoveConternAsync(string ConternID, string fanID);

    }
}
