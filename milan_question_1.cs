Hello Mr.Milan,
I hope you're doing very well,

I got a question regarding C.A :

My domain entities need to depend on an 'ApplicationUser' that inherits from 'IdentityUser', this for properties and navigation like CreatedBy, UpdatedBy, and also for accessing information on the IdentityUser/ApplicationUser  itself.

This is something you have not shown in your course, I also spent hours to look on Youtube and search the entire github for examples in many different ways but unfortunately I did not find any useful information. 

I understand that you use a custom 'Users' table and an external Identity provider, but what if I want to stick to the framework and use 'ApplicationUser' ?

1- Should I install 'Microsoft.AspNetCore.Identity' to my DOMAIN LAYER  ?

2- Is the following solution I came to good ?

3- Can i reference the domain layer inside the infrastructure layer DIRECTLY ? (Adding 'domain layer' project reference inside 'infra layer') ?

----------------------------------------------------------------
This is the solution I came to as a beginner so please advise, you may also make a video on that as I've seen so many posts over the internet asking about this and no answer is out there.

# Domain layer:
    public interface IDomainUser
    {
    }

    public class Order
    {
        public int Id { get; set; }

        public string? ForStoreUserId { get; set; }
        public virtual IDomainUser? ForStoreUser { get; set; }

        public string? CreatedByUserId { get; set; }
        public virtual IDomainUser? CreatedByUser { get; set; }
    }


# Application layer
~ nothing is here

# Infrastructure layer

public class ApplicationUser : IdentityUser, IDomainUser
{
}


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>().HasOne(typeof(ApplicationUser), nameof(Order.ForStoreUser)).WithMany().HasForeignKey(nameof(Order.ForStoreUserId)).OnDelete(DeleteBehavior.NoAction).IsRequired();
        builder.Entity<Order>().Navigation(d => d.ForStoreUser).AutoInclude(true);


        // The project can have dozens of tables, therefore, hunders of these lines !
        builder.Entity<Order>().HasOne(typeof(ApplicationUser), nameof(Order.CreatedByUser)).WithMany().HasForeignKey(nameof(Order.CreatedByUserId)).OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Order>().Navigation(d => d.CreatedByUser).AutoInclude(true);

        base.OnModelCreating(builder);
    }

    public DbSet<Order> Orders { get; set; }
}

# Test

var person = await _context.Persons.FirstOrDefaultAsync(m => m.Id == id);
person.ForStoreUser;  // navigation and eager loading works
person.CreatedByUser;  // navigation and eager loading works


Please give attention to my question because I really do not know anybody else to ask.
Thank you very much.