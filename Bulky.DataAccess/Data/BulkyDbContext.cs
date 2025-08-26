using Bulky.Models.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data;

public class BulkyDbContext : IdentityDbContext<ApplicationUser>
{
    public BulkyDbContext(DbContextOptions<BulkyDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Section> Sections { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // --- Categories ---
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fiction", IsHidden = false, CreatedAt = baseDate, UpdatedAt = null, DisplayOrder = 1 },
            new Category { Id = 2, Name = "Science Fiction", IsHidden = false, CreatedAt = baseDate.AddDays(1), UpdatedAt = null, DisplayOrder = 2 },
            new Category { Id = 3, Name = "Mystery", IsHidden = false, CreatedAt = baseDate.AddDays(2), UpdatedAt = null, DisplayOrder = 3 },
            new Category { Id = 4, Name = "Romance", IsHidden = false, CreatedAt = baseDate.AddDays(3), UpdatedAt = null, DisplayOrder = 4 },
            new Category { Id = 5, Name = "Biography", IsHidden = false, CreatedAt = baseDate.AddDays(4), UpdatedAt = null, DisplayOrder = 5 },
            new Category { Id = 6, Name = "Children's Books", IsHidden = false, CreatedAt = baseDate.AddDays(5), UpdatedAt = null, DisplayOrder = 6 },
            new Category { Id = 7, Name = "History", IsHidden = false, CreatedAt = baseDate.AddDays(7), UpdatedAt = null, DisplayOrder = 7 },
            new Category { Id = 8, Name = "Fantasy", IsHidden = false, CreatedAt = baseDate.AddDays(8), UpdatedAt = null, DisplayOrder = 8 },
            new Category { Id = 9, Name = "Horror", IsHidden = true, CreatedAt = baseDate.AddDays(9), UpdatedAt = baseDate.AddDays(15), DisplayOrder = 9 }
        );

        //--- Companies --- 
        modelBuilder.Entity<Company>().HasData(
               new Company
               {
                   Id = 1,
                   Name = "Tech Solution",
                   StreetAddress = "123 Tech St",
                   City = "Tech City",
                   PostalCode = "12121",
                   State = "IL",
                   PhoneNumber = "6669990000",
                   CreatedAt = baseDate,
                   DisplayOrder = 1,
                   IsHidden = false,
                   UpdatedAt = null
               },
               new Company
               {
                   Id = 2,
                   Name = "Vivid Books",
                   StreetAddress = "999 Vid St",
                   City = "Vid City",
                   PostalCode = "66666",
                   State = "IL",
                   PhoneNumber = "7779990000",
                   CreatedAt = baseDate.AddDays(1),
                   DisplayOrder = 2,
                   IsHidden = false,
                   UpdatedAt = null
               },
               new Company
               {
                   Id = 3,
                   Name = "Readers Club",
                   StreetAddress = "999 Main St",
                   City = "Lala land",
                   PostalCode = "99999",
                   State = "NY",
                   PhoneNumber = "1113335555",
                   CreatedAt = baseDate.AddDays(2),
                   DisplayOrder = 3,
                   IsHidden = false,
                   UpdatedAt = null
               }
               );

        //--- Products ---

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "To Kill a Mockingbird",
                Description = "Harper Lee's Pulitzer Prize-winning novel explores racial injustice and moral growth in the American South. Through young Scout Finch's eyes, the story of her father, Atticus, defending a Black man unjustly accused, reveals themes of empathy, courage, and humanity. A timeless classic celebrated for its powerful narrative.",
                ISBN = "978-0-06-112008-4",
                Author = "Harper Lee",
                ListPrice = 18.99,
                Price = 16.99,
                Price50 = 15.99,
                Price100 = 14.99,
                CategoryId = 1,
                IsHidden = false,
                CreatedAt = baseDate,
                UpdatedAt = null,
                DisplayOrder = 1
            },
        new Product
        {
            Id = 2,
            Title = "The Great Gatsby",
            Description = "F. Scott Fitzgerald's iconic novel captures the Jazz Age's extravagance and disillusionment. Jay Gatsby's obsessive love for Daisy Buchanan and his pursuit of the American Dream unravel in a tragic tale of wealth, ambition, and unattainable desires, set against a backdrop of opulent Long Island parties.",
            ISBN = "978-0-7432-7356-5",
            Author = "F. Scott Fitzgerald",
            ListPrice = 16.99,
            Price = 14.99,
            Price50 = 13.99,
            Price100 = 12.99,
            CategoryId = 1,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(1),
            UpdatedAt = null,
            DisplayOrder = 2
        },
        new Product
        {
            Id = 3,
            Title = "1984",
            Description = "George Orwell's chilling dystopian novel depicts a totalitarian world of surveillance and control. Winston Smith's rebellion against the oppressive Party and Big Brother explores themes of freedom, truth, and individuality. A haunting, prophetic masterpiece that remains a cornerstone of modern literature.",
            ISBN = "978-0-452-28423-4",
            Author = "George Orwell",
            ListPrice = 19.99,
            Price = 17.99,
            Price50 = 16.99,
            Price100 = 15.99,
            CategoryId = 1,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(2),
            UpdatedAt = null,
            DisplayOrder = 3
        },

