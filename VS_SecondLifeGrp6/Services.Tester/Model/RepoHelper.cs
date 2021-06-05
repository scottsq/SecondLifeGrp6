using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VS_SLG6.Repositories.Repositories;

namespace Services.Tester.Model
{
    public static class RepoHelper<T> where T: class
    {
        public static ISetup<IRepository<X>, List<X>> GenerateSetupAll<X>(Mock<IRepository<X>> mockedRepo) where X : class
        {
            return mockedRepo.Setup(x => x.All(It.IsAny<Expression<Func<X, bool>>>(), It.IsAny<Func<X, object>>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        public static void GenerateSetupFindOne<X>(List<T> list, Mock<IRepository<X>> mockedRepo, string prop) where X : class
        {
            if (prop == null) return;
            mockedRepo.Setup(x => x.FindOne(It.IsAny<object[]>()))
                .Returns<object[]>(x =>
                {
                    var property = typeof(T).GetProperty(prop);
                    var subObjectIdProperty = typeof(X).GetProperty("Id");
                    var obj = list.FirstOrDefault(o => Convert.ToInt32(subObjectIdProperty.GetValue(property.GetValue(o))) == (int)x[0]);
                    if (obj == null) return null;
                    return (X)property.GetValue(obj);
                });
        }

        public static Mock<IRepository<X>> GetMockedSubRepo<X>(List<T> list, string prop = null, List<X> listAll = null) where X : class
        {
            var mockedRepo = new Mock<IRepository<X>>();
            GenerateSetupFindOne(list, mockedRepo, prop);
            if (listAll != null) GenerateSetupAll(mockedRepo).Returns(() => listAll);
            return mockedRepo;
        }
    }
}
