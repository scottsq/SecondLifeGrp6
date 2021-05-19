using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class ProductRatingFactory
    {
        public static ProductRating FiveStarsProduct1Rating;
        public static ProductRating ThreeStarsProduct1Rating;
        public static ProductRating TwoStarsProduct2Rating;

        // Generating errors --------------------
        public static ProductRating UnknownProductProductRating;
        public static ProductRating NegativeStarsProductRating;
        public static ProductRating SixStarsProductRating;
        public static ProductRating UnknownUserProductRating;

        public static void InitFactory()
        {
            ProductFactory.InitFactory();

            var list = new List<ProductRating> { FiveStarsProduct1Rating, ThreeStarsProduct1Rating, TwoStarsProduct2Rating, UnknownProductProductRating, NegativeStarsProductRating, SixStarsProductRating, UnknownUserProductRating };
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
            NegativeStarsProductRating.Stars = -1;
            SixStarsProductRating.Stars = 6;
            ThreeStarsProduct1Rating.Stars = 3;
            FiveStarsProduct1Rating.Stars = 5;
            TwoStarsProduct2Rating.Stars = 2;
            UnknownUserProductRating.User = UserFactory.UnknownUser;
        }
    }
}
