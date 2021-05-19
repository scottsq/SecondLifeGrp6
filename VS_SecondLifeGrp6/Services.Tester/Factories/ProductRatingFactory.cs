using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProductRatingFactory
    {
        public static ProductRating FiveStarsProduct1User1Rating = new ProductRating();
        public static ProductRating ThreeStarsProduct1User2Rating = new ProductRating();
        public static ProductRating TwoStarsProduct2User1Rating = new ProductRating();

        // Generating errors --------------------
        public static ProductRating UnknownProductProductRating = new ProductRating();
        public static ProductRating NegativeStarsProductRating = new ProductRating();
        public static ProductRating SixStarsProductRating = new ProductRating();
        public static ProductRating UnknownUserProductRating = new ProductRating();

        public static void InitFactory()
        {
            ProductFactory.InitFactory();

            var list = List();
            for (int i=0; i<list.Count; i++)
            {
                var pr = list[i];
                pr.Id = i;
                pr.Comment = String.Empty;
                pr.Product = ProductFactory.GenericProduct1;
                pr.Stars = 5;
                pr.User = UserFactory.GenericUser1;
            }
            UnknownProductProductRating.Product = ProductFactory.UnknownProduct;
            TwoStarsProduct2User1Rating.Product = ProductFactory.GenericProduct2;
            NegativeStarsProductRating.Stars = -1;
            SixStarsProductRating.Stars = 6;
            ThreeStarsProduct1User2Rating.Stars = 3;
            FiveStarsProduct1User1Rating.Stars = 5;
            TwoStarsProduct2User1Rating.Stars = 2;
            UnknownUserProductRating.User = UserFactory.UnknownUser;
            ThreeStarsProduct1User2Rating.User = UserFactory.GenericUser2;
        }

        public static List<ProductRating> List()
        {
            return new List<ProductRating> { 
                FiveStarsProduct1User1Rating,
                ThreeStarsProduct1User2Rating, 
                TwoStarsProduct2User1Rating, 
                UnknownProductProductRating, 
                NegativeStarsProductRating, 
                SixStarsProductRating, 
                UnknownUserProductRating 
            };
        }
    }
}
