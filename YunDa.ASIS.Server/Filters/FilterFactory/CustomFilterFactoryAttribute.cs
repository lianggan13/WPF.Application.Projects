using Microsoft.AspNetCore.Mvc.Filters;


namespace YunDa.ASIS.Server.Filters
{
    /// <summary>
    /// Filter Attribute Creator (ref class: TypeFilterAttribute)
    /// </summary>
    public class CustomFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public object[]? Arguments { get; set; }

        public Type ImplementationType { get; }

        public CustomFilterFactoryAttribute(Type type)
        {
            this.ImplementationType = type;
        }
        private ObjectFactory _factory;

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            IFilterMetadata filterMetadata = (IFilterMetadata)serviceProvider.GetService(ImplementationType);
            if (filterMetadata == null)
            {
                if (serviceProvider == null)
                {
                    throw new ArgumentNullException("serviceProvider");
                }
                if (_factory == null)
                {
                    Type[] array = Arguments?.Select((object a) => a.GetType())?.ToArray();
                    _factory = ActivatorUtilities.CreateFactory(ImplementationType, array ?? Type.EmptyTypes);
                }
                filterMetadata = (IFilterMetadata)_factory(serviceProvider, Arguments);
                IFilterFactory filterFactory = filterMetadata as IFilterFactory;
                if (filterFactory != null)
                {
                    filterMetadata = filterFactory.CreateInstance(serviceProvider);
                }
            }

            return filterMetadata;
        }
    }
}
