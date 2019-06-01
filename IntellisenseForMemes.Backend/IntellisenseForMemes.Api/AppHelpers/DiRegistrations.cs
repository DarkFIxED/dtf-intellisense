using System;
using System.Linq;
using System.Reflection;
using IntellisenseForMemes.BusinessLogic.Senders;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender;
using IntellisenseForMemes.BusinessLogic.Services.Meme;
using IntellisenseForMemes.BusinessLogic.Services.User;
using IntellisenseForMemes.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace IntellisenseForMemes.Api.AppHelpers
{
    public static class DiRegistrations
    {
        public static void Register(IApplicationBuilder app, Container container)
        {
            RegisterServices(container);
            RegisterSenders(container);
            RegisterCommon(app, container);
        }

        private static void RegisterServices(Container container)
        {
            container.Register<IUserService, UserService>();
            container.Register<IMemeService, MemeService>();
        }

        private static void RegisterSenders(Container container)
        {
            container.Register<IEmailSender, EmailSender>();
            container.Register<IDtfSender, DtfSender>();
        }


        private static void RegisterCommon(IApplicationBuilder app, Container container)
        {
            container.Register(typeof(IRepository<>), typeof(BaseRepository<>), Lifestyle.Scoped);
            container.Register(typeof(IRepository<,>), typeof(BaseRepository<,>), Lifestyle.Scoped);
            container.CrossWire<IntellisenseDbContext>(app);

            container.RegisterConditional(
                typeof(ILogger),
                c => typeof(Logger<>).MakeGenericType(c.Consumer.ImplementationType),
                Lifestyle.Singleton,
                c => true);

            // This next call is not required if you are already calling AutoCrossWireAspNetComponents
            container.CrossWire<ILoggerFactory>(app);

            container.CrossWire<UserManager<Domain.User>>(app); 
            container.CrossWire<SignInManager<Domain.User>>(app);
        }
    }
}
