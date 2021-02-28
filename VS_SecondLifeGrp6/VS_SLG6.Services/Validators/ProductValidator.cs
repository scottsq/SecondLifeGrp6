using System;
using System.Collections.Generic;
using System.Text;
using VS_SLG6.Model.Entities;

namespace VS_SLG6.Services.Validators
{
    public class ProductValidator : IValidator<Product>
    {
        public bool canAdd(Product obj)
        {
            if (obj.Name.Trim() == String.Empty) return false;
            if (obj.Price < 0) return false;
            if (obj.CreationDate == DateTime.MinValue) obj.CreationDate = DateTime.Now;
            return true;
        }

        public bool canDelete(Product obj)
        {
            return true;
        }

        public bool canEdit(Product obj)
        {
            return true;
        }
    }
}
