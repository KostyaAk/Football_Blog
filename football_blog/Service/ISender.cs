using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace football_blog.Service
{
    public interface ISender
    {
        Task SendMessage(string email, string subject, string message);
    }
}
