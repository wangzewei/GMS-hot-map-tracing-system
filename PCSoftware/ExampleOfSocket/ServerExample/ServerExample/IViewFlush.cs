using System.Net.Sockets;

namespace ServerExample
{
    public interface IViewFlush
    {
        void viewFlushmethod(string Msg, NetworkStream ns);
    }
}
