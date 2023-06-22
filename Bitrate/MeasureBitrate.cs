using System.Dynamic;
using Newtonsoft.Json;

public class NICData
{
  public string Description { get; set; }
  public string MAC { get; set; }
  public DateTime Timestamp { get; set; }
  public ulong Rx { get; set; }
  public ulong Tx { get; set; }
}

public class DeviceData
{
  public string Device { get; set; }
  public string Model { get; set; }
  public List<NICData> NIC { get; set; }
}

public class Bitrate
{
  public ulong Rx { get; set; }
  public ulong Tx { get; set; }
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}

public class MeasureBitrate
{
  // Keep track of the samples per NIC.
  private Dictionary<String, List<NICData>> nics = new();
  private double rate;

  public MeasureBitrate(double rate)
  {
    this.rate = rate;
  }

  private static DeviceData? Parse(String data)
  {
    return JsonConvert.DeserializeObject<DeviceData>(data);
  }

  public void AddSample(DeviceData sample)
  {
    foreach (var nic in sample.NIC)
    {
      if (!this.nics.ContainsKey(nic.MAC))
      {
        var samples = new List<NICData>
        {
            nic
        };

        this.nics.Add(nic.MAC, samples);
      }
      else
      {
        this.nics[nic.MAC].Add(nic);
      }
    }
  }

  public void AddSampleFromString(String sample)
  {
    var parsed = Parse(sample);
    if (parsed == null)
    {
      throw new Exception("Failed to parse JSON");
    }

    this.AddSample(parsed);
  }

  public ulong GetBitrate(ulong a, ulong b, double seconds)
  {
    unchecked
    {
      // Not clear what the API's maximum integer value is, assuming 32-bit integer here
      // for the wraparound to make sense.
      return (ulong)Math.Round(((uint)((uint)b - (uint)a) * 8) / seconds * this.rate);
    }
  }

  // Calculate the bitrates given all previous samples. The returning data
  // is grouped by the NIC's MAC address. 
  public Dictionary<String, List<Bitrate>> Calculate()
  {
    var nicBitrates = new Dictionary<String, List<Bitrate>>();

    foreach (var samples in this.nics)
    {
      var result = new List<Bitrate>();
      if (samples.Value.Count <= 1)
      {
        throw new Exception("Expected more than 1 sample to calculate the bitrate");
      }

      for (int sampleIndex = 1; sampleIndex < samples.Value.Count; sampleIndex++)
      {
        var sample0 = samples.Value[sampleIndex - 1];
        var sample1 = samples.Value[sampleIndex];

        var seconds = (sample1.Timestamp - sample0.Timestamp).TotalSeconds;
        result.Add(new Bitrate
        {
          Tx = GetBitrate(sample0.Tx, sample1.Tx, seconds),
          Rx = GetBitrate(sample0.Rx, sample1.Rx, seconds),
          Start = sample0.Timestamp,
          End = sample1.Timestamp,
        });
      }

      nicBitrates.Add(samples.Key, result);
    }

    return nicBitrates;
  }
}