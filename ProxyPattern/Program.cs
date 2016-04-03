using System;

namespace ProxyPattern
{
    class SubjectAccessor // encapsulates Subject and Proxy
    {
        public interface ISubject
        {
            string Request();
        }

        private class Subject // Private class so client cannot get to subject except via Proxy
        {
            public string Request()
            {
                return "Subject Request " + "Choose left door\n";
            }
        }

        public class Proxy : ISubject
        {
            Subject subject;

            public string Request()
            {
                // A Virtual Proxy creates the object only on its first method call
                if (subject == null)
                {
                    Console.WriteLine("Subject inactive");
                    subject = new Subject();
                }
                Console.WriteLine("Subject active");
                return "Proxy: Call to " + subject.Request(); // calls the actual request method
            }
        }

        public class ProtectionProxy : ISubject
        {
            // An Authentication Proxy first asks for a password
            Subject subject;
            string password = "Abracadabra";

            public string Authenticate(string supplied) // instantiates a new subject when password matches
            {
                if (supplied != password)
                    return "Protection Proxy: No access";
                else
                    subject = new Subject();
                return "Protection Proxy: Authenticated";
            }

            public string Request() // requires the user to authenticate first
            {
                if (subject == null)
                    return "Protection Proxy: Authenticate first";
                else
                    return "Protection Proxy: Call to " + subject.Request();
            }
        }
    }


    class Client : SubjectAccessor
    {
        static void Main()
        {
            Console.WriteLine("Proxy Pattern\n");

            ISubject subject = new Proxy(); // Client has a ISubject, that will be used to switch proxies

            Console.WriteLine(subject.Request());
            Console.WriteLine(subject.Request());

            subject = new ProtectionProxy();
            Console.WriteLine(subject.Request()); // calls the Proxy.Request()
            Console.WriteLine((subject as ProtectionProxy).Authenticate("Secret")); // subject is declared as ISubject interface
            Console.WriteLine((subject as ProtectionProxy).Authenticate("Abracadabra"));
            Console.WriteLine(subject.Request()); // calls the Proxy.Request() which calls the Subject.Request()

            Console.ReadKey();
        }
    }
}