using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class PhotoFactory
    {
        public static Photo Product1Photo = new Photo();
        public static Photo Product2Photo = new Photo();
        public static Photo Product3Photo = new Photo();

        // Generating errors --------------------
        public static Photo UnknownProductPhoto = new Photo();
        public static Photo BlankUrlPhoto = new Photo();

        public static void InitFactory()
        {
            ProductFactory.InitFactory();

            var list = All();
            var props = typeof(Photo).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            for (int i=0; i<list.Count; i++)
            {
                var p = list[i];
                p.Id = i;
                p.Product = ProductFactory.GenericProduct1;
                p.Url = "Fake URL";
            }
            Product2Photo.Product = ProductFactory.GenericProduct2;
            Product3Photo.Product = ProductFactory.GenericProduct3;
            UnknownProductPhoto.Product = ProductFactory.UnknownProduct;
            BlankUrlPhoto.Url = String.Empty;
        }

        public static List<Photo> GenericList()
        {
            return new List<Photo> { 
                Product1Photo, 
                Product2Photo, 
                Product3Photo
            };
        }

        public static List<Photo> ErrorList()
        {
            return new List<Photo>
            {
                UnknownProductPhoto,
                BlankUrlPhoto
            };
        }

        internal static List<Photo> All()
        {
            return GenericList().Concat(ErrorList()).ToList();
        }
    }
}
