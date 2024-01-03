using System;
namespace PetPaws.Models.DTOs
{
	public record struct GetUserDto
	(
        int UserId,
        String Username,
        String Email,
        String Phone,
        String Password,
        String imagePath
		);
		
	
}

