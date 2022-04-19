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
            //kernel.Bind<ICriteriaManager>().To<CriteriaManager>();
            //kernel.Bind<IFacetManager>().To<FacetManager>();
            //kernel.Bind<IFacetTypeManager>().To<FacetTypeManager>();
            kernel.Bind<IRubricManager<Rubric>>().To<RubricManager>();
            //kernel.Bind<IRubricManager<RubricVM>>().To<RubricVMManager>();
            //kernel.Bind<IRubricSubjectManager>().To<RubricSubjectManager>();
            //kernel.Bind<IScoreTypeManager>().To<ScoreTypeManager>();
            //kernel.Bind<ISubjectManager>().To<SubjectManager>();
            kernel.Bind<IUserManager>().To<UserManager>();

            //kernel.Bind<ICriteriaAccessor>().To<CriteriaAccessorFake>();
            //kernel.Bind<IFacetAccesor>().To<FacetAccessorFake>();
            //kernel.Bind<IFacetTypeAccessor>().To<FacetTypeFakes>();
            //kernel.Bind<IRubricAccessor>().To<RubricAccessorFake>();
            //kernel.Bind<IRubricSubjectAccessor>().To<RubricSubjectAccessorFake>();
            //kernel.Bind<IScoreTypeAccessor>().To<ScoreTypeFake>();
            //kernel.Bind<ISubjectAccessor>().To<SubjectAccessorFake>();
            //kernel.Bind<IUserAccessor>().To<UserAccessorFake>();

            

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