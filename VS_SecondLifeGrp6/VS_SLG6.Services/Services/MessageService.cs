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

        public List<Message> GetConversation(int idOrigin, int idDest)
        {
            return _repo.FindAll(x => x.Sender.Id == idOrigin && x.Receipt.Id == idDest);
        }

        public List<Message> GetConversations(int idOrigin)
        {            
            var list = _repo.FindAll(x => x.Sender.Id == idOrigin);
            Func<Message, List<Message>, bool> contains = (Message item, List<Message> list) => {
                return list.Find(x => x.Receipt.Id == item.Receipt.Id) != null;
            };
            List<Message> filteredList = list.Aggregate(new List<Message>(), (acc, item) =>
            {
                if (!contains(item, acc)) acc.Add(LastMessage(item.Sender.Id, item.Receipt.Id));
                return acc;
            });
            return filteredList;
        }

        public Message LastMessage(int idOrigin, int idDest)
        {
            var list = GetConversation(idOrigin, idDest);
            list.Sort((a, b) => a.CreationDate > b.CreationDate ? 1 : -1);
            return list.FirstOrDefault();
        }
    }
}
