using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Services
{
    public interface IMessageService : IService<Message>
    {
        public List<Message> GetConversation(int idOrigin, int idDest);
        public List<Message> GetConversations(int idOrigin);
    }
}
