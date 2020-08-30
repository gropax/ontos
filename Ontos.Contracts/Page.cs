using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Contracts
{
    public class Page
    {
        public long Id { get; }
        public string Content { get; }

        public Page(long id, string content)
        {
            Id = id;
            Content = content;
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Page content &&
                   Id == content.Id &&
                   Content == content.Content;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Content);
        }
        #endregion
    }

    public class NewPage
    {
        public string Content { get; }
        public object Properties => new { content = Content };
        public NewPage(string content)
        {
            Content = content;
        }
    }

    public class UpdatePage
    {
        public long Id { get; }
        public string Content { get; }
        public object Properties => new { content = Content };
        public UpdatePage(long id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
