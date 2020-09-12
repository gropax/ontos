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
        public ReferenceDto[] References { get; set; }

        public PageDto(long id, string content, ReferenceDto[] references)
        {
            Id = id;
            Content = content;
            References = references ?? new ReferenceDto[0];
        }

        public PageDto(Page page)
        {
            Id = page.Id;
            Content = page.Content;
            References = page.References.Select(r => new ReferenceDto(r)).ToArray();
        }
    }

    public class NewPageDto
    {
        public string Content { get; set; }
        public NewExpressionDto Expression { get; set; }

        public NewPage ToModel()
        {
            return new NewPage(Content, Expression?.ToModel());
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