        // Science Fiction Books (CategoryId = 2)
        new Product
        {
            Id = 4,
            Title = "Dune",
            Description = "Frank Herbert's epic sci-fi saga follows young Paul Atreides on the desert planet Arrakis, where the valuable spice melange shapes destinies. Blending politics, religion, and ecology, this richly detailed masterpiece explores power and survival in a gripping, universe-spanning narrative.",
            ISBN = "978-0-441-17271-9",
            Author = "Frank Herbert",
            ListPrice = 22.99,
            Price = 20.99,
            Price50 = 19.99,
            Price100 = 18.99,
            CategoryId = 2,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(3),
            UpdatedAt = null,
            DisplayOrder = 4
        },
        new Product
        {
            Id = 5,
            Title = "Ender's Game",
            Description = "Orson Scott Card's gripping sci-fi novel follows young Ender Wiggin, a brilliant strategist trained to lead humanity against an alien threat. Combining intense military tactics with deep moral questions, this coming-of-age tale explores leadership, sacrifice, and the cost of war.",
            ISBN = "978-0-8125-5070-2",
            Author = "Orson Scott Card",
            ListPrice = 18.99,
            Price = 16.99,
            Price50 = 15.99,
            Price100 = 14.99,
            CategoryId = 2,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(4),
            UpdatedAt = null,
            DisplayOrder = 5
        },
        new Product
        {
            Id = 6,
            Title = "The Martian",
            Description = "Andy Weir's thrilling sci-fi novel follows astronaut Mark Watney, stranded on Mars after a mission goes awry. With ingenuity and humor, he fights to survive using science and resourcefulness. Praised for its scientific accuracy, this gripping tale of resilience captivates readers.",
            ISBN = "978-0-8041-3902-1",
            Author = "Andy Weir",
            ListPrice = 20.99,
            Price = 18.99,
            Price50 = 17.99,
            Price100 = 16.99,
            CategoryId = 2,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(6),
            UpdatedAt = null,
            DisplayOrder = 6
        },

