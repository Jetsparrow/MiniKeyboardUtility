using HidSharp;
using HidSharp.Reports;

namespace Jetsparrow.MiniKeyboard;

public class MiniKeyboardDevice
{
    public MiniKeyboardDevice(ProductDescr descr, HidDevice device, HidStream stream)
    {
        Descr = descr;
        Device = device;
        Stream = stream;
    }

    public ProductDescr Descr { get; }
    public int ProtocolType => Descr.ProtocolType;
    HidDevice Device { get; }
    HidStream Stream { get; }

    public async Task SendBuffer(ReadOnlyMemory<byte> report)
    {
        Console.WriteLine($"Sending {Convert.ToHexString(report.Span)}");
        var SendBuf = new byte[65];
        report.CopyTo(SendBuf);
        Stream.Write(SendBuf);
        Stream.Flush();
    }


    public async Task<int> GetKeyboardVersion()
    {
        byte[] buf = new byte[65];

        for (byte i = 0; i < 10; ++i)
        {
            try
            {
                buf[0] = i; 
                await SendBuffer(buf);
                return i;
            }
            catch { }
        }
        throw new NotImplementedException();
    }
}

public partial class DeviceManager
{
    public async Task<MiniKeyboardDevice?> Connect()
	{
        await Task.Yield();
        
        var compat = GetCompatibleDevice();
        if (compat is null)
            return null;

        (var dev, var descr) = compat.Value;

        var stream = dev.Open();

        return new MiniKeyboardDevice(descr, dev, stream);
	}

    (HidDevice device, ProductDescr descr)? GetCompatibleDevice()
    {
        var list = DeviceList.Local;
        var compatDeviceList = list.GetAllDevices()
            .OfType<HidDevice>()
            .ToList();

        foreach (var dev in compatDeviceList)
        {
            var id = (dev.VendorID, dev.ProductID);
            if (!ProductRegistry.ProductsById.TryGetValue(id, out var descr))
                continue;
            if (dev.DevicePath.Contains(descr.SearchPathSegment))
                return (dev, descr);
        }
        return null;
    }
}
