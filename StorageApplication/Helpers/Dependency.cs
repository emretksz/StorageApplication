using IoC;
using System.Linq.Expressions;

namespace StorageApplication.Helpers
{
    public class Dependency : IDependency
    {
        public bool TryBuildExpression([NotNull] IBuildContext buildContext, [CanBeNull] ILifetime lifetime, out Expression expression, out Exception error)
        {
            throw new NotImplementedException();
        }
    }
}
