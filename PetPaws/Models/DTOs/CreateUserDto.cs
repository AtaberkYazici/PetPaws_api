using System;
namespace PetPaws.Models.DTOs
{
	public record struct CreateUserDto(
		int UserId,
		string Username,
		string Email,
		string Phone,
		string imagePath,
		string password
        );
	
}

