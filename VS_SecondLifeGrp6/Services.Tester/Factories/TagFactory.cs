using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class TagFactory
    {
        public static Tag GenericTag1;
        public static Tag GenericTag2;

        // Generating errors --------------------
        public static Tag BlankNameTag;
        public static Tag UnknownTag;

        public static void InitFactory()
        {
            var list = new List<Tag> { GenericTag1, GenericTag2, BlankNameTag, UnknownTag };
            var props = typeof(Tag).GetProperties();
            for (int i=0; i<list.Count; i++)
            {
                var t = list[i];
                t.Id = i;
                t.Name = props[i].Name;
            }
            BlankNameTag.Name = String.Empty;
            UnknownTag.Id = -1;
        }
    }
}
