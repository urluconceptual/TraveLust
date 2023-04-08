﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Post title is mandatory!")]
        [StringLength(100, ErrorMessage = "Post title cannot have more than 100 characters!")]
        [MinLength(5, ErrorMessage = "Post title cannot have less than 5 characters!")]
        [Display(Name = "Post Title")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Please provide a price for this sight!")]
        public double Price { get; set; }

        public float? Rating { get; set; }
    }
}
