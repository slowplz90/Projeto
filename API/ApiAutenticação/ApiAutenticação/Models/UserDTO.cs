﻿namespace ApiAutenticação.Models
{
    public class UserDTO
    {
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}