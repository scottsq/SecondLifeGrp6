using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class UserRatingValidator : GenericValidator<UserRating>, IValidator<UserRating>
    {
        private IRepository<User> _repoUser;

        public UserRatingValidator(IRepository<UserRating> repo, IRepository<User> repoUser) : base(repo) 
        {
            _repoUser = repoUser;
        }

        public override List<string> CanAdd(UserRating obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            IsObjectExisting(listErrors, x => x.Origin.Id == obj.Origin.Id && x.Target.Id == obj.Target.Id);
            return listErrors;
        }

        public override List<string> IsObjectValid(UserRating obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(UserRating.Origin), nameof(UserRating.Target) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps
            };
            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check stars between 1 and 5
            if (obj.Stars < 1 || obj.Stars > 5) listErrors.Add("Rating Stars must be between 1 and 5.");

            // Check if Origin exists
            var o = _repoUser.FindOne(obj.Origin.Id);
            if (o == null) listErrors.Add("Rating Origin doesn't exist.");
            else obj.Origin = o;

            // Check if Product exists
            var t = _repoUser.FindOne(obj.Target.Id);
            if (t == null) listErrors.Add("Rating Target doesn't exist.");
            else obj.Target = t;

            // Format Comment (can be optional that's why we don't give it to parent function)
            if (obj.Comment != null && StringHelper.StringIsEmptyOrBlank(obj, "Comment").Value) obj.Comment = null;
            else if (obj.Comment != null) obj.Comment = obj.Comment.Trim();

            return listErrors;
        }
    }
}
