﻿namespace MyWebApp
{
    public interface IEmailService
    {
        Task<bool> SendMailAsync(string emailAddress, string name, string subject, string body);
    }
}
