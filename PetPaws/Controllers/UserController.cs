using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetPaws.Models;
using PetPaws.Models.DTOs;
using PetPaws.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetPaws.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiDbContext _dbRepository;

        public UserController(ApiDbContext dbRepository)
        {
            _dbRepository = dbRepository;
        }

        // GET api/values/5
        [HttpGet("{email}")]
        public ActionResult<GetUserDto> GetUser([FromRoute(Name = "email")] String email)
        {
            var user = _dbRepository.Users.FirstOrDefault(U=>U.Email==email);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = new GetUserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                imagePath = user.imagePath,
            };
            return userDto;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<User>> Create(CreateUserDto userDto)
        {
            var user =new User();
            user.Username = userDto.Username;
            user.Phone = userDto.Phone;
            user.imagePath = userDto.imagePath;
            user.Email = userDto.Email;
            user.Password = user.Password;

            _dbRepository.Add(user);
            await _dbRepository.SaveChangesAsync();
            return Ok(user);
        }

        // POST api/values
        [HttpPost("saveanimal/{animalId}")]
        public async Task<ActionResult<User>> SaveAnimal(int animalId, GetUserDto userDto)
        {
            if (userDto.Email == null)
            {
                return BadRequest();
            }
            var user = _dbRepository.Users.Include(U => U.Animals).FirstOrDefault(U => U.Email == userDto.Email);
            var animal = await _dbRepository.Animals.FindAsync(animalId);
            if (animal == null)
                return NotFound();

            user.Animals.Add(animal);
            _dbRepository.Entry(user).State = EntityState.Modified;
            await _dbRepository.SaveChangesAsync();
            return Ok(user.Animals);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CreateUserDto userDto)
        {
            if (id != userDto.UserId)
                return BadRequest();
            var user = _dbRepository.Users.Find(userDto.UserId);

            user.Username = userDto.Username;
            user.Password = userDto.password;
            user.imagePath = userDto.imagePath;

            _dbRepository.Entry(user).State = EntityState.Modified;
            await _dbRepository.SaveChangesAsync();
            return Ok();
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _dbRepository.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _dbRepository.Users.Remove(user);
            await _dbRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("deletesaving/{id}")]
        public async Task<IActionResult> DeleteSavingAnimal(GetUserDto userDto, int id)
        {
            //List<Animal> animalList = await _dbRepository.Animals.Where(u => u.UserName == userEmail).ToListAsync();
            //var user = _dbRepository.Users.FirstOrDefault(a => a.Email == userEmail);
            Animal animaldeleted = new Animal();
            //if (user == null)
            //{
            //    return BadRequest();
            //}

            if (userDto.Email == null)
            {
                return BadRequest();
            }
            var user = _dbRepository.Users.Include(U=>U.Animals).FirstOrDefault(U => U.Email == userDto.Email);
            user.Animals.ForEach(a =>
            {
                if (a.AnimalId == id)
                {
                    animaldeleted = a;
                }
            });
            user.Animals.Remove(animaldeleted);
            _dbRepository.Update(user);
            await _dbRepository.SaveChangesAsync();
            return Ok(user.Animals);
        }


    }
}

