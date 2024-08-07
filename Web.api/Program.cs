using BLL;
using BLL.Interface;
using BLL.Services;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace CodeNamesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<AppDbContext>(s =>
                s.UseSqlServer(builder.Configuration.GetConnectionString("ConString")));

            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWordRepository, WordRepository>();
            builder.Services.AddScoped<IWordRoomRepository, WordRoomRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IWordRoomService, WordRoomService>();
            builder.Services.AddScoped<IWordService, WordService>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new AutomapperProfile()));
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}