﻿using System.Text.RegularExpressions;

namespace CoincheServer
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using DotNetty.Transport.Channels;
    using DotNetty.Transport.Channels.Groups;

    public class ServerHandler : SimpleChannelInboundHandler<string>
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
            poker.AddPlayer(poker.Player, contex.Channel.RemoteAddress.ToString());
            //contex.WriteAndFlushAsync(g.Count);
            if (poker.Player == 4)
            {
                contex.WriteAndFlushAsync(string.Format("Welcome to the game!\n"));
                group.WriteAndFlushAsync("Welcome to the game\n", new EveryOneBut(contex.Channel.Id));
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
                //group.WriteAndFlushAsync(broadcast, new EveryOneBut(contex.Channel.Id));
                response = poker.LaunchPoker(msg.Trim().ToUpper(), contex.Channel.RemoteAddress.ToString());
                if (response.StartsWith("ACTION:"))
                    group.WriteAndFlushAsync(response, new EveryOneBut(contex.Channel.Id));
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