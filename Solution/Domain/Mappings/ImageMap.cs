using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class ImageMap : ClassMapping<Image>
    {

        public ImageMap()
        {
            Table("Images");

            Id(x => x.ImageId, map =>
            {
                map.Column("ImageId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.Bytes, map =>
            {
                map.Column("Bytes");
                map.Length(Int32.MaxValue);
            });
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
        }
    }
}
