﻿using System.ComponentModel.DataAnnotations;

namespace HOTELAPI1.Models
{
    public class Cliente
    {
        
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }


}
