using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.App_Code
{
    public static class RecursiveHierarchicalParentchild
    {
        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursive(children, selector))
                    yield return child;
            }
        }

        //Usage:
        //var lookup = col.ToLookup(x => x.Parent_Id);
        //var res = lookup[null].SelectRecursive(x => lookup[x.Id]).ToList();

        //Single line
        //static void BuildTree(List<Group> items)
        //{
        //    items.ForEach(i => i.Children = items.Where(ch => ch.ParentID == i.ID).ToList());
        //}
        //BuildTree(flatList);
        //public class Group
        //{
        //    public int ID { get; set; }
        //    public int? ParentID { get; set; }
        //    public List<Group> Children { get; set; }

        //}
    }
}