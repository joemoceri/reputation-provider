using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace MefReputationProvider
{
    public class Compositor
    {
        public CompositionContainer Configure(object parts, string pluginsSearchPattern)
        {
            var catalog = new AggregateCatalog();
            
            var reputationPlugins = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, pluginsSearchPattern);

            foreach (var plugin in reputationPlugins)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFile(plugin)));
            }

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(parts);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            return container;
        }
    }
}
