// See https://aka.ms/new-console-template for more information
using HidSharp.Utility;
using Jetsparrow.MiniKeyboard;


HidSharpDiagnostics.EnableTracing = true;
HidSharpDiagnostics.PerformStrictChecks = true;


Console.WriteLine("Hello, World!");


var man = new DeviceManager();
var dev = await  man.Connect();

var keyboardVersion = await dev.GetKeyboardVersion();



/*
    ReportID
    KeyType_Num
    KeyGroupCharNum
    KeyGroupCharNum
    Key_Fun_Num
    KEY_Char_Num
    KEY_Char_Num
    ...
*/

//await dev.SendBuffer(Convert.FromHexString("03 A1 02 00 000000000000".Replace(" ", "")));


// 03       01   1__________1     05      00   00    00
// VERSION  KEY  layer1 key+mods  STRLEN  I    MODS  KEY
// strings are sent starting with null at pos zero

Console.WriteLine($"KeyboardVersion {keyboardVersion}");
await SetKey(PhysicalKey.Key1, 0, Convert.FromHexString("0918060E2C"));
await SetKey(PhysicalKey.Key2, 0, Convert.FromHexString("0918060E2C"));
await SetKey(PhysicalKey.Key3, 0, Convert.FromHexString("0918060E2C"));

await SetKey(PhysicalKey.Knob1Left, 0, Convert.FromHexString("0918060E2C"));
await SetKey(PhysicalKey.Knob1Right, 0, Convert.FromHexString("0918060E2C"));
await SetKey(PhysicalKey.Knob1Click, 0, Convert.FromHexString("0918060E2C"));


//await dev.SendBuffer(Convert.FromHexString("03 AA AA 00 000000000000".Replace(" ", "")));

//await dev.SendBuffer(Convert.FromHexString("03 02 00 01 01 000700000000".Replace(" ", "")));
//await dev.SendBuffer(Convert.FromHexString("03 03 00 01 01 000800000000".Replace(" ", "")));




async Task SetKey(PhysicalKey keyNum, KeyModifier mods, byte[] codes)
{
    var buf = new byte[7];
    buf[0] = (byte)keyboardVersion;
    buf[1] = (byte)keyNum;
    buf[2] = 0x11;
    buf[3] = (byte)codes.Length;

    buf[5] = (byte)mods;

    await dev.SendBuffer(buf);

    for (int i = 1; i <= codes.Length; ++i)
    {
        buf[4] = (byte)i;
        buf[6] = (byte)codes[i-1];
        await dev.SendBuffer(buf);
    }
}

