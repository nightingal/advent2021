using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\dev\advantofcode\Day16\input.txt"))
            {
                var rules = new Dictionary<char, List<char>>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    var split = line.Split(" = ");
                    rules.Add(split[0][0], split[1].ToCharArray().ToList());
                }

                var input = reader.ReadLine();
                var binary = new List<char>();

                foreach (var c in input)
                {
                    binary.AddRange(rules[c]);
                }

                var packet = ParsePacket(binary);

                var sum = GetSumOfVersions(packet);

                Console.WriteLine(sum);

                var val = GetPacketValue(packet);

                Console.Write(val);
            }
        }

        static long GetPacketValue(Packet packet)
        {
            var parentPacket = packet as PacketWithSubPacket;

            var childValues = new List<long>();

            if (parentPacket != null)
            {
                foreach (var childPacket in parentPacket.Packets)
                {
                    childValues.Add(GetPacketValue(childPacket));
                }
            }

            var result = (long)0;
            switch (packet.Type)
            {
                case 0:
                    foreach (var sub in childValues)
                    {
                        result += sub;
                    }
                    break;

                case 1:
                    result = 1;
                    foreach (var sub in childValues)
                    {
                        result *= sub;
                    }
                    break;

                case 2:
                    result = childValues[0];
                    foreach (var val in childValues)
                    {
                        if (val < result)
                        {
                            result = val;
                        }
                    }
                    break;

                case 3:
                    result = childValues[0];
                    foreach (var val in childValues)
                    {
                        if (val > result)
                        {
                            result = val;
                        }
                    }
                    break;

                case 4:
                    result = (packet as ValuePacket).Value;
                    break;

                case 5:
                    result = childValues[0] > childValues[1] ? 1 : 0;
                    break;

                case 6:
                    result = childValues[0] < childValues[1] ? 1 : 0;
                    break;

                case 7:
                    result = childValues[0] == childValues[1] ? 1 : 0;
                    break;

                default:
                    throw new Exception("Uknown packet type: " + packet.Type);
            }

            return result;
        }

        static int GetSumOfVersions(Packet rootPacket)
        {
            var toCheck = new Queue<Packet>();
            toCheck.Enqueue(rootPacket);

            var sum = 0;

            while (toCheck.Count > 0)
            {
                var packet = toCheck.Dequeue();

                sum += packet.Version;

                if (packet is PacketWithSubPacket withSubs)
                {
                    foreach (var sub in withSubs.Packets)
                    {
                        toCheck.Enqueue(sub);
                    }
                }
            }

            return sum;
        }

        static Packet ParsePacket(List<char> packet)
        {
            var version = Convert.ToInt32("" + packet[0] + packet[1] + packet[2], 2);
            var type = Convert.ToInt32("" + packet[3] + packet[4] + packet[5], 2);

            if (type == 4)
            {
                // Litteral number packet
                var bits = string.Empty;

                var readIndex = 6;

                while (true)
                {
                    bits += new string(packet.GetRange(readIndex + 1, 4).ToArray());

                    readIndex += 5;
                    if (packet[readIndex - 5] == '0')
                    {
                        break;
                    }
                }

                var value = Convert.ToInt64(bits, 2);
                var totalLength = readIndex;

                return new ValuePacket(version, type, value, totalLength);
            }
            else
            {
                var lengthTypeId = packet[6];
                var readIndex = 7;

                if (lengthTypeId == '0')
                {
                    //If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
                    var subPacketLength = 15;
                    var lengthBits = new string(packet.GetRange(readIndex, subPacketLength).ToArray());
                    var length = Convert.ToInt32(lengthBits, 2);

                    readIndex += subPacketLength;

                    var usedLength = 0;

                    var subPackets = new List<Packet>();

                    while (usedLength < length)
                    {
                        var subPacketContent = packet.GetRange(readIndex, length - usedLength);
                        var subPacket = ParsePacket(subPacketContent);
                        subPackets.Add(subPacket);

                        readIndex += subPacket.TotalLength;
                        usedLength += subPacket.TotalLength;
                    }

                    var parentPacket = new PacketWithSubPacket(version, type, readIndex);

                    foreach (var sub in subPackets)
                    {
                        parentPacket.Add(sub);
                    }

                    return parentPacket;
                }
                else
                {
                    //If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
                    var subPacketLength = 11;
                    var subPacketNumberBits = new string(packet.GetRange(readIndex, subPacketLength).ToArray());
                    var subPacketCount = Convert.ToInt32(subPacketNumberBits, 2);

                    readIndex += subPacketLength;

                    var subPackets = new List<Packet>();

                    for (var i = 0; i < subPacketCount; ++i)
                    {
                        var subPacket = ParsePacket(packet.GetRange(readIndex, packet.Count - readIndex));
                        subPackets.Add(subPacket);

                        readIndex += subPacket.TotalLength;
                    }

                    var parentPacket = new PacketWithSubPacket(version, type, readIndex);
                    parentPacket.Add(subPackets);

                    return parentPacket;
                }
            }

            throw new Exception("We should never reach this point");
        }

        private abstract class Packet
        {
            public Packet(int version, int type, int totalLength)
            {
                this.Version = version;
                this.Type = type;
                this.TotalLength = totalLength;
            }

            public int Version { get; }
            public int Type { get; }
            public int TotalLength { get; }
        }

        private class ValuePacket : Packet
        {
            public ValuePacket(int version, int type, long value, int totalLength) : base(version, type, totalLength)
            {
                this.Value = value;
            }

            public long Value { get; }
        }

        private class PacketWithSubPacket : Packet
        {
            public PacketWithSubPacket(int version, int type, int totalLength) : base(version, type, totalLength)
            {
                this.Packets = new List<Packet>();
            }

            public List<Packet> Packets { get; }

            public void Add(Packet packet)
            {
                this.Packets.Add(packet);
            }

            public void Add(List<Packet> packets)
            {
                this.Packets.AddRange(packets);
            }
        }
    }
}
