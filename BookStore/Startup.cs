using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BookStore.Startup))]

namespace BookStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(); //add this line for signalR 13/10/2018
            ConfigureAuth(app); //omit this line 13/10/2018
            
        }
    }
}
