namespace API.Jobs
{
    using System;
    using Hangfire;

    public class ContainerJobActivator : JobActivator
    {
        private readonly IServiceProvider serviceProvider;

        public ContainerJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return this.serviceProvider.GetService(type);
        }
    }
}
