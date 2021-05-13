﻿using VS_SLG6.Model.Entities;

namespace VS_SLG6.Api.ControllerAccess
{
    public class TagControllerAccess : ControllerAccess<Tag>
    {
        public override bool CanAdd(ContextUser ctxUser, Tag obj)
        {
            return HasRole(Roles.USER, ctxUser);
        }
        public override bool CanDelete(ContextUser ctxUser, Tag obj)
        {
            return HasRole(Roles.ADMIN, ctxUser);
        }
        public override bool CanEdit(ContextUser ctxUser, Tag obj)
        {
            return CanDelete(ctxUser, obj);
        }
    }
}
