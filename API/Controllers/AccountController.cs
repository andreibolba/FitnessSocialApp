using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController:BaseAPIController
    {

        private readonly SocialAppContext _context;

        public AccountController(SocialAppContext context)
        {
            _context = context;
        }

        private async Task<bool> UsernameExists(string username){
            return await _context.People.AnyAsync(person=>person.Username==username);
        }

        private async Task<bool> EmailExists(string email){
            return await _context.People.AnyAsync(person=>person.Email==email);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Person>> Register([FromBody] PersonRegisterDto person){
            
            switch(Utils.Utils.IsPasswordValid(person.Password)){
                case 900:
                return BadRequest("Password is too short! It must be at least 8 charaters long!");
                case 901:
                return BadRequest("Password doesn't have capital letters! It must have at least 1 capital letter!");
                case 902:
                return BadRequest("Password doesn't have small letters! It must have at least 1 small letter!");
                case 903:
                return BadRequest("Password doesn't have digits! It must have at least 1 digit!");
                case 904:
                return BadRequest("Password doesn't have any special charaters! It must have at least 1 special charater!");
                default:
                break;
            }

            if(person.Password!=person.RetypePassword)
                return BadRequest("Password and retype password are not the same!");

            if(await UsernameExists(person.Username))
                return BadRequest("Username exists!");

            if(await EmailExists(person.Email))
                return BadRequest("Email exists!");

            using var hmac=new HMACSHA512();

            Person newPerson= new Person{
                FirstName=person.FirstName,
                LastName=person.LastName,
                Email=person.Email,
                Username=person.Username,
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(person.Password)),
                PasswordSalt=hmac.Key,
                BirthDate=person.birthDate,
                IsAdmin=false,
                Deleted=false
            };

            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            return newPerson;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Person>> LogIn([FromBody] PersonLogInDto person){
            var loggedPerson=await _context.People.SingleOrDefaultAsync(p=>p.Email==person.Email);
            if(loggedPerson==null)
                return Unauthorized("Invalid email!");
            using var hmac=new HMACSHA512(loggedPerson.PasswordSalt);
            var compputedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(person.Password));
            for(int i=0;i<compputedHash.Length;i++){
                if(compputedHash[i]!=loggedPerson.PasswordHash[i])
                return Unauthorized("Invalid password");
            }
            return loggedPerson;
        }
    }
}