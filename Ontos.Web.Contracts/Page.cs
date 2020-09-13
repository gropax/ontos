using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Ontos.Web.Contracts
{
    public class PageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public ReferenceDto[] References { get; set; }

        public PageDto(long id, string content, string type, ReferenceDto[] references)
        {
            Id = id;
            Content = content;
            Type = type;
            References = references ?? new ReferenceDto[0];
        }

        public PageDto(Page page)
        {
            Id = page.Id;
            Content = page.Content;
            Type = page.Type.ToString();
            References = page.References.Select(r => new ReferenceDto(r)).ToArray();
        }
    }

    public class NewPageDto
    {
        public string Content { get; set; }
        public NewExpressionDto Expression { get; set; }
        public string Type { get; set; }

        public NewPage ToModel()
        {
            var type = string.IsNullOrWhiteSpace(Type) ? default : Enum.Parse<PageType>(Type);
            return new NewPage(Content, Expression?.ToModel(), type);
        }
    }

    public class UpdatePageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }

        public UpdatePage ToModel()
        {
            var type = string.IsNullOrWhiteSpace(Type) ? default : Enum.Parse<PageType>(Type);
            return new UpdatePage(Id, Content, type);
        }
    }

    public class PageSearchResultDto
    {
        public long PageId { get; set; }
        public double Score { get; set; }
        public string[] Expressions { get; set; }

        public PageSearchResultDto(long pageId, double score, string[] expressions)
        {
            PageId = pageId;
            Score = score;
            Expressions = expressions;
        }

        public PageSearchResultDto(PageSearchResult result)
        {
            PageId = result.PageId;
            Score = result.Score;
            Expressions = result.Expressions;
        }
    }

    public class SearchPageDto
    {
        public string Language { get; set; }
        public string Text { get; set; }
    }
}
