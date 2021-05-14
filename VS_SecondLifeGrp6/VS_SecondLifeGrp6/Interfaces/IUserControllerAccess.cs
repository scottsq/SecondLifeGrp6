using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS_SLG6.Api.ControllerAccess;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.Interfaces
{
    public interface IUserControllerAccess : IControllerAccess<User>
    {
        public bool CanGet(ContextUser ctxUser, int id);
    }
}
