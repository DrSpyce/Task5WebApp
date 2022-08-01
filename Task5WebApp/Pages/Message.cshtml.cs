using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task5WebApp.Data;
using Task5WebApp.Model;


namespace Task5WebApp.Pages
{
    [Authorize]
    public class MessageModel : PageModel
    {
        private readonly Task5WebAppContext _context;

        public MessageModel(Task5WebAppContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Message Message { get; set; } = default!;

        public IList<Message> Messages { get; set; } = default!;

        public string CurrentUserName { get; set; } = default!;

        [TempData]
        public string Alert { get; set; }

        public async Task<IActionResult> OnPostSendAsync()
        {
            if (ModelState.IsValid)
            {
                SetUserName();
                if (CurrentUserName != null)
                {
                    if(await IfExistRecipient())
                    {
                        Message.Sender = CurrentUserName;
                        Message.TimeOfSending = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
                        _context.Message.Add(Message);
                        await _context.SaveChangesAsync();
                        Alert = $"Message to {Message.Recipient} was sent";
                    }
                }
            }
            return RedirectToPage();
        }

        public async Task OnGetAsync()
        {
            SetUserName();
            if (_context.Message != null || CurrentUserName != null)
            {
                Messages = await _context.Message.Where(m => m.Recipient == CurrentUserName).OrderBy(m => m.TimeOfSending).ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetSearchAsync(string term)
        {
            var names = await _context.ApplicationUser.Where(u => u.Name.Contains(term)).Select(u => u.Name).ToListAsync();
            return new JsonResult(names);
        }


        public async Task<IActionResult> OnGetUpdateAsync()
        {
            SetUserName();
            Messages = await _context.Message.Where(m => m.Recipient == CurrentUserName).ToListAsync();
            return new JsonResult(Messages);
        }

        public async Task<IActionResult> OnGetIdAsync(int id)
        {
            Messages = await _context.Message.Where(m => m.Id == id).ToListAsync();
            return new JsonResult(Messages);
        }

        public void SetUserName()
        {
            ClaimsPrincipal currentUser = this.User;
            CurrentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<bool> IfExistRecipient()
        {
            var recipient = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Name == Message.Recipient);
            if(recipient != null)
            {
                return true;
            }
            return false;
        }
    }
}
