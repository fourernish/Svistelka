﻿using System.ComponentModel.DataAnnotations;

namespace Svistelka.Models
{
    public class User
    {
        public User()
        {
            Microposts = new HashSet<Micropost>();
            RelationFolloweds = new HashSet<Relation>();
            RelationFollowers = new HashSet<Relation>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Имя должно быть!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Почта должна быть!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль должен быть!")]
        public string Password { get; set; } = null!;

        public string PasswordConfirmation { get; set; } = null!;
        public bool IsAdmin { get; set; }

        public virtual ICollection<Micropost> Microposts { get; set; }
        public virtual ICollection<Relation> RelationFolloweds { get; set; }
        public virtual ICollection<Relation> RelationFollowers { get; set; }
    }
}
