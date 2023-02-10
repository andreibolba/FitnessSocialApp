using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController:BaseAPIController
    {

        private readonly SocialAppContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(SocialAppContext context,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        private async Task<bool> UsernameExists(string username){
            return await _context.People.AnyAsync(person=>person.Username==username);
        }

        private async Task<bool> EmailExists(string email){
            return await _context.People.AnyAsync(person=>person.Email==email);
        }

        [HttpPost("register")]
        public async Task<ActionResult<PersonDto>> Register([FromBody] PersonRegisterDto person){

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

            return new PersonDto{
                Username=newPerson.Username,
                Token=_tokenService.CreateToken(newPerson)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<PersonDto>> LogIn([FromBody] PersonLogInDto person){
            var loggedPerson=await _context.People.SingleOrDefaultAsync(p=>p.Email==person.Email);
            if(loggedPerson==null)
                return Unauthorized("Invalid email!");
            using var hmac=new HMACSHA512(loggedPerson.PasswordSalt);
            var compputedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(person.Password));
            for(int i=0;i<compputedHash.Length;i++){
                if(compputedHash[i]!=loggedPerson.PasswordHash[i])
                return Unauthorized("Invalid password");
            }
            return new PersonDto{
                Username=loggedPerson.Username,
                Token=_tokenService.CreateToken(loggedPerson)
            };
        }
    }
}