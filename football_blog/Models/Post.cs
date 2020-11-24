using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace football_blog.Models
{
    public class Post
    {
        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }
        [Required(ErrorMessage = "Введите название статьи!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Введите статью!")]
        public string Text { get; set; }

        public string Image { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        public Post()
        {
            Comments = new List<Comment>();
        }

    }
}
