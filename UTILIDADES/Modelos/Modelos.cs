﻿namespace UTILIDADES.Modelos
{
    public class SmtpSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string MailSender {  get; set; } = string.Empty ;
        public string NameSender {  get; set; } = string.Empty ;

    }
}
