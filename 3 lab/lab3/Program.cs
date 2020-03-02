using System;
using System.Threading;
namespace Lab3
{
    class Program
    {
        private static string criticalSectionFile = "CriticalSections.txt";
        private static string eventsFile = "Events.txt";
        static void Main(string[] args)
        {


            // Menu for main application console screen:
            while (true)
            {
                Console.WriteLine(
                    "\n1: Critical Sections\n" +
                    "2: Mutual Exclusions\n" +
                    "3: Events\n"+
                    "4: Exit app\n"
                    );
                int pressedKey = Int32.Parse(Console.ReadLine());
                switch (pressedKey)
                {

                    //Critical Sections case:
                    case 1:
                        //Call the CriticalSection class:
                        CriticalSectionExample writer = new CriticalSectionExample(criticalSectionFile);

                        //Initialization writers:
                        Thread th1 = new Thread(writer.WriteA);
                        Thread th2 = new Thread(writer.WriteB);
                        th1.Start(); th2.Start();

                        //Abort writers:
                        Console.WriteLine("Press the key to cancel");
                        Console.ReadKey();
                        th1.Abort(); th2.Abort();
                        break;

                    //Mutex case:
                    case 2:

                        //Initialize the system Mutex method:
                        bool isOpened;
                        Mutex mutex = new Mutex(true, "isOpened", out isOpened);

                        //Check if one copy is open:                 
                        if (isOpened)
                        {
                            Console.WriteLine("The application is open in a single copy");
                            Console.WriteLine("Press the key to exit");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("The application is already open. Exit in few sec");
                            Thread.Sleep(3000);
                            return;
                        }
                        break;

                    //Events case:
                    case 3:
                        EventExample ee = new EventExample(eventsFile);
                        //
                        ee.Added += ee.HandlerMessage;
                        ee.Deleted += ee.HandlerMessage;
                        //
                        for (int i = 0; i < 4; i++)
                        {
                            ee.Add();
                            Thread.Sleep(1000);
                        }
                        ee.Remove();
                        
                        //Exit:
                        Console.WriteLine("Press the key to exit");
                        Console.ReadKey();
                        break;


                    //Interface features for casing:
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong key");
                        break;
                }
            }
        }
    }
    //Critical Section class:
    class CriticalSectionExample
    {
        private string fileName;

        //Create text file:
        public CriticalSectionExample(string fileName)
        {
            this.fileName = fileName;
        }
        private void Write(string ch)
        {
            lock (this)
            {
                System.IO.File.AppendAllText(fileName, ch);
            }
        }
        //Filling up:
        public void WriteA()
        {
            for (; ; ) Write("0");
        }
        public void WriteB()
        {
            for (; ; ) Write("1");
        }
    }
    //Events class:
    class EventExample
    {
        public delegate void FileContentHandler(string msg);
        public event FileContentHandler Added;
        public event FileContentHandler Deleted;
        private string fileName;
        
       //Creating:
       public EventExample(string fileName)
       {
         this.fileName = fileName;
       }

        //Add the row to the text file:
        public void Add()
        {
            System.IO.File.AppendAllText(fileName, "Added ");
            if (Added != null)
                Added("Added a row");
        }

        //Remove and change the row from the text file:
        public void Remove()
        {
            string file = System.IO.File.ReadAllText(fileName);
            int i = file.IndexOf("Added");
          
            //Changing first word as "Dedda"
            string result = file.Remove(i, "Added".Length).Insert(i, "Dedda");
            System.IO.File.WriteAllText(fileName, result);
            if (Deleted != null)
            Deleted("Row deleted and changing");
        }

        //View the status in console:
        public void HandlerMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}