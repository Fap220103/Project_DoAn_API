using Domain.Bases;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Config : BaseEntityCommon
    {
        public string SmtpHost { get; set; } = null!;   
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; } = null!;
        public string SmtpPassword { get; set; } = null!;
        public bool SmtpUseSSL { get; set; }
        public bool Active { get; set; }


        public Config() : base() { } //for EF Core
        public Config(
            string? userId,
            string title,
            string? description,
            string smtpHost,
            int smtpPort,
            string smtpUserName,
            string smtpPassword,
            bool smtpUseSSL,
            bool active
            ) : base(userId, title, description)
        {      
            SmtpHost = smtpHost.Trim();
            SmtpPort = smtpPort;
            SmtpUserName = smtpUserName.Trim();
            SmtpPassword = smtpPassword.Trim();
            SmtpUseSSL = smtpUseSSL;
            Active = active;
        }


        public void Update(
            string? userId,
            string title,
            string? description,
            string smtpHost,
            int smtpPort,
            string smtpUserName,
            string? smtpPassword,
            bool smtpUseSSL,
            bool active
            )
        {
            Title = title.Trim();
            Description = description?.Trim();
            SmtpHost = smtpHost.Trim();
            SmtpPort = smtpPort;
            SmtpUserName = smtpUserName.Trim();
            if (!string.IsNullOrEmpty(smtpPassword?.Trim()))
            {
                SmtpPassword = smtpPassword!.Trim();
            }
            SmtpUseSSL = smtpUseSSL;
            Active = active;

            SetAudit(userId);
        }

        public void Delete(
            string? userId
            )
        {
            SetAsDeleted();
            SetAudit(userId);
        }
    }
}
