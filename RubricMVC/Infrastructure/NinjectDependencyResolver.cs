using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using LogicLayer;
using DataAccessFakes;
using DataAccessInterFaces;
using DataObjects;

namespace RubricMVC.Infrastructure
{
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            //default
            kernel.Bind<ICriteriaManager>().To<CriteriaManager>();
            kernel.Bind<IFacetManager>().To<FacetManager>();
            kernel.Bind<IFacetTypeManager>().To<FacetTypeManager>();
            kernel.Bind<IRubricManager<Rubric>>().To<RubricManager>();
            kernel.Bind<IRubricManager<RubricVM>>().To<RubricVMManager>();
            kernel.Bind<IRubricSubjectManager>().To<RubricSubjectManager>();
            kernel.Bind<IScoreTypeManager>().To<ScoreTypeManager>();
            kernel.Bind<ISubjectManager>().To<SubjectManager>();
            kernel.Bind<IUserManager>().To<UserManager>();

            //fakes
            //kernel.Bind<ICriteriaAccessor>().To<CriteriaAccessorFake>().WithConstructorArgument("criteriatAccessor", new CriteriaAccessorFake());
            //kernel.Bind<IFacetManager>().To<FacetManager>().WithConstructorArgument("facetAccessor", new FacetAccessorFake());
            //kernel.Bind<IFacetTypeAccessor>().To<FacetTypeFakes>().WithConstructorArgument("facetTypeAccessor", new FacetTypeAccessorFake());
            //kernel.Bind<IRubricAccessor>().To<RubricAccessorFake>().WithConstructorArgument("rubricAccessor", new RubricAccessorFake());
            //kernel.Bind<IRubricSubjectAccessor>().To<RubricSubjectAccessorFake>().WithConstructorArgument("rubricSubjectAccessor", new RubricSubjectAccessorFake());
            //kernel.Bind<IScoreTypeAccessor>().To<ScoreTypeFake>().WithConstructorArgument("scoreTypeFake", new ScoreTypeFake());
            //kernel.Bind<ISubjectAccessor>().To<SubjectAccessorFake>().WithConstructorArgument("subjectAccessor", new SubjectAccessorFake());
            //kernel.Bind<IUserAccessor>().To<UserAccessorFake>().WithConstructorArgument("userAccessor", new UserAccessorFake());

        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

    }
}