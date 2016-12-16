using System;

namespace ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            start();

            Console.ReadLine();
        }

        private async static void start()
        {
            TodoClient client = new TodoClient();

            var items = await client.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("****************");

            await client.Add(new Models.TodoItem() { Name = "Do Stuff" });

            items = await client.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("****************");

            await client.Update(items[0].Key, new Models.TodoItem() { Key = items[0].Key, Name = items[0].Name, IsComplete = true });

            items = await client.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("****************");

            await client.Delete(items[0].Key);

            items = await client.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
