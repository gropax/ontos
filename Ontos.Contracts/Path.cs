using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Contracts
{
    public class PagePath
    {
        public Page[] Pages { get; }
        public Relation[] Relations { get; }

        public PagePath(Page[] pages, Relation[] relations)
        {
            Pages = pages;
            Relations = relations;
        }

        public static PagePath Create(Page[] pages, Relation[] relations)
        {
            Validate(pages, relations);
            return new PagePath(pages, relations);
        }

        private static void Validate(Page[] pages, Relation[] relations)
        {
            if (pages.Length != relations.Length - 1)
                throw new ArgumentException($"Mismatch between nodes and relations counts.");

            for (int i = 0; i < relations.Length; i++)
            {
                var r = relations[i];
                var origin = pages[i];
                var target = pages[i + 1];
                if (r.OriginId != origin.Id || r.TargetId != target.Id)
                    throw new ArgumentException($"Mismatch between node and relation ids.");
            }
        }
    }

}
