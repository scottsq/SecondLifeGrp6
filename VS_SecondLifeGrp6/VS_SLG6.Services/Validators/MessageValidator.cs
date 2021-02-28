using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Validators
{
    public class MessageValidator : IValidator<Message>
    {
        public bool canAdd(Message obj)
        {
            if (obj.Content.Trim() == String.Empty) return false;
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;
            return true;
        }

        public bool canDelete(Message obj)
        {
            return true;
        }

        public bool canEdit(Message obj)
        {
            return true;
        }
    }
}
