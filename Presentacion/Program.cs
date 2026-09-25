using Aplication.Interfaces;
using Aplication.Services;
using Domain.Interfaces;
using Infraestructure.Data;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Servicios de Core ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("AgenteIA", client =>
{
    client.BaseAddress = new Uri("http://localhost:8000");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// --- JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// --- Swagger con soporte JWT ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Nicatek Ecommerce API",
        Version = "v1",
        Description = "Documentaci�n de endpoints del sistema de ecommerce"
    });

    // Permite enviar el token desde Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe: Bearer {tu token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- Configuraci�n de Base de Datos ---
// Si falta la cadena, se corta aqui con un mensaje claro. Sin esta comprobacion
// la API arranca igual y falla en la primera consulta con un error de referencia
// nula, que al desplegar no dice nada sobre el origen real del problema.
var connectionString = builder.Configuration.GetConnectionString("CadenaSQL")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexion 'CadenaSQL'. En local se define en appsettings.json; " +
        "al publicar, con la variable de entorno ConnectionStrings__CadenaSQL.");

builder.Services.AddSingleton(new DbConection(connectionString));

// --- Inyecci�n de Dependencias ---
// Catalogos
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<ISegmentRepository, SegmentRepository>();
builder.Services.AddScoped<ISegmentService, SegmentService>();
builder.Services.AddScoped<IMarkRepository, MarkRepository>();
builder.Services.AddScoped<IMarkService, MarkService>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IAttributeTypeRepository, AttributeTypeRepository>();
builder.Services.AddScoped<IAttributeTypeService, AttributeTypeService>();
builder.Services.AddScoped<IProductVariableTypeRepository, ProductVariableTypeRepository>();
builder.Services.AddScoped<IProductVariableTypeService, ProductVariableTypeService>();
builder.Services.AddScoped<IPaymentMethodTypeRepository, PaymentMethodTypeRepository>();
builder.Services.AddScoped<IPaymentMethodTypeService, PaymentMethodTypeService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IStockMovementTypeRepository, StockMovementTypeRepository>();
builder.Services.AddScoped<IStockMovementTypeService, StockMovementTypeService>();
builder.Services.AddScoped<IMarkByProviderRepository, MarkByProviderRepository>();
builder.Services.AddScoped<IMarkByProviderService, MarkByProviderService>();
builder.Services.AddScoped<IProductIdentificatorRepository, ProductIdentificatorRepository>();
builder.Services.AddScoped<IProductIdentificatorService, ProductIdentificatorService>();

// Seguridad
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<IUserPaymentMethodRepository, UserPaymentMethodRepository>();
builder.Services.AddScoped<IUserPaymentMethodService, UserPaymentMethodService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductVariableRepository, ProductVariableRepository>();
builder.Services.AddScoped<IProductVariableService, ProductVariableService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IAttributeProductRepository, AttributeProductRepository>();
builder.Services.AddScoped<IAttributeProductService, AttributeProductService>();
builder.Services.AddScoped<IAttributeProductVariableRepository, AttributeProductVariableRepository>();
builder.Services.AddScoped<IAttributeProductVariableService, AttributeProductVariableService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartDetailRepository, CartDetailRepository>();
builder.Services.AddScoped<ICartDetailService, CartDetailService>();
builder.Services.AddScoped<IPaymentOrderRepository, PaymentOrderRepository>();
builder.Services.AddScoped<IPaymentOrderService, PaymentOrderService>();
builder.Services.AddScoped<IPaymentOrderDetailRepository, PaymentOrderDetailRepository>();
builder.Services.AddScoped<IPaymentOrderDetailService, PaymentOrderDetailService>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IStockMovementDetailRepository, StockMovementDetailRepository>();
builder.Services.AddScoped<IStockMovementDetailService, StockMovementDetailService>();

// Notificaciones y Favoritos
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
builder.Services.AddScoped<IUserFavoriteService, UserFavoriteService>();

builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

var app = builder.Build();

// --- Pipeline de HTTP ---
// Swagger queda disponible tambien fuera de Development. Al publicar en Azure el
// entorno pasa a Production, y con la condicion anterior la unica forma de
// comprobar que la API respondia era llamarla a ciegas desde la app.
//
// Se puede apagar poniendo la App Setting "Swagger:Habilitado" en false.
var swaggerHabilitado = app.Configuration.GetValue("Swagger:Habilitado", true);
if (swaggerHabilitado)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nicatek Ecommerce API v1");
    });
}

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
app.Run();