using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VS_SLG6.Model.Entities;
using VS_SLG6.Repositories.Repositories;
using VS_SLG6.Services.Validators;

namespace Validator.Tester
{
    [TestClass]

    public class UserValidatorTester 
    {
        private IValidator<User> _validator;
        readonly Mock<IRepository<User>> _repoMock;

        public UserValidatorTester()
        {
            _repoMock = new Mock<IRepository<User>>();
            _validator = new UserValidator(_repoMock.Object);
        }

        [TestMethod]
        public void CanAdd_With_Existing_Then_False()
        {
           // fait la recherche par login
            User user = new User();
           // user.Id = 1;
            user.Login = "toto";
            _repoMock.Setup(x => x.FindOne("toto")).Returns(user);
            bool res = _validator.canAdd(user);
            Assert.IsFalse(res);
        }
    }

}
