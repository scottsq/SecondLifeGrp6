using System;
using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class MessageFactory
    {
        public static Message MinCreationDateMessage = new Message();
        public static Message User1ToUser2Message = new Message();
        public static Message User1ToUser3Message = new Message();
        public static Message User2ToUser3Message = new Message();

        // Generating errors --------------------
        public static Message NoContentMessage = new Message();
        public static Message UnknownSenderMessage = new Message();
        public static Message UnknownReceiptMessage = new Message();

        public static void InitFactory()
        {
            UserFactory.InitFactory();

            var list = All();
            list.Add(MinCreationDateMessage);
            for (int i=0; i<list.Count; i++)
            {
                var m = list[i];
                m.Id = i;
                m.Content = "Fake Content";
                m.CreationDate = DateTime.Now.AddDays(i);
                m.Sender = UserFactory.GenericUser1;
                m.Receipt = UserFactory.GenericUser2;
            }
            User1ToUser3Message.Receipt = User2ToUser3Message.Receipt = UserFactory.GenericUser3;
            User2ToUser3Message.Sender = UserFactory.GenericUser2;
            MinCreationDateMessage.CreationDate = DateTime.MinValue;
            NoContentMessage.Content = String.Empty;
            UnknownReceiptMessage.Receipt = UnknownSenderMessage.Sender = UserFactory.UnknownUser;
        }

        public static List<Message> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }

        public static List<Message> GenericList()
        {
            return new List<Message>
            {
                User1ToUser2Message,
                User1ToUser3Message,
                User2ToUser3Message
            };
        }
        
        public static List<Message> ErrorList()
        {
            return new List<Message>
            {
                NoContentMessage,
                UnknownSenderMessage,
                UnknownReceiptMessage
            };
        }
    }
}
