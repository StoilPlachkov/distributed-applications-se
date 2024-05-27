﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class Train : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Type { get; set; }
        public int Capacity { get; set; }
        [Required]
        public DateTime ManufactureDate { get; set; }
        public double MaxSpeed { get; set; }
    }
}
