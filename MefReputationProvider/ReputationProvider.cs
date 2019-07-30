using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MefReputationProvider
{
    public class ReputationProvider : IDisposable
    {
        private CompositionContainer container;
        private Repository repository;

        [ImportMany(typeof(IReputationSite))]
        public IEnumerable<Lazy<IReputationSite>> reputations;

        public ReputationProvider() : this (new Repository()) { }

        public ReputationProvider(Repository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<ReputationDataModel> GetAll()
        {
            if(container == null)
            {
                container = new Compositor().Configure(this, "MefReputationProvider.*.dll");
            }

            var result = new List<ReputationDataModel>();
            foreach (var reputation in repository.GetReputationConfigurations())
            {
                result.Add(container.GetExport<IReputationSite>(reputation.Name).Value.Get(reputation.Id));
            }

            return result;
        }

        public void Dispose()
        {
            if(container != null)
                container.Dispose();
        }
    }
}
