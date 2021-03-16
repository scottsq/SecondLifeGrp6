using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Services;

namespace VS_SLG6.Services.Validators
{
    public class MessageValidator : GenericValidator<Message>, IValidator<Message>
    {
        private IRepository<User> _repoUser;

        public MessageValidator(IRepository<Message> repo, ValidationModel<bool> validationModel, IRepository<User> repoUser) : base(repo, validationModel) 
        {
            _repoUser = repoUser;
        }

        public override ValidationModel<bool> CanAdd(Message obj)
        {
            _validationModel.Value = false;
            // Check null values
            if (obj == null)
            {
                _validationModel.Errors.Add("Cannot add null message.");
                return _validationModel;
            }
            if (obj.Content == null || obj.Receipt == null || obj.Sender == null)
            {
                _validationModel.Errors.Add("Message cannot have empty fields.");
                return _validationModel;
            }

            // check Content
            var check = StringIsEmptyOrBlank(obj, "Content");
            if (check.Errors.Count > 0) AppendFormattedErrors(check.Errors, "Message {0} cannot be empty.");

            // check if Sender and Receipt exist
            var sender = _repoUser.FindOne(obj.Sender.Id);
            var receipt = _repoUser.FindOne(obj.Receipt.Id);
            if (sender == null || receipt == null) _validationModel.Errors.Add("Cannot send message with invalid User(s).");

            // check time / not an error but more a formatting task
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;

            // check if message exist for same receipt at same datetime
            var m = _repo.FindAll(x => x.CreationDate == obj.CreationDate && x.Receipt.Id == obj.Receipt.Id);
            if (m.Count > 0) _validationModel.Errors.Add("Message already exists.");

            _validationModel.Value = _validationModel.Errors.Count == 0;
            return _validationModel;
        }
    }
}