        // Mystery Books (CategoryId = 3)
        new Product
        {
            Id = 7,
            Title = "The Girl with the Dragon Tattoo",
            Description = "Stieg Larsson's gripping thriller follows journalist Mikael Blomkvist and hacker Lisbeth Salander as they unravel a decades-old mystery of murder and corruption in Sweden. This dark, suspenseful tale of revenge and intrigue became a global phenomenon for its complex characters.",
            ISBN = "978-0-307-45454-1",
            Author = "Stieg Larsson",
            ListPrice = 19.99,
            Price = 17.99,
            Price50 = 16.99,
            Price100 = 15.99,
            CategoryId = 3,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(7),
            UpdatedAt = null,
            DisplayOrder = 7
        },
        new Product
        {
            Id = 8,
            Title = "The Murder of Roger Ackroyd",
            Description = "Agatha Christie's ingenious mystery, featuring Hercule Poirot, unravels the murder of a wealthy man in a quiet English village. Renowned for its shocking twist, this classic redefined the detective genre with its clever narrative and meticulous plotting.",
            ISBN = "978-0-06-207349-4",
            Author = "Agatha Christie",
            ListPrice = 16.99,
            Price = 14.99,
            Price50 = 13.99,
            Price100 = 12.99,
            CategoryId = 3,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(8),
            UpdatedAt = null,
            DisplayOrder = 8
        },
        new Product
        {
            Id = 9,
            Title = "And Then There Were None",
            Description = "Agatha Christie's chilling masterpiece traps ten strangers on an isolated island, where they face a murderer among them. With suspenseful pacing and a brilliant plot, this bestselling mystery keeps readers guessing until the shocking conclusion.",
            ISBN = "978-0-06-207348-7",
            Author = "Agatha Christie",
            ListPrice = 17.99,
            Price = 15.99,
            Price50 = 14.99,
            Price100 = 13.99,
            CategoryId = 3,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(9),
            UpdatedAt = null,
            DisplayOrder = 9
        },

        // Romance Books (CategoryId = 4)
        new Product
        {
            Id = 10,
            Title = "Pride and Prejudice",
            Description = "Jane Austen's timeless romance follows spirited Elizabeth Bennet and wealthy Mr. Darcy as they navigate love, pride, and societal expectations. With sharp wit and insightful social commentary, this beloved classic explores personal growth and enduring love in 19th-century England.",
            ISBN = "978-0-14-143951-8",
            Author = "Jane Austen",
            ListPrice = 15.99,
            Price = 13.99,
            Price50 = 12.99,
            Price100 = 11.99,
            CategoryId = 4,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(10),
            UpdatedAt = null,
            DisplayOrder = 10
        },
        new Product
        {
            Id = 11,
            Title = "A Court of Thorns and Roses",
            Description = "Sarah J. Maas's enchanting romantasy follows Feyre, a human huntress drawn into a magical faerie world. Inspired by Beauty and the Beast, this steamy, action-packed tale blends romance and fantasy, captivating readers with its lush world and complex characters.",
            ISBN = "978-1-61963-693-3",
            Author = "Sarah J. Maas",
            ListPrice = 21.99,
            Price = 19.99,
            Price50 = 18.99,
            CategoryId = 4,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(12),
            UpdatedAt = null,
            DisplayOrder = 11
        },

        // Biography Books (CategoryId = 5)
        new Product
        {
            Id = 12,
            Title = "Unbroken",
            Description = "Laura Hillenbrand's gripping biography chronicles Louis Zamperini's extraordinary life—from Olympic athlete to WWII bombardier, POW, and survivor. This inspiring true story of resilience and redemption captures the indomitable human spirit against unimaginable adversity.",
            ISBN = "978-0-8129-8187-5",
            Author = "Laura Hillenbrand",
            ListPrice = 24.99,
            Price = 22.99,
            Price50 = 21.99,
            Price100 = 20.99,
            CategoryId = 5,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(13),
            UpdatedAt = null,
            DisplayOrder = 12
        },
        new Product
        {
            Id = 13,
            Title = "Steve Jobs",
            Description = "Walter Isaacson's definitive biography reveals the life of Apple co-founder Steve Jobs. Through interviews and unparalleled access, it portrays his brilliance, passion, and flaws, offering an intimate look at the visionary who revolutionized technology and culture.",
            ISBN = "978-1-4516-4853-9",
            Author = "Walter Isaacson",
            ListPrice = 26.99,
            Price = 24.99,
            Price50 = 23.99,
            Price100 = 22.99,
            CategoryId = 5,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(14),
            UpdatedAt = null,
            DisplayOrder = 13
        },

