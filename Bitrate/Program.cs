var measure = new MeasureBitrate(2.0);
measure.AddSampleFromString(@"{
    ""Device"": ""Arista"",
    ""Model"": ""X-Video"",
    ""NIC"": [{
        ""Description"": ""Linksys ABR"",
        ""MAC"": ""14:91:82:3C:D6:7D"",
        ""Timestamp"": ""2023-03-23T18:25:43.511Z"",
        ""Rx"": ""3698574500"",
        ""Tx"": ""122558800""
    }]
}");

measure.AddSampleFromString(@"{
    ""Device"": ""Arista"",
    ""Model"": ""X-Video"",
    ""NIC"": [{
        ""Description"": ""Linksys ABR"",
        ""MAC"": ""14:91:82:3C:D6:7D"",
        ""Timestamp"": ""2023-03-23T18:30:44.511Z"",
        ""Rx"": ""412500056"",
        ""Tx"": ""180106500""
    }]
}");

measure.AddSampleFromString(@"{
    ""Device"": ""Arista"",
    ""Model"": ""X-Video"",
    ""NIC"": [{
        ""Description"": ""Linksys ABR"",
        ""MAC"": ""14:91:82:3C:D6:7D"",
        ""Timestamp"": ""2023-03-23T18:31:44.511Z"",
        ""Rx"": ""812500056"",
        ""Tx"": ""180106500""
    }]
}");

var calculations = measure.Calculate();
PrintBitrates.Print(calculations, ByteFormat.KiB);