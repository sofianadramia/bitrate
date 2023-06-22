public enum ByteFormat
{
  Bits,
  KiB,
  MiB,
}

class PrintBitrates
{
  public static void Print(Dictionary<String, List<Bitrate>> samples, ByteFormat format)
  {
    foreach (var entry in samples)
    {
      Console.WriteLine("NIC: " + entry.Key);
      foreach (var bitrate in entry.Value)
      {
        Console.WriteLine(String.Format("  Tx: {0}  Rx: {1} - {2}", FormatBytes(format, bitrate.Tx), FormatBytes(format, bitrate.Rx),
          bitrate.End - bitrate.Start
        ));
      }
    }
  }

  private static String FormatBytes(ByteFormat format, ulong bits)
  {
    switch (format)
    {
      case ByteFormat.MiB:
        return (bits / 8 / 1024 / 1024).ToString() + "Mbps";
      case ByteFormat.KiB:
        return (bits / 8 / 1024).ToString() + "Kbps";
      case ByteFormat.Bits:
      default:
        return bits.ToString() + "bps";
    }
  }
}