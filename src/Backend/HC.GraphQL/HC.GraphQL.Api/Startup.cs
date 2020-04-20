using System;
using HotChocolate;
using HotChocolate.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HotChocolate.Server;
using static HC.GraphQL.Api.RootTypes;

namespace HC.GraphQL.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.SetIsOriginAllowed(_ => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials())
                        );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("WebSockets", ctx => { })
            .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5001"; //TODO: Make configurable
                options.ApiName = "resourceapi"; //TODO: Change this to correct api name ind Config.cs for AutServer
                options.RequireHttpsMetadata = false;

                options.ForwardDefaultSelector = context =>
                {
                    if (!context.Items.ContainsKey(AuthenticationSocketInterceptor.HTTP_CONTEXT_WEBSOCKET_AUTH_KEY) &&
                            context.Request.Headers.TryGetValue("Upgrade", out var value) &&
                            value.Count > 0 &&
                            value[0] is string stringValue &&
                            stringValue == "websocket")
                    {
                        return "WebSockets";
                    }
                    return JwtBearerDefaults.AuthenticationScheme;
                };

                options.TokenRetriever = new Func<HttpRequest, string>(req =>
                {
                    if (req.HttpContext.Items.TryGetValue(
                        AuthenticationSocketInterceptor.HTTP_CONTEXT_WEBSOCKET_AUTH_KEY,
                        out object token) &&
                        token is string stringToken)
                    {
                        return stringToken;
                    }
                    var fromHeader = TokenRetrieval.FromAuthorizationHeader();
                    var fromQuery = TokenRetrieval.FromQueryString();

                    return fromHeader(req) ?? fromQuery(req);
                });
            });

            services
                .AddInMemorySubscriptions()
                .AddGraphQL(SchemaBuilder.New()
                    // .AddServices(_)
                    .AddQueryType<Queries>()
                    .AddMutationType<Mutations>()
                    .AddSubscriptionType<Subscriptions>()
                    .AddAuthorizeDirectiveType()
                );

            services.AddSingleton<ISocketConnectionInterceptor<HttpContext>, AuthenticationSocketInterceptor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();

            app.UseWebSockets();

            app.UseGraphQL()
                .UsePlayground();
        }
    }
}
