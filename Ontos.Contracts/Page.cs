using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Contracts
{
    public class Page
    {
        public long Id { get; }
        public string Content { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public Reference[] References { get; }

        public Page(long id, string content, DateTime createdAt, DateTime updatedAt, Reference[] references = null)
        {
            Id = id;
            Content = content;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            References = references ?? new Reference[0];
        }

        public Page(Page page, Reference[] references)
            : this(page.Id, page.Content, page.CreatedAt, page.UpdatedAt, references)
        { }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Page page &&
                   Id == page.Id &&
                   Content == page.Content &&
                   CreatedAt == page.CreatedAt &&
                   UpdatedAt == page.UpdatedAt;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Content, CreatedAt, UpdatedAt);
        }
        #endregion
    }

    public enum PageSortKey
    {
        CreatedAt,
        UpdatedAt,
    }

    public class NewPage
    {
        public string Content { get; }
        public NewExpression Expression { get; }
        public object Properties => new
        {
            content = Content,
            created_at = DateTime.UtcNow,
            updated_at = DateTime.UtcNow,
        };

        public NewPage(string content, NewExpression newExpression = null)
        {
            Content = content;
            Expression = newExpression;
        }
    }

    public class UpdatePage
    {
        public long Id { get; }
        public string Content { get; }
        public object Properties => new
        {
            content = Content,
            updated_at = DateTime.UtcNow,
        };

        public UpdatePage(long id, string content)
        {
            Id = id;
            Content = content;
        }
    }

    public class PageSearchResult
    {
        public long PageId { get; }
        public double Score { get; }
        public string[] Expressions { get; }

        public PageSearchResult(long pageId, double score, string[] expressions)
        {
            PageId = pageId;
            Score = score;
            Expressions = expressions;
        }
    }
}
