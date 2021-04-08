using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class MessageService : GenericService<Message>, IMessageService
    {
        public MessageService(IRepository<Message> repo, IValidator<Message> validator) : base(repo, validator)
        {
        }

        public ValidationModel<List<Message>> GetConversation(int idOrigin, int idDest)
        {
            var res = new ValidationModel<List<Message>>();
            var v = ((MessageValidator)_validator).CanGet(idOrigin, idDest);
            if (!v.Value)
            {
                res.Value = null;
                res.Errors = v.Errors;
            }
            else
            {
                res.Value = _repo.All(x => (
                    x.Sender.Id == idOrigin && x.Receipt.Id == idDest) 
                    || (x.Sender.Id == idDest && x.Receipt.Id == idOrigin)
                );
            }
            return res;
        }

        public ValidationModel<List<Message>> ListConversations(int idOrigin)
        {
            var res = new ValidationModel<List<Message>>();
            var v = ((MessageValidator)_validator).CanGet(idOrigin, -1);
            if (!v.Value)
            {
                res.Value = null;
                res.Errors = v.Errors;
            }
            else
            {
                res.Value = _repo.All(x => x.Sender.Id == idOrigin || x.Receipt.Id == idOrigin).Distinct(new ConversationComparer()).ToList();
            }
            return res;
            
            /*Func<Message, List<Message>, bool> contains = (Message item, List<Message> list) => {
                return list.Find(x => (x.Receipt.Id == item.Receipt.Id && x.Sender.Id == item.Sender.Id) || (x.Receipt.Id == item.Sender.Id && x.Sender.Id == item.Receipt.Id)) != null;
            };
            List<Message> filteredList = list.Aggregate(new List<Message>(), (acc, item) =>
            {
                if (!contains(item, acc)) acc.Add(LastMessage(item.Sender.Id, item.Receipt.Id));
                return acc;
            });
            return filteredList;*/
        }

        public ValidationModel<Message> LastMessage(int idOrigin, int idDest)
        {
            var list = GetConversation(idOrigin, idDest);
            if (list.Value == null) return _validationModel;
            else
            {
                list.Value.Sort((a, b) => a.CreationDate > b.CreationDate ? 1 : -1);
                _validationModel.Value = list.Value.FirstOrDefault();
            }
            return _validationModel;
        }
    }

    public class ConversationComparer : IEqualityComparer<Message>
    {
        public bool Equals(Message x, Message y)
        {
            return (x.Sender == y.Sender && x.Receipt == y.Receipt) || (x.Sender == y.Receipt && x.Receipt == y.Sender);
        }

        public int GetHashCode(Message obj)
        {
            return int.Parse(obj.Sender.Id.ToString() + obj.Receipt.Id.ToString());
        }
    }
}
