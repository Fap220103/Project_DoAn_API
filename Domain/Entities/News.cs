using Domain.Bases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class News : BaseEntityCommon
    {
        [AllowHtml]
        public string? Detail { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public string Type { get; set; } 
        public News() : base() { } //for EF Core
        public News(
            string? userId,
            string title,
            string? description,
            string? image,
            string? detail,
            string? link,
            string type
            ) : base( userId, title, description)
        {
           
            Image = image;
            Detail = detail;
            Link = link;
            Type = type;
        }
        public void Update(
            string? userId,
            string title,
            string? description,
            string? image,
            string? detail,
            string? link,
            string type
            ) 
        {
            Title = title;
            Description = description;
            Image = image;
            Detail = detail;
            Link = link;
            Type = type;
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
