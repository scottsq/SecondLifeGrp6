using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class MessageValidator : GenericValidator<Message>, IValidator<Message>
    {
        public MessageValidator(IRepository<Message> repo, ValidationModel<bool> validationModel) : base(repo, validationModel) { }
    }
}
