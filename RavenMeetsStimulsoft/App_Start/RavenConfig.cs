using Raven.Abstractions.Smuggler;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Smuggler;

namespace RavenMeetsStimulsoft
{
    public static class RavenConfig
    {
        public static void ConfigureRaven(MvcApplication application)
        {
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = @"~\App_Data\Database",
                UseEmbeddedHttpServer = true // just for debugging purposes in this app
            };

            MvcApplication.DocumentStore = store.Initialize();
            IndexCreation.CreateIndexes(typeof(RavenConfig).Assembly, store);

            var statistics = store.DatabaseCommands.GetStatistics();

            if (statistics.CountOfDocuments < 5)
                ImportData(application.Server.MapPath("~/App_Data/Northwind.dump"), store);
        }

        private static void ImportData(string path, EmbeddableDocumentStore store)
        {
            var options = new SmugglerOptions { BackupPath = path };
            var dumper = new DataDumper(store.DocumentDatabase, options);
            dumper.ImportData(options).Wait();
        }
    }
}