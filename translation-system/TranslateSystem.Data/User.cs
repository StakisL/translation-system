﻿using System;

namespace TranslateSystem.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public AccountInformation Account { get; set; }
    }
}