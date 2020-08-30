using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Contracts
{
    public class Content
    {
        public long Id { get; }
        public string Details { get; }

        public Content(long id, string details)
        {
            Id = id;
            Details = details;
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Content content &&
                   Id == content.Id &&
                   Details == content.Details;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Details);
        }
        #endregion
    }

    public class NewContent
    {
        public string Details { get; }
        public object Properties => new { details = Details };
        public NewContent(string details)
        {
            Details = details;
        }
    }

    public class UpdateContent
    {
        public long Id { get; }
        public string Details { get; }
        public object Properties => new { details = Details };
        public UpdateContent(long id, string details)
        {
            Id = id;
            Details = details;
        }
    }
}
