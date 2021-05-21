using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class TagFactory
    {
        public static Tag GenericTag1 = new Tag();
        public static Tag GenericTag2 = new Tag();
        public static Tag GenericTag3 = new Tag();

        // Generating errors --------------------
        public static Tag BlankNameTag = new Tag();
        public static Tag UnknownTag = new Tag();

        public static void InitFactory()
        {
            var list = All();
            var props = typeof(TagFactory).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            for (int i=0; i<list.Count; i++)
            {
                var t = list[i];
                t.Id = i;
                t.Name = props[i].Name;
            }
            BlankNameTag.Name = String.Empty;
            UnknownTag.Id = -1;
        }

        public static List<Tag> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }

        public static List<Tag> GenericList()
        {
            return new List<Tag> {
                GenericTag1,
                GenericTag2,
                GenericTag3
            };
        }

        public static List<Tag> ErrorList()
        {
            return new List<Tag> {
                BlankNameTag
            };
        }
    }
}