        // Children's Books (CategoryId = 6)
        new Product
        {
            Id = 14,
            Title = "Where the Wild Things Are",
            Description = "Maurice Sendak's beloved children's classic follows Max, a young boy who sails to an island of wild creatures. With stunning illustrations and heartfelt storytelling, this imaginative tale of adventure and belonging captivates readers of all ages.",
            ISBN = "978-0-06-025492-6",
            Author = "Maurice Sendak",
            ListPrice = 12.99,
            Price = 10.99,
            Price50 = 9.99,
            Price100 = 8.99,
            CategoryId = 6,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(16),
            UpdatedAt = null,
            DisplayOrder = 14
        },

        // History Books (CategoryId = 7)
        new Product
        {
            Id = 15,
            Title = "The Rise and Fall of the Third Reich",
            Description = "William L. Shirer's monumental history chronicles Nazi Germany's rise and fall. Drawing on firsthand accounts and extensive research, this definitive work offers a chilling, detailed narrative of Hitler's regime and its catastrophic impact on the world.",
            ISBN = "978-1-4516-4592-7",
            Author = "William L. Shirer",
            ListPrice = 32.99,
            Price = 29.99,
            Price50 = 28.99,
            Price100 = 27.99,
            CategoryId = 7,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(19),
            UpdatedAt = null,
            DisplayOrder = 15
        },
        new Product
        {
            Id = 16,
            Title = "A People's History of the United States",
            Description = "Howard Zinn's groundbreaking history reframes America's past through the lens of ordinary people, minorities, and the oppressed. From Columbus to the present, this influential work challenges traditional narratives, highlighting struggles for justice and equality.",
            ISBN = "978-0-06-083865-2",
            Author = "Howard Zinn",
            ListPrice = 24.99,
            Price = 22.99,
            Price50 = 21.99,
            Price100 = 20.99,
            CategoryId = 7,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(21),
            UpdatedAt = null,
            DisplayOrder = 16
        },

        // Fantasy Books (CategoryId = 8)
        new Product
        {
            Id = 17,
            Title = "The Hobbit",
            Description = "J.R.R. Tolkien's enchanting prelude to The Lord of the Rings follows Bilbo Baggins on an unexpected adventure with dwarves and a wizard. This timeless fantasy, rich with mythical creatures and epic quests, laid the foundation for modern fantasy literature.",
            ISBN = "978-0-547-92822-7",
            Author = "J.R.R. Tolkien",
            ListPrice = 19.99,
            Price = 17.99,
            Price50 = 16.99,
            Price100 = 15.99,
            CategoryId = 8,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(22),
            UpdatedAt = null,
            DisplayOrder = 17
        },
        new Product
        {
            Id = 18,
            Title = "A Game of Thrones",
            Description = "George R.R. Martin's epic fantasy launches A Song of Ice and Fire with intricate political intrigue and vivid characters. In a world where seasons last years, noble families vie for the Iron Throne, unaware of ancient threats awakening. A modern fantasy classic.",
            ISBN = "978-0-553-10354-0",
            Author = "George R.R. Martin",
            ListPrice = 23.99,
            Price = 21.99,
            Price50 = 20.99,
            Price100 = 19.99,
            CategoryId = 8,
            IsHidden = false,
            CreatedAt = baseDate.AddDays(24),
            UpdatedAt = null,
            DisplayOrder = 18
        },


            new Product
            {
                Id = 19,
                Title = "A Dance with Dragons",
                Description = "George R.R. Martin's fifth *A Song of Ice and Fire* novel follows Daenerys Targaryen ruling Meereen with her dragons, while Tyrion Lannister flees Westeros and Jon Snow faces threats at the Wall. Packed with intrigue, betrayal, and epic scope, this installment deepens the saga’s complex narrative.",
                ISBN = "978-0-553-80558-1",
                Author = "George R.R. Martin",
                ListPrice = 23.99,
                Price = 21.99,
                Price50 = 20.99,
                Price100 = 19.99,
                CategoryId = 8,
                IsHidden = false,
                CreatedAt = baseDate.AddDays(25),
                UpdatedAt = null,
                DisplayOrder = 19
            }
        );

    }
}

