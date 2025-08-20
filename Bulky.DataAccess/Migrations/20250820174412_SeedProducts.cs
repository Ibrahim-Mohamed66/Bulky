using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedAt", "Description", "DisplayOrder", "ISBN", "ImageUrl", "IsHidden", "ListPrice", "Price", "Price100", "Price50", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Harper Lee's Pulitzer Prize-winning novel explores racial injustice and moral growth in the American South. Through young Scout Finch's eyes, the story of her father, Atticus, defending a Black man unjustly accused, reveals themes of empathy, courage, and humanity. A timeless classic celebrated for its powerful narrative.", 1, "978-0-06-112008-4", "\\images\\products\\daa24267-8df5-4c8d-b923-11ff134e4994.jpg", false, 18.989999999999998, 16.989999999999998, 14.99, 15.99, "To Kill a Mockingbird", null },
                    { 2, "F. Scott Fitzgerald", 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "F. Scott Fitzgerald's iconic novel captures the Jazz Age's extravagance and disillusionment. Jay Gatsby's obsessive love for Daisy Buchanan and his pursuit of the American Dream unravel in a tragic tale of wealth, ambition, and unattainable desires, set against a backdrop of opulent Long Island parties.", 2, "978-0-7432-7356-5", "\\images\\products\\f460667e-d6b1-4eaa-b98f-371678bc64a8.jpg", false, 16.989999999999998, 14.99, 12.99, 13.99, "The Great Gatsby", null },
                    { 3, "George Orwell", 1, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "George Orwell's chilling dystopian novel depicts a totalitarian world of surveillance and control. Winston Smith's rebellion against the oppressive Party and Big Brother explores themes of freedom, truth, and individuality. A haunting, prophetic masterpiece that remains a cornerstone of modern literature.", 3, "978-0-452-28423-4", "\\images\\products\\b8fd5d61-67ff-4058-beb7-d82d833d2aa4.jpg", false, 19.989999999999998, 17.989999999999998, 15.99, 16.989999999999998, "1984", null },
                    { 4, "Frank Herbert", 2, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Frank Herbert's epic sci-fi saga follows young Paul Atreides on the desert planet Arrakis, where the valuable spice melange shapes destinies. Blending politics, religion, and ecology, this richly detailed masterpiece explores power and survival in a gripping, universe-spanning narrative.", 4, "978-0-441-17271-9", "\\images\\products\\d2497e8a-81f5-4472-a671-eee13524dd2b.jpg", false, 22.989999999999998, 20.989999999999998, 18.989999999999998, 19.989999999999998, "Dune", null },
                    { 5, "Orson Scott Card", 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Orson Scott Card's gripping sci-fi novel follows young Ender Wiggin, a brilliant strategist trained to lead humanity against an alien threat. Combining intense military tactics with deep moral questions, this coming-of-age tale explores leadership, sacrifice, and the cost of war.", 5, "978-0-8125-5070-2", "\\images\\products\\8244510a-91c2-4cbe-90bf-7b6bf38783a5.jpg", false, 18.989999999999998, 16.989999999999998, 14.99, 15.99, "Ender's Game", null },
                    { 6, "Andy Weir", 2, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Andy Weir's thrilling sci-fi novel follows astronaut Mark Watney, stranded on Mars after a mission goes awry. With ingenuity and humor, he fights to survive using science and resourcefulness. Praised for its scientific accuracy, this gripping tale of resilience captivates readers.", 6, "978-0-8041-3902-1", "\\images\\products\\c5d93804-05cc-4403-8049-1577bd6ef496.jpg", false, 20.989999999999998, 18.989999999999998, 16.989999999999998, 17.989999999999998, "The Martian", null },
                    { 7, "Stieg Larsson", 3, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Stieg Larsson's gripping thriller follows journalist Mikael Blomkvist and hacker Lisbeth Salander as they unravel a decades-old mystery of murder and corruption in Sweden. This dark, suspenseful tale of revenge and intrigue became a global phenomenon for its complex characters.", 7, "978-0-307-45454-1", "\\images\\products\\243af559-d40c-42da-99b4-1ed75ee96d1d.jpg", false, 19.989999999999998, 17.989999999999998, 15.99, 16.989999999999998, "The Girl with the Dragon Tattoo", null },
                    { 8, "Agatha Christie", 3, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Agatha Christie's ingenious mystery, featuring Hercule Poirot, unravels the murder of a wealthy man in a quiet English village. Renowned for its shocking twist, this classic redefined the detective genre with its clever narrative and meticulous plotting.", 8, "978-0-06-207349-4", "\\images\\products\\34507dbf-7ceb-43df-bab8-32d5e4c18cdb.jpg", false, 16.989999999999998, 14.99, 12.99, 13.99, "The Murder of Roger Ackroyd", null },
                    { 9, "Agatha Christie", 3, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Agatha Christie's chilling masterpiece traps ten strangers on an isolated island, where they face a murderer among them. With suspenseful pacing and a brilliant plot, this bestselling mystery keeps readers guessing until the shocking conclusion.", 9, "978-0-06-207348-7", "\\images\\products\\7e58b144-b584-42d4-9c17-09b9de15402f.jpg", false, 17.989999999999998, 15.99, 13.99, 14.99, "And Then There Were None", null },
                    { 10, "Jane Austen", 4, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), "Jane Austen's timeless romance follows spirited Elizabeth Bennet and wealthy Mr. Darcy as they navigate love, pride, and societal expectations. With sharp wit and insightful social commentary, this beloved classic explores personal growth and enduring love in 19th-century England.", 10, "978-0-14-143951-8", "\\images\\products\\565b71ed-12d3-4056-aaae-44664636c40c.jpg", false, 15.99, 13.99, 11.99, 12.99, "Pride and Prejudice", null },
                    { 11, "Sarah J. Maas", 4, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), "Sarah J. Maas's enchanting romantasy follows Feyre, a human huntress drawn into a magical faerie world. Inspired by Beauty and the Beast, this steamy, action-packed tale blends romance and fantasy, captivating readers with its lush world and complex characters.", 11, "978-1-61963-693-3", "\\images\\products\\d6ae3472-3de8-4c66-97e8-52fc98c821f6.jpg", false, 21.989999999999998, 19.989999999999998, 17.989999999999998, 18.989999999999998, "A Court of Thorns and Roses", null },
                    { 12, "Laura Hillenbrand", 5, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Laura Hillenbrand's gripping biography chronicles Louis Zamperini's extraordinary life—from Olympic athlete to WWII bombardier, POW, and survivor. This inspiring true story of resilience and redemption captures the indomitable human spirit against unimaginable adversity.", 12, "978-0-8129-8187-5", "\\images\\products\\85c0c030-e4a4-4d2f-8e51-ab6f5918031e.jpg", false, 24.989999999999998, 22.989999999999998, 20.989999999999998, 21.989999999999998, "Unbroken", null },
                    { 13, "Walter Isaacson", 5, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Walter Isaacson's definitive biography reveals the life of Apple co-founder Steve Jobs. Through interviews and unparalleled access, it portrays his brilliance, passion, and flaws, offering an intimate look at the visionary who revolutionized technology and culture.", 13, "978-1-4516-4853-9", "\\images\\products\\daa61ac4-ed64-469b-8288-d1dfd3a0e77d.jpg", false, 26.989999999999998, 24.989999999999998, 22.989999999999998, 23.989999999999998, "Steve Jobs", null },
                    { 14, "Maurice Sendak", 6, new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), "Maurice Sendak's beloved children's classic follows Max, a young boy who sails to an island of wild creatures. With stunning illustrations and heartfelt storytelling, this imaginative tale of adventure and belonging captivates readers of all ages.", 14, "978-0-06-025492-6", "\\images\\products\\7f1d85b4-625d-4575-ab84-effd3ad5188b.jpg", false, 12.99, 10.99, 8.9900000000000002, 9.9900000000000002, "Where the Wild Things Are", null },
                    { 15, "William L. Shirer", 7, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "William L. Shirer's monumental history chronicles Nazi Germany's rise and fall. Drawing on firsthand accounts and extensive research, this definitive work offers a chilling, detailed narrative of Hitler's regime and its catastrophic impact on the world.", 15, "978-1-4516-4592-7", "\\images\\products\\0fbcafb1-4f9b-43b0-b5f9-23e65a126913.jpg", false, 32.990000000000002, 29.989999999999998, 27.989999999999998, 28.989999999999998, "The Rise and Fall of the Third Reich", null },
                    { 16, "Howard Zinn", 7, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Howard Zinn's groundbreaking history reframes America's past through the lens of ordinary people, minorities, and the oppressed. From Columbus to the present, this influential work challenges traditional narratives, highlighting struggles for justice and equality.", 16, "978-0-06-083865-2", "\\images\\products\\9595b365-d06d-4d3b-bedd-42c558e1eb39.jpg", false, 24.989999999999998, 22.989999999999998, 20.989999999999998, 21.989999999999998, "A People's History of the United States", null },
                    { 17, "J.R.R. Tolkien", 8, new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), "J.R.R. Tolkien's enchanting prelude to The Lord of the Rings follows Bilbo Baggins on an unexpected adventure with dwarves and a wizard. This timeless fantasy, rich with mythical creatures and epic quests, laid the foundation for modern fantasy literature.", 17, "978-0-547-92822-7", "\\images\\products\\c73a8bd9-f43b-41b5-b057-2de625f4772f.jpg", false, 19.989999999999998, 17.989999999999998, 15.99, 16.989999999999998, "The Hobbit", null },
                    { 18, "George R.R. Martin", 8, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "George R.R. Martin's epic fantasy launches A Song of Ice and Fire with intricate political intrigue and vivid characters. In a world where seasons last years, noble families vie for the Iron Throne, unaware of ancient threats awakening. A modern fantasy classic.", 18, "978-0-553-10354-0", "\\images\\products\\8e239aa0-19e0-4519-9a58-b7fb9015263b.jpeg", false, 23.989999999999998, 21.989999999999998, 19.989999999999998, 20.989999999999998, "A Game of Thrones", null },
                    { 19, "George R.R. Martin", 8, new DateTime(2024, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), "George R.R. Martin's fifth *A Song of Ice and Fire* novel follows Daenerys Targaryen ruling Meereen with her dragons, while Tyrion Lannister flees Westeros and Jon Snow faces threats at the Wall. Packed with intrigue, betrayal, and epic scope, this installment deepens the saga’s complex narrative.", 19, "978-0-553-80558-1", "\\images\\products\\56fa1605-1471-4837-9d3a-4cc98e80c27f.jpg", false, 23.989999999999998, 21.989999999999998, 19.989999999999998, 20.989999999999998, "A Dance with Dragons", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);
        }
    }
}
