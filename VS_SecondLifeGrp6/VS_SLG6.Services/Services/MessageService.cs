﻿using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Interfaces;
using VS_SLG6.Services.Validators;

namespace VS_SLG6.Services.Services
{
    public class MessageService : GenericService<Message>, IMessageService
    {
        public MessageService(IRepository<Message> repo, IValidator<Message> validator) : base(repo, validator)
        {
        }

        public List<Message> Find(int id = -1, int idOrigin = -1, int idDest = -1, bool twoWays = false, string orderBy = null, bool reverse = false, int from = 0, int max = 10)
        {
            var list = _repo.All(
                GenerateCondition(id, idOrigin, idDest, twoWays), 
                GenerateOrderByCondition(orderBy),
                reverse, from, max
            );
            list = list.Distinct(new ConversationComparer()).ToList();
            return list;
        }

        public static Expression<Func<Message, bool>> GenerateCondition(int id = -1, int idOrigin = -1, int idDest = -1, bool twoWays = false)
        {
            Expression<Func<Message, bool>> condition = x => true;
            if (id > -1) condition.And(x => x.Id == id);
            if (idOrigin > -1) condition.And(x => x.Sender.Id == idOrigin || (twoWays ? x.Receipt.Id == idOrigin : false));
            if (idDest > -1) condition.And(x => x.Receipt.Id == idDest || (twoWays ? x.Sender.Id == idDest : false));
            return condition;
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
