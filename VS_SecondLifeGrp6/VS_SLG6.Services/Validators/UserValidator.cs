using System.Collections.Generic;
using System.Linq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Models;

namespace VS_SLG6.Services.Validators
{
    public class UserValidator : GenericValidator<User>, IValidator<User>
    {
        

        public UserValidator(IRepository<User> repo): base(repo)
        {
        }

        public override List<string> CanAdd(User obj)
        {
            var listErrors = IsObjectValid(obj);
            if (listErrors.Any()) return listErrors;

            IsObjectExisting(listErrors, x => x.Login == obj.Login);
            obj.Password = StringHelper.GetStringSha256Hash(obj.Password);
            return listErrors;
        }

        public override List<string> IsObjectValid(User obj, ConstraintsObject constraintsObject = null)
        {
            var listProps = new List<string> { nameof(User.Login), nameof(User.Password), nameof(User.Email), nameof(User.Name) };
            constraintsObject = new ConstraintsObject
            {
                FieldsNotNull = listProps,
                FieldsStringNotBlank = listProps,
                FieldsStringNotLongerThanMax = listProps.Where(x => x != nameof(obj.Email)).ToList()
            };

            // Basic check on fields (null, blank, size)
            var listErrors = base.IsObjectValid(obj, constraintsObject);
            if (listErrors.Any()) return listErrors;

            // Check Email
            var splittedMail = obj.Email.Split('@');
            if (obj.Email.Contains("..") || splittedMail.Length < 2 || splittedMail[0].Trim().Length == 0 || splittedMail[1].Trim().Length == 0 || splittedMail[1].Trim().Split('.').Length < 2)
            {
                listErrors.Add("User Email is invalid.");
            }

            return listErrors;
        }
    }
}
