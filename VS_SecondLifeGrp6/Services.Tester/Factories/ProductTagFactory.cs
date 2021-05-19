using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProductTagFactory
    {
        public static ProductTag Product1Tag1ProductTag;
        public static ProductTag Product1Tag2ProductTag;
        public static ProductTag Product2Tag1ProductTag;

        // Generating errors --------------------
        public static ProductTag UnknownProductProductTag;
        public static ProductTag UnknownTagProductTag;

        public static void InitFactory()
        {
            TagFactory.InitFactory();
            ProductFactory.InitFactory();

            var list = new List<ProductTag> { Product1Tag1ProductTag, Product1Tag2ProductTag, Product2Tag1ProductTag, UnknownProductProductTag, UnknownTagProductTag };
            for (int i=0; i<list.Count; i++)
            {
                var pt = list[i];
                pt.Id = i;
                pt.Product = ProductFactory.GenericProduct1;
                pt.Tag = TagFactory.GenericTag1;
            }
            Product1Tag2ProductTag.Tag = TagFactory.GenericTag2;
            Product2Tag1ProductTag.Product = ProductFactory.GenericProduct2;
            UnknownProductProductTag.Product = ProductFactory.UnknownProduct;
            UnknownTagProductTag.Tag = TagFactory.UnknownTag;
        }
    }
}
