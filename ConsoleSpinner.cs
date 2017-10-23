using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    internal class ConsoleSpinner : IDisposable
    {
        private const string Sequence = @"/-\|";
        private int counter = 0;
        private readonly int left;
        private readonly int top;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;

        public ConsoleSpinner(int left = 10, int top = 0, int delay = 100)
        {
            this.left = left;
            this.top = top;
            this.delay = delay;
            thread = new Thread(Spin);
        }

        public void Start()
        {
            active = true;
            if (!thread.IsAlive)
            {
                Console.Write("Working... ");
                thread.Start();
            }
        }

        public void Stop()
        {
            active = false;
            Draw(' ');
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);
            }
        }

        private void Draw(char c)
        {
            Console.SetCursorPosition(left, top);
            Console.Write(c);
        }

        private void Turn()
        {
            Draw(Sequence[++counter % Sequence.Length]);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
