using Autofac;
using BotApp.Caches;
using BotApp.Modules;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using System.Web.Http;

namespace BotApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LocationCache.InitCache();
            ActivityCahce.InitCache();
            EventCache.InitCache();

            RegisterBotModules();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void RegisterBotModules()
        {
            Conversation.UpdateContainer(builder =>
            {
                builder.RegisterModule(new ReflectionSurrogateModule());
                builder.RegisterModule<GlobalMessageHandlersBotModule>();
            });
        }
    }
}
