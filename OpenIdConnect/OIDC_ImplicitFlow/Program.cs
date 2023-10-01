using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure authentication - OIDC Implicit flow + Cookie auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie", options =>
{
    options.Cookie.Name = "MyAuthCookie";
    options.LoginPath = "Home/Index";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration.GetValue<string>("OIDC_Config:Authority");
    options.ClientId = builder.Configuration.GetValue<string>("OIDC_Config:ClientId");
    options.ResponseType = "token id_token";
    options.ResponseMode = "form_post";
    options.CallbackPath = "/Home/SignIn";
    options.SignInScheme = "cookie";
    options.SaveTokens = true;
    options.AccessDeniedPath = "Home/AccessDenied";
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
