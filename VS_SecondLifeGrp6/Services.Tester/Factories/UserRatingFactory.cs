using System;
using System.Collections.Generic;
using VS_SLG6.Model.Entities;

namespace Services.Tester.Factories
{
    public static class UserRatingFactory
    {
        public static UserRating FiveStarsOrigin1Target2UserRating;
        public static UserRating ThreeStarsOrigin1Target3UserRating;
        public static UserRating TwoStarsOrigin2Target3UserRating;

        // Generating errors --------------------
        public static UserRating NegativeStarsUserRating;
        public static UserRating SixStarsUserRating;
        public static UserRating UnknownOriginUserRating;
        public static UserRating UnknownTargetUserRating;
        public static UserRating Origin1Target1UserRating;

        public static void InitFactory()
        {
            UserFactory.InitFactory();

            var list = new List<UserRating> { FiveStarsOrigin1Target2UserRating, ThreeStarsOrigin1Target3UserRating, TwoStarsOrigin2Target3UserRating, NegativeStarsUserRating, SixStarsUserRating, UnknownOriginUserRating, UnknownTargetUserRating, Origin1Target1UserRating };
            for (int i=0; i<list.Count; i++)
            {
                var ur = list[i];
                ur.Id = i;
                ur.Origin = UserFactory.GenericUser1;
                ur.Target = UserFactory.GenericUser3;
                ur.Comment = String.Empty;
                ur.Stars = 5;
            }
            FiveStarsOrigin1Target2UserRating.Target = TwoStarsOrigin2Target3UserRating.Origin = UserFactory.GenericUser2;
            ThreeStarsOrigin1Target3UserRating.Stars = 3;
            TwoStarsOrigin2Target3UserRating.Stars = 2;
            NegativeStarsUserRating.Stars = -1;
            SixStarsUserRating.Stars = 6;
            UnknownOriginUserRating.Origin = UnknownTargetUserRating.Target = UserFactory.UnknownUser;
            Origin1Target1UserRating.Target = UserFactory.GenericUser1;
        }
    }
}
