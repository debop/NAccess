using FluentNHibernate.Mapping;
using NSoft.NFramework.Data.NHibernateEx;
using NSoft.NFramework.Data.NHibernateEx.Domain;

namespace NSoft.NAccess.Domain.Model
{
    public class NodePositionMap : ComponentMap<TreeNodePosition>
    {
        public NodePositionMap()
        {
            Map(x => x.Order).Column("TreeOrder".AsNamingText());
            Map(x => x.Level).Column("TreeOrder".AsNamingText());
        }
    }
}