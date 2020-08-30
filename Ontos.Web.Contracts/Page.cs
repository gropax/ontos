using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Ontos.Web.Contracts
{
    public class PageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }

        public PageDto(long id, string content)
        {
            Id = id;
            Content = content;
        }

        public PageDto(Page page)
        {
            Id = page.Id;
            Content = page.Content;
        }
    }

    public class NewPageDto
    {
        public string Content { get; set; }

        public NewPage ToModel()
        {
            return new NewPage(Content);
        }
    }

    public class UpdatePageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }

        public UpdatePage ToModel()
        {
            return new UpdatePage(Id, Content);
        }
    }
}
