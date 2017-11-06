namespace CoincheServer
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Groups;

    public class TelnetServerHandler : SimpleChannelInboundHandler<string>
    {
        static volatile IChannelGroup group;
        static volatile PokerManager poker = new PokerManager();


        class EveryOneBut : IChannelMatcher
        {
            readonly IChannelId id;

            public EveryOneBut(IChannelId id)
            {
                this.id = id;
            }

            public bool Matches(IChannel channel) => channel.Id != this.id;
        }

        public override void ChannelActive(IChannelHandlerContext contex)
        {
            IChannelGroup g = group;

            if (g == null)
            {
                lock (this)
                {
                    if (group == null)
                    {
                        g = group = new DefaultChannelGroup(contex.Executor);
                    }
                }
            }
            g.Add(contex.Channel);
            poker.Player += 1;
            //contex.WriteAndFlushAsync(g.Count);
            if (poker.Player == 2)
            {
                contex.WriteAndFlushAsync(string.Format("Welcome to the game secure chat server!\n"));
                group.WriteAndFlushAsync("Vilcome to the game\n", new EveryOneBut(contex.Channel.Id));
                poker.IsGameStarted = true;
            }
            Console.WriteLine(poker.Player);
        }

        protected override void ChannelRead0(IChannelHandlerContext contex, string msg)
        {
            string response;
            bool close = false;

            string broadcast = string.Format("[{0}] {1}\n", contex.Channel.RemoteAddress, msg);

            if (string.IsNullOrEmpty(msg))
            {
                response = "Please type something.\r\n";
            }
            else if (string.Equals("bye", msg, StringComparison.OrdinalIgnoreCase))
            {
                response = "Have a good day!\r\n";
                close = true;
            }
            else if (poker.IsGameStarted == false)
            {
                response = "Waiting for player\r\n";
            }
            else
            {
                group.WriteAndFlushAsync(broadcast, new EveryOneBut(contex.Channel.Id));
                poker.launchPoker();
                response = "";
            }

            Task wait_close = contex.WriteAndFlushAsync(response);
            if (close)
            {
                Task.WaitAll(wait_close);
                contex.CloseAsync();
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext contex)
        {
            contex.Flush();
        }

        public override void ExceptionCaught(IChannelHandlerContext contex, Exception e)
        {
            Console.WriteLine("{0}", e.StackTrace);
            contex.CloseAsync();
        }

        public override bool IsSharable => true;
    }
}