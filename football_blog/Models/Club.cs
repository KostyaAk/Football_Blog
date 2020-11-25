using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace football_blog.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubDescription { get; set; }

        public string ClubImage { get; set; }
    }
}
