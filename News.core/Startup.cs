using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using News.core.AuthHelp.Models;
using News.core.IRepository;
using News.core.IServices;
using News.core.Model.Entities;
using News.core.Repository;
using News.core.Services;

namespace News.core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            #region ����
            services.AddCors(m => m.AddPolicy("any", a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            #endregion

            #region swagger����
            services.AddSwaggerGen(m =>
            {
                //��swagger����˵��
                //����/�汾 ����
                //����/��������/��д�����֤ ѡ��
                m.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Api",
                    Version = "v1",
                    Description = "�ҵ�api"
                });


            });
            #endregion

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region ͼƬ��֤�����
            services.AddMemoryCache()
              .AddSimpleCaptcha(builder =>
              {
                  builder.UseMemoryStore();
              });
            #endregion

            #region JWT��֤����
            //��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
            //�����ð󶨵�JwtSettingsʵ����
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            //��������֤
            services.AddAuthentication(options =>
            {
                //��֤middleware����
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //jwt token��������
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                    //Token�䷢����
                    ValidIssuer = jwtSettings.Issuer,
                    //�䷢��˭
                    ValidAudience = jwtSettings.Audience,
                    //�����keyҪ���м���
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // �Ƿ�Ҫ��Token��Claims�б������Expires
                    // RequireExpirationTime = true,
                    // ����ķ�����ʱ��ƫ����
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                    // ValidateLifetime = true
                };
            });

            #endregion





            #region ����ע��
            services.AddDbContext<NewsDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICategoryRepostorycs, CategoryRepostorycs>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<INewsToCategoryRepository, NewsToCategoryRepository>();
            services.AddScoped<INewsToCategoryService, NewsToCategoryService>();

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();

            services.AddScoped<ICommentRepostory, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<ICommentChildRepository, CommentChildRepository>();
            services.AddScoped<ICommentChildService, CommentChildService>();

            services.AddScoped<IFocusRepository, FocusRepository>();
            services.AddScoped<IFocusService, FocusService>();

            services.AddScoped<IImgsRepository, ImgsRepository>();
            services.AddScoped<IImgsService, ImgsService>();
            #endregion



            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(m =>
            {
                m.SwaggerEndpoint("/swagger/v1/swagger.json", "My api v1");

            });

            //��̬��Դ
            app.UseStaticFiles(); //wwwrootĿ¼
            app.UseAuthentication();
            app.UseRouting();

            //����
            app.UseCors();





            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
