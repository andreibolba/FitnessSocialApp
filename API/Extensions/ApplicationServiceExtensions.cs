using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Repository;
using API.Models;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<InternShipAppSystemContext>(options =>{
            options.UseSqlServer(config.GetConnectionString("dbconn"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ILoggRepository, LoggRepository>();
            services.AddScoped<IInternGroupRepository, InternGroupRepository>();
            services.AddScoped<IPasswordLinkRepository, PasswordLinkRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();
            services.AddScoped<ITestInternGroupRepository, TestInternGroupRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IQuestionSolutionRepository, QuestionSolutionRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IStatsRepository, StatsRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IMessageRepository,MessageRepository>();
            services.AddScoped<IGroupChatRepository,GroupChatRepository>();
            services.AddScoped<IGroupChatMessagesRepostitory,GroupChatMessageRepository>();
            services.AddScoped<INoteRepository,NoteRepository>();
            services.AddScoped<IChallengeRepository,ChallengeRepository>();
            services.AddScoped<IChallengeSolutionRepository,ChallengeSolutionRepository>();
            services.AddScoped<ITaskRepository,TaskRepository>();
            services.AddScoped<ITaskSolutionRepository,TaskSolutionRepository>();
            services.AddScoped<ITaskInternGroupRepository,TaskInternGroupRepository>();
            services.AddScoped<ISubTaskRepository,SubTaskRepository>();
            services.AddScoped<IFeedbackRepository,FeedbacksRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));
            return services;
        }
    }
}