using FastX.Contexts;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using FastX.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace FastX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.WriteIndented = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });


		builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", opts =>
    {
       // opts.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
        opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

            builder.Services.AddDbContext<FastXContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("fastx"));
            });

            #region Repository
            builder.Services.AddScoped<IRepository<int, Routee>, RouteRepository>();
            builder.Services.AddScoped<IRepository<int, Bus>, BusRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<string, AllUser>, AllUserRepository>();
            builder.Services.AddScoped<IRepository<int, Admin>, AdminRepository>();
            builder.Services.AddScoped<IBookingRepository<int, Booking>, BookingRepository>();
            builder.Services.AddScoped<IRepository<int, BusOperator>, BusOperatorRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<ISeatRepository<int, Seat>, SeatRepository>();
            builder.Services.AddScoped<IRepository<int, Ticket>, TicketRepository>();
            builder.Services.AddScoped<IAmenityRepository<int, Amenity>, AmenityRepository>();
            #endregion


            #region Service
            builder.Services.AddScoped<IRouteeService, RouteService>();
            builder.Services.AddScoped<IBusService, BusService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAllUserService, AllUserService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IBusOperatorService, BusOperatorService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
        

            #endregion





            

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
		
	app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}