using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Task5WebApp.Data;
using Task5WebApp.Model;

namespace Task5WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        private Task5WebAppContext _context;

        public IndexModel(ILogger<IndexModel> logger, Task5WebAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        [Required, MinLength(1)]
        public string Name { get; set; }
       
        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("message");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (ModelState.IsValid)
            {
                string name = await GetOrCreate();
                await Authenticate(name);
            }
            return RedirectToPage("message");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage();
        }

        private async Task<string> GetOrCreate()
		{
            var AppUser = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Name == Name);
            if (AppUser == null)
            {
                AppUser = CreateUser();
                AppUser.Name = Name;
                await _context.ApplicationUser.AddAsync(AppUser);
                await _context.SaveChangesAsync();
            }
            return AppUser.Name;
        }

        private async Task Authenticate(string name)
		{
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, name),
                };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively");
            }
        }
    }
}