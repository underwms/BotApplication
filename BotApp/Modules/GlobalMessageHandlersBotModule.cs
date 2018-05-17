using Autofac;
using BotApp.Dialogs;
using BotApp.Scorables;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables;
using Microsoft.Bot.Connector;

namespace BotApp.Modules
{
    public class GlobalMessageHandlersBotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Root
            builder.RegisterType<RootDialog>()
                .As<IDialog<object>>()
                .InstancePerDependency();

            // Global Commands
            builder
                .RegisterType<HelpScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ViewBranchScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ViewLocationsScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ViewActivitiesScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<StartOverScorable>()
                .As<IScorable<IActivity, double>>()
                .InstancePerLifetimeScope();

            // Dialogs
            builder.RegisterType<ViewActivitiesDialog>()
               .InstancePerDependency();

            builder.RegisterType<ViewBranchesDialog>()
               .InstancePerDependency();

            builder.RegisterType<ViewLocationsDialog>()
               .InstancePerDependency();

            builder.RegisterType<ViewEventsDialog>()
               .InstancePerDependency();

            builder.RegisterType<HelpDialog>()
                .InstancePerDependency();

            builder.RegisterType<EventRootDialog>()
               .InstancePerDependency();

            // Interfaces
            builder.RegisterType<DialogFactory>()
                .Keyed<IDialogFactory>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}