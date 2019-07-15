using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        //Creating a secure api using [Authorize] attribute
        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserprofile()
        {
            //How do we identify user who made this request.
            //In order to access these private or secure routes/api who have this attribute [authorise], we need to
            //send the JWT token which we received during login oprn. And then JWT Auth system will validate that token.
            //Inside that token we have added the claim userID, so while accessing this secure web api method here JWT auth 
            //system will extract the Payload or UserID claim. So in order to access USERID here, we do as:
            string userId = User.Claims.First(c => c.Type == "UserID").Value;

            //with userID we can retrieve the details of user such as firstname, email etc by injecting UserManager
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName

            };
        }
    }
}