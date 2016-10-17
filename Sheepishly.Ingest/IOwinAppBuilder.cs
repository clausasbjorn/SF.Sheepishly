using Owin;

namespace Sheepishly.Ingest
{
    public interface IOwinAppBuilder
    {
        void Configuration(IAppBuilder appBuilder);
    }
}