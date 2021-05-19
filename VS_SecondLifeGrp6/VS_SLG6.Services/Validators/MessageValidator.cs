using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class MessageValidator : GenericValidator<Message>, IValidator<Message>
    {
        private IRepository<User> _repoUser;

        public MessageValidator(IRepository<Message> repo, IRepository<User> repoUser) : base(repo) 
        {
            _repoUser = repoUser;
        }

        public override List<string> CanAdd(Message obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            // check if message exist for same receipt at same datetime
            IsObjectExisting(listErrors, x => x.CreationDate == obj.CreationDate && x.Receipt.Id == obj.Receipt.Id);
            return listErrors;
        }

        public override List<string> CanEdit(Message obj)
        {
            return IsObjectValid(obj);
        }

        public override List<string> CanDelete(Message obj)
        {
            return IsObjectValid(obj);
        }

        public override List<string> IsObjectValid(Message obj, ConstraintsObject constraintsObject = null)
        {
            // Build helper
            var listProps = new List<string> { nameof(Message.Content), nameof(Message.Receipt), nameof(Message.Sender) };
            constraintsObject = new ConstraintsObject()
            {
                FieldsNotNull = listProps,
                FieldsStringNotBlank = listProps.Where(x => x == nameof(obj.Content)).ToList()
            };

            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // check if Sender and Receipt exist
            var sender = _repoUser.FindOne(obj.Sender.Id);
            var receipt = _repoUser.FindOne(obj.Receipt.Id);
            if (sender == null || receipt == null) listErrors.Add("Cannot send message with invalid User(s).");
            else
            {
                obj.Sender = sender;
                obj.Receipt = receipt;
            }

            // Check if Sender and Receipt are identical
            if (obj.Sender.Id == obj.Receipt.Id) listErrors.Add("Message Sender and Receipt cannot be indentical.");

            // check time / not an error but more a formatting task
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;

            return listErrors;
        }
    }
}
