using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Entity;

namespace Talabat.Repository.Data.Configrations
{
    internal class OrderItemsConfigrations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(
                orderItem => orderItem.Product,
                Product => Product.WithOwner()
                );

            builder.Property(O => O.Price).HasColumnType("decimal(18,2)");
        }
    }
}
