using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class AdditionalInstructorInfoMap : ClassMapping<AdditionalInstructorInfo>
    {
        public AdditionalInstructorInfoMap()
        {
            Table("AdditionalInstructorInfos");
            Lazy(true);

            Id(x => x.InstructorId, map => map.Column("InstructorId"));
            Property(x => x.AvailableFrom, map =>
            {
                map.Column("AvailableFrom");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.AvailableTo, map =>
            {
                map.Column("AvailableTo");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.ShortBiography, map => map.Column("ShortBiography"));
        }
    }
}
