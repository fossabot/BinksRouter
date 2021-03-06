﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BinksRouter.Extensions;

namespace BinksRouter.Network.Entities
{
    class RipPacket
    {
        #region Subclasses

        public enum RipCommand
        {
            Request = 1,
            Response = 2
        }

        public class RipRecord
        {
            public readonly IPAddress IpAddress;
            public readonly IPAddress Mask;
            public readonly IPAddress NextHop;

            public RipRecord(byte[] payload)
            {

                IpAddress = new IPAddress(payload.Skip(4).Take(4).ToArray());
                Mask = new IPAddress(payload.Skip(8).Take(4).ToArray());
                NextHop = new IPAddress(payload.Skip(12).Take(4).ToArray());
            }

            public override string ToString()
            {
                return $"{IpAddress}/{Mask.ToShortMask()} -> {NextHop}";
            }
        }
        #endregion

        public readonly char Version;
        public readonly RipCommand Command;
        public readonly List<RipRecord> Records = new List<RipRecord>();

        public RipPacket(byte[] payload)
        {
            Command = (RipCommand)Convert.ToChar(payload[0]);
            Version = Convert.ToChar(payload[1]);

            var current = 4;
            while (current < payload.Length)
            {
                Records.Add(new RipRecord(
                    payload.Skip(current).Take(20).ToArray()
                ));
                current += 20;
            }
        }
    }
}
