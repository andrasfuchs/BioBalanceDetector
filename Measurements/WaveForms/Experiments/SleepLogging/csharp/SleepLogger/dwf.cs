using System.Runtime.InteropServices;

public class dwf
{
    public const int hdwfNone = 0;

    // device enumeration filters
    public const int enumfilterAll            = 0;
    public const int enumfilterEExplorer      = 1;
    public const int enumfilterDiscovery      = 2;
    public const int enumfilterDiscovery2     = 3;
    public const int enumfilterDDiscovery     = 4;
    public const int enumfilterSaluki         = 6;

    // device ID
    public const int devidEExplorer    = 1;
    public const int devidDiscovery    = 2;
    public const int devidDiscovery2   = 3;
    public const int devidDDiscovery   = 4;
    public const int devidSaluki       = 6;

    // device version
    public const int devverEExplorerC     = 2;
    public const int devverEExplorerE     = 4;
    public const int devverEExplorerF     = 5;
    public const int devverDiscoveryA     = 1;
    public const int devverDiscoveryB     = 2;
    public const int devverDiscoveryC     = 3;

    // trigger source
    public const byte trigsrcNone                 = 0;
    public const byte trigsrcPC                   = 1;
    public const byte trigsrcDetectorAnalogIn     = 2;
    public const byte trigsrcDetectorDigitalIn    = 3;
    public const byte trigsrcAnalogIn             = 4;
    public const byte trigsrcDigitalIn            = 5;
    public const byte trigsrcDigitalOut           = 6;
    public const byte trigsrcAnalogOut1           = 7;
    public const byte trigsrcAnalogOut2           = 8;
    public const byte trigsrcAnalogOut3           = 9;
    public const byte trigsrcAnalogOut4           = 10;
    public const byte trigsrcExternal1            = 11;
    public const byte trigsrcExternal2            = 12;
    public const byte trigsrcExternal3            = 13;
    public const byte trigsrcExternal4            = 14;
    public const byte trigsrcHigh                 = 15;
    public const byte trigsrcLow                  = 16;

    // instrument states:
    public const byte DwfStateReady          = 0;
    public const byte DwfStateConfig         = 4;
    public const byte DwfStatePrefill        = 5;
    public const byte DwfStateArmed          = 1;
    public const byte DwfStateWait           = 7;
    public const byte DwfStateTriggered      = 3;
    public const byte DwfStateRunning        = 3;
    public const byte DwfStateDone           = 2;

    //
    public const byte DECIAnalogInChannelCount   = 1;
    public const byte DECIAnalogOutChannelCount   = 2;
    public const byte DECIAnalogIOChannelCount   = 3;
    public const byte DECIDigitalInChannelCount   = 4;
    public const byte DECIDigitalOutChannelCount   = 5;
    public const byte DECIDigitalIOChannelCount   = 6;
    public const byte DECIAnalogInBufferSize   = 7;
    public const byte DECIAnalogOutBufferSize   = 8;
    public const byte DECIDigitalInBufferSize   = 9;
    public const byte DECIDigitalOutBufferSize   = 10;

    // acquisition modes:
    public const byte acqmodeSingle       = 0;
    public const byte acqmodeScanShift    = 1;
    public const byte acqmodeScanScreen   = 2;
    public const byte acqmodeRecord       = 3;
    public const byte acqmodeOvers        = 4;
    public const byte acqmodeSingle1      = 5;

    // analog acquisition filter:
    public const byte filterDecimate   = 0;
    public const byte filterAverage    = 1;
    public const byte filterMinMax     = 2;

    // analog in trigger mode:
    public const byte trigtypeEdge           = 0;
    public const byte trigtypePulse          = 1;
    public const byte trigtypeTransition     = 2;

    // trigger slope:
    public const byte DwfTriggerSlopeRise     = 0;
    public const byte DwfTriggerSlopeFall     = 1;
    public const byte DwfTriggerSlopeEither   = 2;

    // trigger length condition
    public const byte triglenLess         = 0;
    public const byte triglenTimeout      = 1;
    public const byte triglenMore         = 2;

    // error codes for the public static extern ints:
    public const int dwfercNoErc = 0;        //  No error occurred
    public const int dwfercUnknownError = 1 ;       //  API waiting on pending API timed out
    public const int dwfercApiLockTimeout = 2;        //  API waiting on pending API timed out
    public const int dwfercAlreadyOpened = 3;        //  Device already opened
    public const int dwfercNotSupported = 4;        //  Device not supported
    public const int dwfercInvalidParameter0 = 0x10;     //  Invalid parameter sent in API call
    public const int dwfercInvalidParameter1 = 0x11;     //  Invalid parameter sent in API call
    public const int dwfercInvalidParameter2 = 0x12;     //  Invalid parameter sent in API call
    public const int dwfercInvalidParameter3 = 0x13;     //  Invalid parameter sent in API call
    public const int dwfercInvalidParameter4 = 0x14;     //  Invalid parameter sent in API call

    // analog out signal types
    public const byte funcDC = 0;
    public const byte funcSine = 1;
    public const byte funcSquare = 2;
    public const byte funcTriangle = 3;
    public const byte funcRampUp = 4;
    public const byte funcRampDown = 5;
    public const byte funcNoise = 6;
    public const byte funcPulse = 7;
    public const byte funcTrapezium = 8;
    public const byte funcSinePower = 9;
    public const byte funcCustom = 30;
    public const byte funcPlay = 31;

    // analog io channel node types
    public const byte analogioEnable = 1;
    public const byte analogioVoltage = 2;
    public const byte analogioCurrent = 3;
    public const byte analogioPower = 4;
    public const byte analogioTemperature = 5;

    public const byte AnalogOutNodeCarrier = 0;
    public const byte AnalogOutNodeFM = 1;
    public const byte AnalogOutNodeAM = 2;

    public const byte DwfAnalogOutModeVoltage = 0;
    public const byte DwfAnalogOutModeCurrent = 1;

    public const byte DwfAnalogOutIdleDisable = 0;
    public const byte DwfAnalogOutIdleOffset = 1;
    public const byte DwfAnalogOutIdleInitial = 2;

    public const byte DwfDigitalInClockSourceInternal = 0;
    public const byte DwfDigitalInClockSourceExternal = 1;

    public const byte DwfDigitalInSampleModeSimple = 0;
    // alternate samples: noise|sample|noise|sample|...  
    // where noise is more than 1 transition between 2 samples
    public const byte DwfDigitalInSampleModeNoise = 1;

    public const byte DwfDigitalOutOutputPushPull = 0;
    public const byte DwfDigitalOutOutputOpenDrain = 1;
    public const byte DwfDigitalOutOutputOpenSource = 2;
    public const byte DwfDigitalOutOutputThreeState = 3; // for custom and random

    public const byte DwfDigitalOutTypePulse = 0;
    public const byte DwfDigitalOutTypeCustom = 1;
    public const byte DwfDigitalOutTypeRandom = 2;
    public const byte DwfDigitalOutTypeROM = 3;
    public const byte DwfDigitalOutTypeFSM = 3;

    public const byte DwfDigitalOutIdleInit = 0;
    public const byte DwfDigitalOutIdleLow = 1;
    public const byte DwfDigitalOutIdleHigh = 2;
    public const byte DwfDigitalOutIdleZet = 3;

    public const byte DwfAnalogImpedanceImpedance = 0; // Ohms
    public const byte DwfAnalogImpedanceImpedancePhase = 1; // Radians
    public const byte DwfAnalogImpedanceResistance = 2; // Ohms
    public const byte DwfAnalogImpedanceReactance = 3; // Ohms
    public const byte DwfAnalogImpedanceAdmittance = 4; // Siemen
    public const byte DwfAnalogImpedanceAdmittancePhase = 5; // Radians
    public const byte DwfAnalogImpedanceConductance = 6; // Siemen
    public const byte DwfAnalogImpedanceSusceptance = 7; // Siemen
    public const byte DwfAnalogImpedanceSeriesCapactance = 8; // Farad
    public const byte DwfAnalogImpedanceParallelCapacitance = 9; // Farad
    public const byte DwfAnalogImpedanceSeriesInductance = 10; // Henry
    public const byte DwfAnalogImpedanceParallelInductance = 11; // Henry
    public const byte DwfAnalogImpedanceDissipation = 12; // factor
    public const byte DwfAnalogImpedanceQuality = 13; // factor

    public const byte DwfParamUsbPower = 2; // 1 keep the USB power enabled even when AUX is connected, Analog Discovery 2
    public const byte DwfParamLedBrightness = 3; // LED brightness 0 ... 100%, Digital Discovery
    public const byte DwfParamOnClose = 4; // 0 continue, 1 stop, 2 shutdown
    public const byte DwfParamAudioOut = 5; // 0 disable / 1 enable audio output, Analog Discovery 1, 2
    public const byte DwfParamUsbLimit = 6; // 0..1000 mA USB power limit, -1 no limit, Analog Discovery 1, 2

    // Macro used to verify if bit is 1 or 0 in given bit field
    // #define IsBitSet(fs, bit) ((fs & (1<<bit)) != 0)


    // Error and version APIs:
    [DllImport("dwf.dll", EntryPoint="FDwfGetLastError", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfGetLastError(out int pdwferc);

    [DllImport("dwf.dll", EntryPoint="FDwfGetLastErrorMsg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfGetLastErrorMsg([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szError); // 512

    public static int FDwfGetLastErrorMsg(out string szError){
        System.Text.StringBuilder sb = new System.Text.StringBuilder(512);
        int ret = _FDwfGetLastErrorMsg(sb);
        szError = sb.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfGetVersion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfGetVersion([MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szVersion); // 32
    // Returns DLL version, for instance: "3.8.5"

    public static int FDwfGetVersion(out string szVersion){
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        int ret = _FDwfGetVersion(sb);
        szVersion = sb.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfParamSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfParamSet(int param, int value);

    [DllImport("dwf.dll", EntryPoint="FDwfParamGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfParamGet(int param, out int pvalue);


    // DEVICE MANAGMENT public static extern intS
    // Enumeration:
    [DllImport("dwf.dll", EntryPoint="FDwfEnum", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnum(int enumfilter, out int pcDevice);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumDeviceType", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumDeviceType(int idxDevice, out int pDeviceId, out int pDeviceRevision);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumDeviceIsOpened", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumDeviceIsOpened(int idxDevice, out int pfIsUsed);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumUserName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfEnumUserName(int idxDevice, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szUserName); //32

    public static int FDwfEnumUserName(int idxDevice, out string szUserName){
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        int ret = _FDwfEnumUserName(idxDevice, sb);
        szUserName = sb.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfEnumDeviceName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfEnumDeviceName(int idxDevice, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szUserName); //32

    public static int FDwfEnumDeviceName(int idxDevice, out string szDeviceName){
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        int ret = _FDwfEnumDeviceName(idxDevice, sb);
        szDeviceName = sb.ToString();
        return ret;
    }
    
    [DllImport("dwf.dll", EntryPoint="FDwfEnumSN", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfEnumSN(int idxDevice, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szSN); //32

    public static int FDwfEnumSN(int idxDevice, out string szSN){
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        int ret = _FDwfEnumSN(idxDevice, sb);
        szSN = sb.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfEnumConfig", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumConfig(int idxDevice, out int pcConfig);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumConfigInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumConfigInfo(int idxConfig, int info, out int pv);


    // Open/Close:
    [DllImport("dwf.dll", EntryPoint="FDwfDeviceOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceOpen(int idxDevice, out int phdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceConfigOpen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceConfigOpen(int idxDev, int idxCfg, out int phdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceClose", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceClose(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceCloseAll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceCloseAll();

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceAutoConfigureSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceAutoConfigureSet(int hdwf, int fAutoConfigure);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceAutoConfigureGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceAutoConfigureGet(int hdwf, out int pfAutoConfigure);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceEnableSet(int hdwf, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceTriggerInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceTriggerInfo(int hdwf, out int pfstrigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceTriggerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceTriggerSet(int hdwf, int idxPin, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceTriggerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceTriggerGet(int hdwf, int idxPin, out byte ptrigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceTriggerPC", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceTriggerPC(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceTriggerSlopeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceTriggerSlopeInfo(int hdwf, out int pfsslope);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceParamSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceParamSet(int hdwf, int param, int value);

    [DllImport("dwf.dll", EntryPoint="FDwfDeviceParamGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDeviceParamGet(int hdwf, int param, out int pvalue);



    // ANALOG IN INSTRUMENT public static extern intS
    // Control and status: 
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInConfigure(int hdwf, int fReconfigure, int fStart);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerForce", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerForce(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatus(int hdwf, int fReadData, out byte psts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusSamplesLeft", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusSamplesLeft(int hdwf, out int pcSamplesLeft);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusSamplesValid", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusSamplesValid(int hdwf, out int pcSamplesValid);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusIndexWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusIndexWrite(int hdwf, out int pidxWrite);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusAutoTriggered", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusAutoTriggered(int hdwf, out int pfAuto);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusData(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdVoltData, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusData2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusData2(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdVoltData, int idxData, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusData16", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusData16(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgu16Data, int idxData, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusNoise", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusNoise(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdMin, [MarshalAs(UnmanagedType.LPArray)] double[] rgdMax, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusNoise2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusNoise2(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdMin, [MarshalAs(UnmanagedType.LPArray)] double[] rgdMax, int idxData, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusSample", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusSample(int hdwf, int idxChannel, out double pdVoltSample);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInStatusRecord", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInStatusRecord(int hdwf, out int pcdDataAvailable, out int pcdDataLost, out int pcdDataCorrupt);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInRecordLengthSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInRecordLengthSet(int hdwf, double sLegth);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInRecordLengthGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInRecordLengthGet(int hdwf, out double psLegth);


    // Acquisition configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInFrequencyInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInFrequencyInfo(int hdwf, out double phzMin, out double phzMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInFrequencySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInFrequencySet(int hdwf, double hzFrequency);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInFrequencyGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInFrequencyGet(int hdwf, out double phzFrequency);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInBitsInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInBitsInfo(int hdwf, out int pnBits); // Returns the number of ADC bits 

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInBufferSizeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInBufferSizeInfo(int hdwf, out int pnSizeMin, out int pnSizeMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInBufferSizeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInBufferSizeSet(int hdwf, int nSize);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInBufferSizeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInBufferSizeGet(int hdwf, out int pnSize);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInNoiseSizeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInNoiseSizeInfo(int hdwf, out int pnSizeMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInNoiseSizeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInNoiseSizeSet(int hdwf, int nSize);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInNoiseSizeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInNoiseSizeGet(int hdwf, out int pnSize);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInAcquisitionModeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInAcquisitionModeInfo(int hdwf, out int pfsacqmode);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInAcquisitionModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInAcquisitionModeSet(int hdwf, int acqmode);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInAcquisitionModeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInAcquisitionModeGet(int hdwf, out int pacqmode);


    // Channel configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelCount", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelCount(int hdwf, out int pcChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelEnableSet(int hdwf, int idxChannel, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelEnableGet(int hdwf, int idxChannel, out int pfEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelFilterInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelFilterInfo(int hdwf, out int pfsfilter);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelFilterSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelFilterSet(int hdwf, int idxChannel, int filter);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelFilterGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelFilterGet(int hdwf, int idxChannel, out int pfilter);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelRangeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelRangeInfo(int hdwf, out double pvoltsMin, out double pvoltsMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint = "FDwfAnalogInChannelRangeSteps", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelRangeSteps(int hdwf, [MarshalAs(UnmanagedType.LPArray)] double[] rgVoltsStep, out int pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelRangeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelRangeSet(int hdwf, int idxChannel, double voltsRange);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelRangeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelRangeGet(int hdwf, int idxChannel, out double pvoltsRange);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelOffsetInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelOffsetInfo(int hdwf, out double pvoltsMin, out double pvoltsMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelOffsetSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelOffsetSet(int hdwf, int idxChannel, double voltOffset);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelOffsetGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelOffsetGet(int hdwf, int idxChannel, out double pvoltOffset);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelAttenuationSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelAttenuationSet(int hdwf, int idxChannel, double xAttenuation);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInChannelAttenuationGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInChannelAttenuationGet(int hdwf, int idxChannel, out double pxAttenuation);


    // Trigger configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerSourceSet(int hdwf, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerSourceGet(int hdwf, out byte ptrigsrc);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerPositionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerPositionInfo(int hdwf, out double psecMin, out double psecMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerPositionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerPositionSet(int hdwf, double secPosition);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerPositionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerPositionGet(int hdwf, out double psecPosition);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerPositionStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerPositionStatus(int hdwf, out double psecPosition);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerAutoTimeoutInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerAutoTimeoutInfo(int hdwf, out double psecMin, out double psecMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerAutoTimeoutSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerAutoTimeoutSet(int hdwf, double secTimeout);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerAutoTimeoutGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerAutoTimeoutGet(int hdwf, out double psecTimeout);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHoldOffInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHoldOffInfo(int hdwf, out double psecMin, out double psecMax, out double pnStep);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHoldOffSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHoldOffSet(int hdwf, double secHoldOff);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHoldOffGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHoldOffGet(int hdwf, out double psecHoldOff);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerTypeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerTypeInfo(int hdwf, out int pfstrigtype);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerTypeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerTypeSet(int hdwf, int trigtype);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerTypeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerTypeGet(int hdwf, out int ptrigtype);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerChannelInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerChannelInfo(int hdwf, out int pidxMin, out int pidxMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerChannelSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerChannelSet(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerChannelGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerChannelGet(int hdwf, out int pidxChannel);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerFilterInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerFilterInfo(int hdwf, out int pfsfilter);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerFilterSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerFilterSet(int hdwf, int filter);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerFilterGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerFilterGet(int hdwf, out int pfilter);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLevelInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLevelInfo(int hdwf, out double pvoltsMin, out double pvoltsMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLevelSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLevelSet(int hdwf, double voltsLevel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLevelGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLevelGet(int hdwf, out double pvoltsLevel);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHysteresisInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHysteresisInfo(int hdwf, out double pvoltsMin, out double pvoltsMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHysteresisSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHysteresisSet(int hdwf, double voltsLevel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerHysteresisGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerHysteresisGet(int hdwf, out double pvoltsHysteresis);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerConditionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerConditionInfo(int hdwf, out int pfstrigcond);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerConditionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerConditionSet(int hdwf, int trigcond);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerConditionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerConditionGet(int hdwf, out int ptrigcond);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthInfo(int hdwf, out double psecMin, out double psecMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthSet(int hdwf, double secLength);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthGet(int hdwf, out double psecLength);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthConditionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthConditionInfo(int hdwf, out int pfstriglen);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthConditionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthConditionSet(int hdwf, int triglen);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerLengthConditionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerLengthConditionGet(int hdwf, out int ptriglen);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingSourceSet(int hdwf, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingSourceGet(int hdwf, out byte ptrigsrc);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingSlopeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingSlopeSet(int hdwf, int slope);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingSlopeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingSlopeGet(int hdwf, out int pslope);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingDelaySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingDelaySet(int hdwf, double sec);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInSamplingDelayGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInSamplingDelayGet(int hdwf, out double psec);



    // ANALOG OUT INSTRUMENT public static extern intS
    // Configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutCount", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutCount(int hdwf, out int pcChannel);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutMasterSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutMasterSet(int hdwf, int idxChannel, int idxMaster);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutMasterGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutMasterGet(int hdwf, int idxChannel, out int pidxMaster);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutTriggerSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutTriggerSourceSet(int hdwf, int idxChannel, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutTriggerSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutTriggerSourceGet(int hdwf, int idxChannel, out byte ptrigsrc);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutTriggerSlopeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutTriggerSlopeSet(int hdwf, int idxChannel, int slope);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutTriggerSlopeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutTriggerSlopeGet(int hdwf, int idxChannel, out int pslope);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRunInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRunInfo(int hdwf, int idxChannel, out double psecMin, out double psecMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRunSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRunSet(int hdwf, int idxChannel, double secRun);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRunGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRunGet(int hdwf, int idxChannel, out double psecRun);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRunStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRunStatus(int hdwf, int idxChannel, out double psecRun);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutWaitInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutWaitInfo(int hdwf, int idxChannel, out double psecMin, out double psecMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutWaitSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutWaitSet(int hdwf, int idxChannel, double secWait);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutWaitGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutWaitGet(int hdwf, int idxChannel, out double psecWait);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatInfo(int hdwf, int idxChannel, out int pnMin, out int pnMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatSet(int hdwf, int idxChannel, int cRepeat);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatGet(int hdwf, int idxChannel, out int pcRepeat);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatStatus(int hdwf, int idxChannel, out int pcRepeat);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatTriggerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatTriggerSet(int hdwf, int idxChannel, int fRepeatTrigger);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutRepeatTriggerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutRepeatTriggerGet(int hdwf, int idxChannel, out int pfRepeatTrigger);


    // EExplorer channel 3&4 current/voltage limitation
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutLimitationInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutLimitationInfo(int hdwf, int idxChannel, out double pMin, out double pMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutLimitationSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutLimitationSet(int hdwf, int idxChannel, double limit);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutLimitationGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutLimitationGet(int hdwf, int idxChannel, out double plimit);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutModeSet(int hdwf, int idxChannel, int mode);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutModeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutModeGet(int hdwf, int idxChannel, out int pmode);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutIdleInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutIdleInfo(int hdwf, int idxChannel, out int pfsidle);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutIdleSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutIdleSet(int hdwf, int idxChannel, int idle);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutIdleGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutIdleGet(int hdwf, int idxChannel, out int pidle);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeInfo(int hdwf, int idxChannel, out int pfsnode);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeEnableSet(int hdwf, int idxChannel, int node, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeEnableGet(int hdwf, int idxChannel, int node, out int pfEnable);


    [DllImport("dwf.dll", EntryPoint= "FDwfAnalogOutNodeFunctionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFunctionInfo(int hdwf, int idxChannel, int node, out int pfsfunc);

    [DllImport("dwf.dll", EntryPoint= "FDwfAnalogOutNodeFunctionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFunctionSet(int hdwf, int idxChannel, int node, byte func);

    [DllImport("dwf.dll", EntryPoint= "FDwfAnalogOutNodeFunctionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFunctionGet(int hdwf, int idxChannel, int node, out byte pfunc);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeFrequencyInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFrequencyInfo(int hdwf, int idxChannel, int node, out double phzMin, out double phzMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeFrequencySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFrequencySet(int hdwf, int idxChannel, int node, double hzFrequency);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeFrequencyGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeFrequencyGet(int hdwf, int idxChannel, int node, out double phzFrequency);

    // Carrier Amplitude or Modulation Index 
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeAmplitudeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeAmplitudeInfo(int hdwf, int idxChannel, int node, out double pMin, out double pMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeAmplitudeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeAmplitudeSet(int hdwf, int idxChannel, int node, double vAmplitude);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeAmplitudeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeAmplitudeGet(int hdwf, int idxChannel, int node, out double pvAmplitude);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeOffsetInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeOffsetInfo(int hdwf, int idxChannel, int node, out double pMin, out double pMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeOffsetSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeOffsetSet(int hdwf, int idxChannel, int node, double vOffset);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeOffsetGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeOffsetGet(int hdwf, int idxChannel, int node, out double pvOffset);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeSymmetryInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeSymmetryInfo(int hdwf, int idxChannel, int node, out double ppercentageMin, out double ppercentageMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeSymmetrySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeSymmetrySet(int hdwf, int idxChannel, int node, double percentageSymmetry);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeSymmetryGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeSymmetryGet(int hdwf, int idxChannel, int node, out double ppercentageSymmetry);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodePhaseInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodePhaseInfo(int hdwf, int idxChannel, int node, out double pdegreeMin, out double pdegreeMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodePhaseSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodePhaseSet(int hdwf, int idxChannel, int node, double degreePhase);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodePhaseGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodePhaseGet(int hdwf, int idxChannel, int node, out double pdegreePhase);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeDataInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeDataInfo(int hdwf, int idxChannel, int node, out int pnSamplesMin, out int pnSamplesMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodeDataSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodeDataSet(int hdwf, int idxChannel, int node, [MarshalAs(UnmanagedType.LPArray)] double[] rgdData, int cdData);


    // needed for EExplorer, not used for ADiscovery
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutCustomAMFMEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutCustomAMFMEnableSet(int hdwf, int idxChannel, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutCustomAMFMEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutCustomAMFMEnableGet(int hdwf, int idxChannel, out int pfEnable);


    // Control:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutReset(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutConfigure(int hdwf, int idxChannel, int fStart);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutStatus(int hdwf, int idxChannel, out byte psts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodePlayStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodePlayStatus(int hdwf, int idxChannel, int node, out int cdDataFree, out int cdDataLost, out int cdDataCorrupted);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutNodePlayData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutNodePlayData(int hdwf, int idxChannel, int node, [MarshalAs(UnmanagedType.LPArray)] double[] rgdData, int cdData);



    // ANALOG IO INSTRUMENT public static extern intS
    // Control:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOConfigure(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOStatus(int hdwf);


    // Configure:
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOEnableInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOEnableInfo(int hdwf, out int pfSet, out int pfStatus);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOEnableSet(int hdwf, int fMasterEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOEnableGet(int hdwf, out int pfMasterEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOEnableStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOEnableStatus(int hdwf, out int pfMasterEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelCount", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelCount(int hdwf, out int pnChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfAnalogIOChannelName(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szName, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szLabel); //32 16

    public static int FDwfAnalogIOChannelName(int hdwf, int idxChannel, out string szName, out string szLabel){
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder(32);
        System.Text.StringBuilder sb2 = new System.Text.StringBuilder(16);
        int ret = _FDwfAnalogIOChannelName(hdwf, idxChannel, sb1, sb2);
        szName = sb1.ToString();
        szLabel = sb2.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelInfo(int hdwf, int idxChannel, out int pnNodes);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int _FDwfAnalogIOChannelNodeName(int hdwf, int idxChannel, int idxNode, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szNodeName, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder szNodeUnits); //32 16

    public static int FDwfAnalogIOChannelNodeName(int hdwf, int idxChannel, int idxNode, out string szNodeName, out string szNodeUnits){
        System.Text.StringBuilder sb1 = new System.Text.StringBuilder(32);
        System.Text.StringBuilder sb2 = new System.Text.StringBuilder(16);
        int ret = _FDwfAnalogIOChannelNodeName(hdwf, idxChannel, idxNode, sb1, sb2);
        szNodeName = sb1.ToString();
        szNodeUnits = sb2.ToString();
        return ret;
    }

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeInfo(int hdwf, int idxChannel, int idxNode, out byte panalogio);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeSetInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeSetInfo(int hdwf, int idxChannel, int idxNode, out double pmin, out double pmax, out int pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeSet(int hdwf, int idxChannel, int idxNode, double value);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeGet(int hdwf, int idxChannel, int idxNode, out double pvalue);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeStatusInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeStatusInfo(int hdwf, int idxChannel, int idxNode, out double pmin, out double pmax, out int pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogIOChannelNodeStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogIOChannelNodeStatus(int hdwf, int idxChannel, int idxNode, out double pvalue);



    // DIGITAL IO INSTRUMENT public static extern intS
    // Control:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOConfigure(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOStatus(int hdwf);


    // Configure:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableInfo(int hdwf, out int pfsOutputEnableMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableSet(int hdwf, uint fsOutputEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableGet(int hdwf, out int pfsOutputEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputInfo(int hdwf, out int pfsOutputMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputSet(int hdwf, uint fsOutput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputGet(int hdwf, out int pfsOutput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOInputInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOInputInfo(int hdwf, out int pfsInputMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOInputStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOInputStatus(int hdwf, out int pfsInput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableInfo64(int hdwf, out ulong pfsOutputEnableMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableSet64(int hdwf, ulong fsOutputEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputEnableGet64(int hdwf, out ulong pfsOutputEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputInfo64(int hdwf, out ulong pfsOutputMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputSet64(int hdwf, ulong fsOutput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOOutputGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOOutputGet64(int hdwf, out ulong pfsOutput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOInputInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOInputInfo64(int hdwf, out ulong pfsInputMask);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalIOInputStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalIOInputStatus64(int hdwf, out ulong pfsInput);



    // DIGITAL IN INSTRUMENT public static extern intS
    // Control and status: 
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInConfigure(int hdwf, int fReconfigure, int fStart);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatus(int hdwf, int fReadData, out byte psts);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusSamplesLeft", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusSamplesLeft(int hdwf, out int pcSamplesLeft);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusSamplesValid", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusSamplesValid(int hdwf, out int pcSamplesValid);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusIndexWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusIndexWrite(int hdwf, out int pidxWrite);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusAutoTriggered", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusAutoTriggered(int hdwf, out int pfAuto);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusData(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] rgData, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusData2(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] rgData, int idxSample, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoise(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] rgData, int countOfDataBytes);
    
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoise2(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] rgData, int idxSample, int countOfDataBytes);
    
    
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusDataUShort(int hdwf, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgData, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusData2UShort(int hdwf, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgData, int idxSample, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoiseUShort(int hdwf, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgData, int countOfDataBytes);
    
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoise2UShort(int hdwf, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgData, int idxSample, int countOfDataBytes);
    

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusDataUInt(int hdwf, [MarshalAs(UnmanagedType.LPArray)] uint[] rgData, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusData2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusData2UInt(int hdwf, [MarshalAs(UnmanagedType.LPArray)] uint[] rgData, int idxSample, int countOfDataBytes);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoiseUInt(int hdwf, [MarshalAs(UnmanagedType.LPArray)] uint[] rgData, int countOfDataBytes);
    
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusNoise2", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusNoise2UInt(int hdwf, [MarshalAs(UnmanagedType.LPArray)] uint[] rgData, int idxSample, int countOfDataBytes);
    
    
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInStatusRecord", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInStatusRecord(int hdwf, out int pcdDataAvailable, out int pcdDataLost, out int pcdDataCorrupt);

    // Acquisition configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInInternalClockInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInInternalClockInfo(int hdwf, out double phzFreq);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInClockSourceInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInClockSourceInfo(int hdwf, out int pfsDwfDigitalInClockSource);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInClockSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInClockSourceSet(int hdwf, int v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInClockSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInClockSourceGet(int hdwf, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInDividerInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInDividerInfo(int hdwf, out int pdivMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInDividerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInDividerSet(int hdwf, uint div);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInDividerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInDividerGet(int hdwf, out int pdiv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInBitsInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInBitsInfo(int hdwf, out int pnBits);
    // Returns the number of Digital In bits
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleFormatSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleFormatSet(int hdwf, int nBits);
    // valid options 8/16/32
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleFormatGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleFormatGet(int hdwf, out int pnBits);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInInputOrderSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInInputOrderSet(int hdwf, int fDioFirst);
    // for Digital Discovery

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInBufferSizeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInBufferSizeInfo(int hdwf, out int pnSizeMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInBufferSizeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInBufferSizeSet(int hdwf, int nSize);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInBufferSizeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInBufferSizeGet(int hdwf, out int pnSize);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleModeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleModeInfo(int hdwf, out int pfsDwfDigitalInSampleMode);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleModeSet(int hdwf, int v);
    // 
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleModeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleModeGet(int hdwf, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleSensibleSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleSensibleSet(int hdwf, uint fs);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInSampleSensibleGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInSampleSensibleGet(int hdwf, out int pfs);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInAcquisitionModeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInAcquisitionModeInfo(int hdwf, out int pfsacqmode);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInAcquisitionModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInAcquisitionModeSet(int hdwf, int acqmode);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInAcquisitionModeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInAcquisitionModeGet(int hdwf, out int pacqmode);


    // Trigger configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSourceSet(int hdwf, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSourceGet(int hdwf, out byte ptrigsrc);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSlopeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSlopeSet(int hdwf, int slope);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSlopeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSlopeGet(int hdwf, out int pslope);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerPositionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerPositionInfo(int hdwf, out int pnSamplesAfterTriggerMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerPositionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerPositionSet(int hdwf, uint cSamplesAfterTrigger);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerPositionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerPositionGet(int hdwf, out int pcSamplesAfterTrigger);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerPrefillSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerPrefillSet(int hdwf, uint cSamplesBeforeTrigger);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerPrefillGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerPrefillGet(int hdwf, out int pcSamplesBeforeTrigger);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerAutoTimeoutInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerAutoTimeoutInfo(int hdwf, out double psecMin, out double psecMax, out double pnSteps);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerAutoTimeoutSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerAutoTimeoutSet(int hdwf, double secTimeout);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerAutoTimeoutGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerAutoTimeoutGet(int hdwf, out double psecTimeout);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerInfo(int hdwf, out int pfsLevelLow, out int pfsLevelHigh, out int pfsEdgeRise, out int pfsEdgeFall);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSet(int hdwf, uint fsLevelLow, uint fsLevelHigh, uint fsEdgeRise, uint fsEdgeFall);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerGet(int hdwf, out int pfsLevelLow, out int pfsLevelHigh, out int pfsEdgeRise, out int pfsEdgeFall);

    // the logic for trigger bits: Low and High and (Rise or Fall)
    // bits set in Rise and Fall means any edge

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerResetSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerResetSet(int hdwf, uint fsLevelLow, uint fsLevelHigh, uint fsEdgeRise, uint fsEdgeFall);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerCountSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerCountSet(int hdwf, int cCount, int fRestart);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerLengthSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerLengthSet(int hdwf, double secMin, double secMax, int idxSync);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerMatchSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerMatchSet(int hdwf, int iPin, uint fsMask, uint fsValue, int cBitStuffing);



    // DIGITAL OUT INSTRUMENT public static extern intS
    // Control:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutConfigure(int hdwf, int fStart);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutStatus(int hdwf, out byte psts);


    // Configuration:
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutInternalClockInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutInternalClockInfo(int hdwf, out double phzFreq);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTriggerSourceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTriggerSourceSet(int hdwf, byte trigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTriggerSourceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTriggerSourceGet(int hdwf, out byte ptrigsrc);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRunInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRunInfo(int hdwf, out double psecMin, out double psecMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRunSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRunSet(int hdwf, double secRun);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRunGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRunGet(int hdwf, out double psecRun);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRunStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRunStatus(int hdwf, out double psecRun);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutWaitInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutWaitInfo(int hdwf, out double psecMin, out double psecMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutWaitSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutWaitSet(int hdwf, double secWait);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutWaitGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutWaitGet(int hdwf, out double psecWait);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatInfo(int hdwf, out int pnMin, out int pnMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatSet(int hdwf, uint cRepeat);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatGet(int hdwf, out int pcRepeat);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatStatus(int hdwf, out int pcRepeat);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTriggerSlopeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTriggerSlopeSet(int hdwf, int slope);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTriggerSlopeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTriggerSlopeGet(int hdwf, out int pslope);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatTriggerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatTriggerSet(int hdwf, int fRepeatTrigger);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutRepeatTriggerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutRepeatTriggerGet(int hdwf, out int pfRepeatTrigger);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCount", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCount(int hdwf, out int pcChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutEnableSet(int hdwf, int idxChannel, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutEnableGet(int hdwf, int idxChannel, out int pfEnable);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutOutputInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutOutputInfo(int hdwf, int idxChannel, out int pfsDwfDigitalOutOutput);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutOutputSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutOutputSet(int hdwf, int idxChannel, int v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutOutputGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutOutputGet(int hdwf, int idxChannel, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTypeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTypeInfo(int hdwf, int idxChannel, out int pfsDwfDigitalOutType);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTypeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTypeSet(int hdwf, int idxChannel, int v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTypeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTypeGet(int hdwf, int idxChannel, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutIdleInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutIdleInfo(int hdwf, int idxChannel, out int pfsDwfDigitalOutIdle);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutIdleSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutIdleSet(int hdwf, int idxChannel, int v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutIdleGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutIdleGet(int hdwf, int idxChannel, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDividerInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDividerInfo(int hdwf, int idxChannel, out int vMin, out int vMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDividerInitSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDividerInitSet(int hdwf, int idxChannel, int v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDividerInitGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDividerInitGet(int hdwf, int idxChannel, out int pv);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDividerSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDividerSet(int hdwf, int idxChannel, uint v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDividerGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDividerGet(int hdwf, int idxChannel, out int pv);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCounterInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCounterInfo(int hdwf, int idxChannel, out int vMin, out int vMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCounterInitSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCounterInitSet(int hdwf, int idxChannel, int fHigh, uint v);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCounterInitGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCounterInitGet(int hdwf, int idxChannel, out int pfHigh, out int pv);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCounterSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCounterSet(int hdwf, int idxChannel, uint vLow, uint vHigh);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutCounterGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutCounterGet(int hdwf, int idxChannel, out int pvLow, out int pvHigh);



    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDataInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDataInfo(int hdwf, int idxChannel, out int pcountOfBitsMax);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutDataSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutDataSet(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] byte[] rgBits, uint countOfBits);

    // bits order is lsb first
    // for TS output the count of bits its the total number of IO|OE bits, it should be an even number
    // BYTE:   0                 |1     ...
    // bit:    0 |1 |2 |3 |...|7 |0 |1 |...
    // sample: IO|OE|IO|OE|...|OE|IO|OE|...


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartRateSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartRateSet(int hdwf, double hz);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartBitsSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartBitsSet(int hdwf, int cBits);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartParitySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartParitySet(int hdwf, int parity);
    // 0 none, 1 odd, 2 even
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartStopSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartStopSet(int hdwf, double cBit);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartTxSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartTxSet(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartRxSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartRxSet(int hdwf, int idxChannel);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartTx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartTx(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] szTx, int cTx);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalUartRx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalUartRx(int hdwf, [MarshalAs(UnmanagedType.LPArray)] byte[] szRx, int cRx, out int pcRx, out int pParity);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiFrequencySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiFrequencySet(int hdwf, double hz);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiClockSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiClockSet(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiDataSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiDataSet(int hdwf, int idxDQ, int idxChannel);
    // 0 DQ0_MOSI_SISO, 1 DQ1_MISO, 2 DQ2, 3 DQ3
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiModeSet(int hdwf, int iMode);
    // bit1 CPOL, bit0 CPHA
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiOrderSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiOrderSet(int hdwf, int fMSBLSB);
    // bit order: 0 MSB first, 1 LSB first

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiSelect", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiSelect(int hdwf, int idxChannel, int level);
    // 0 low, 1 high, -1 Z
    // cDQ 0 SISO, 1 MOSI/MISO, 2 dual, 4 quad, // 1-32 bits / word
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWriteRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWriteRead(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] byte[] rgTX, int cTX, [MarshalAs(UnmanagedType.LPArray)] byte[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWriteRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWriteRead16(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgTX, int cTX, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWriteRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWriteRead32(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] int[] rgTX, int cTX, [MarshalAs(UnmanagedType.LPArray)] int[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiRead(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] byte[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiReadOne", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiReadOne(int hdwf, int cDQ, int cBitPerWord, out int pRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiRead16(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiRead", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiRead32(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] int[] rgRX, int cRX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWrite(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] byte[] rgTX, int cTX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWriteOne", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWriteOne(int hdwf, int cDQ, int cBits, uint vTX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWrite16(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] ushort[] rgTX, int cTX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalSpiWrite", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalSpiWrite32(int hdwf, int cDQ, int cBitPerWord, [MarshalAs(UnmanagedType.LPArray)] int[] rgTX, int cTX);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cClear(int hdwf, out int pfFree);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cStretchSet(int hdwf, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cRateSet(int hdwf, double hz);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cReadNakSet(int hdwf, int fNakLastReadByte);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cSclSet(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cSdaSet(int hdwf, int idxChannel);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cWriteRead(int hdwf, byte adr8bits, [MarshalAs(UnmanagedType.LPArray)] byte[] rgbTx, int cTx, [MarshalAs(UnmanagedType.LPArray)] byte[] rgRx, int cRx, out int pNak);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cRead(int hdwf, byte adr8bits, [MarshalAs(UnmanagedType.LPArray)] byte[] rgbRx, int cRx, out int pNak);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cWrite(int hdwf, byte adr8bits, [MarshalAs(UnmanagedType.LPArray)] byte[] rgbTx, int cTx, out int pNak);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalI", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalI2cWriteOne(int hdwf, byte adr8bits, byte bTx, out int pNak);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanRateSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanRateSet(int hdwf, double hz);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanPolaritySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanPolaritySet(int hdwf, int fHigh);
    // 0 low, 1 high
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanTxSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanTxSet(int hdwf, int idxChannel);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanRxSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanRxSet(int hdwf, int idxChannel);


    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanTx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanTx(int hdwf, int vID, int fExtended, int fRemote, int cDLC, [MarshalAs(UnmanagedType.LPArray)] byte[] rgTX);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalCanRx", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalCanRx(int hdwf, out int pvID, out int pfExtended, out int pfRemote, out int pcDLC, [MarshalAs(UnmanagedType.LPArray)] byte[] rgRX, int cRX, out int pvStatus);


    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceModeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceModeSet(int hdwf, int mode);
    // 0 W1-C1-DUT-C2-R-GND, 1 W1-C1-R-C2-DUT-GND, 8 Impedance Analyzer for AD
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceModeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceModeGet(int hdwf, out int mode);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceReferenceSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceReferenceSet(int hdwf, double ohms);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceReferenceGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceReferenceGet(int hdwf, out double pohms);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceFrequencySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceFrequencySet(int hdwf, double hz);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceFrequencyGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceFrequencyGet(int hdwf, out double phz);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceAmplitudeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceAmplitudeSet(int hdwf, double volts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceAmplitudeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceAmplitudeGet(int hdwf, out double pvolts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceOffsetSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceOffsetSet(int hdwf, double volts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceOffsetGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceOffsetGet(int hdwf, out double pvolts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceProbeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceProbeSet(int hdwf, double ohmRes, double faradCap);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceProbeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceProbeGet(int hdwf, out double pohmRes, out double pfaradCap);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedancePeriodSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedancePeriodSet(int hdwf, int cMinPeriods);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedancePeriodGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedancePeriodGet(int hdwf, out int cMinPeriods);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceCompReset", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceCompReset(int hdwf);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceCompSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceCompSet(int hdwf, double ohmOpenResistance, double ohmOpenReactance, double ohmShortResistance, double ohmShortReactance);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceCompGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceCompGet(int hdwf, out double pohmOpenResistance, out double pohmOpenReactance, out double pohmShortResistance, out double pohmShortReactance);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceConfigure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceConfigure(int hdwf, int fStart);
    // 1 start, 0 stop
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceStatus(int hdwf, out byte psts);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceStatusInput", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceStatusInput(int hdwf, int idxChannel, out double pgain, out double pradian);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogImpedanceStatusMeasure", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogImpedanceStatusMeasure(int hdwf, int measure, out double pvalue);




    // OBSOLETE but supported, avoid using the following in new projects:
    public const byte DwfParamKeepOnClose = 1; // keep the device running after close, use DwfParamOnClose

    // use FDwfDigitalInTriggerSourceSet trigsrcAnalogIn
    // call FDwfDigitalInConfigure before FDwfAnalogInConfigure
    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInMixedSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInMixedSet(int hdwf, int fEnable);

    // use DwfTriggerSlope
    public const int trigcondRisingPositive = 0;
    public const int trigcondFallingNegative = 1;

    // use FDwfDeviceTriggerInfo(hdwf, ptrigsrcInfo) As Integer
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogInTriggerSourceInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogInTriggerSourceInfo(int hdwf, out int pfstrigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutTriggerSourceInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutTriggerSourceInfo(int hdwf, int idxChannel, out int pfstrigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalInTriggerSourceInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalInTriggerSourceInfo(int hdwf, out int pfstrigsrc);

    [DllImport("dwf.dll", EntryPoint="FDwfDigitalOutTriggerSourceInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfDigitalOutTriggerSourceInfo(int hdwf, out int pfstrigsrc);


    // use BYTE
    public const byte stsRdy = 0;
    public const byte stsArm = 1;
    public const byte stsDone = 2;
    public const byte stsTrig = 3;
    public const byte stsCfg = 4;
    public const byte stsPrefill = 5;
    public const byte stsNotDone = 6;
    public const byte stsTrigDly = 7;
    public const byte stsError = 8;
    public const byte stsBusy = 9;
    public const byte stsStop = 10;


    // use FDwfAnalogOutNode*
    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutEnableSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutEnableSet(int hdwf, int idxChannel, int fEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutEnableGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutEnableGet(int hdwf, int idxChannel, out int pfEnable);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutFunctionInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFunctionInfo(int hdwf, int idxChannel, out int pfsfunc);

    [DllImport("dwf.dll", EntryPoint= "FDwfAnalogOutFunctionSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFunctionSet(int hdwf, int idxChannel, byte func);

    [DllImport("dwf.dll", EntryPoint= "FDwfAnalogOutFunctionGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFunctionGet(int hdwf, int idxChannel, out byte pfunc);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutFrequencyInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFrequencyInfo(int hdwf, int idxChannel, out double phzMin, out double phzMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutFrequencySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFrequencySet(int hdwf, int idxChannel, double hzFrequency);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutFrequencyGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutFrequencyGet(int hdwf, int idxChannel, out double phzFrequency);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutAmplitudeInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutAmplitudeInfo(int hdwf, int idxChannel, out double pvoltsMin, out double pvoltsMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutAmplitudeSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutAmplitudeSet(int hdwf, int idxChannel, double voltsAmplitude);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutAmplitudeGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutAmplitudeGet(int hdwf, int idxChannel, out double pvoltsAmplitude);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutOffsetInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutOffsetInfo(int hdwf, int idxChannel, out double pvoltsMin, out double pvoltsMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutOffsetSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutOffsetSet(int hdwf, int idxChannel, double voltsOffset);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutOffsetGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutOffsetGet(int hdwf, int idxChannel, out double pvoltsOffset);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutSymmetryInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutSymmetryInfo(int hdwf, int idxChannel, out double ppercentageMin, out double ppercentageMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutSymmetrySet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutSymmetrySet(int hdwf, int idxChannel, double percentageSymmetry);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutSymmetryGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutSymmetryGet(int hdwf, int idxChannel, out double ppercentageSymmetry);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutPhaseInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutPhaseInfo(int hdwf, int idxChannel, out double pdegreeMin, out double pdegreeMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutPhaseSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutPhaseSet(int hdwf, int idxChannel, double degreePhase);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutPhaseGet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutPhaseGet(int hdwf, int idxChannel, out double pdegreePhase);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutDataInfo", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutDataInfo(int hdwf, int idxChannel, out int pnSamplesMin, out int pnSamplesMax);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutDataSet", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutDataSet(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdData, int cdData);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutPlayStatus", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutPlayStatus(int hdwf, int idxChannel, out int cdDataFree, out int cdDataLost, out int cdDataCorrupted);

    [DllImport("dwf.dll", EntryPoint="FDwfAnalogOutPlayData", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfAnalogOutPlayData(int hdwf, int idxChannel, [MarshalAs(UnmanagedType.LPArray)] double[] rgdData, int cdData);


    [DllImport("dwf.dll", EntryPoint="FDwfEnumAnalogInChannels", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumAnalogInChannels(int idxDevice, out int pnChannels);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumAnalogInBufferSize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumAnalogInBufferSize(int idxDevice, out int pnBufferSize);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumAnalogInBits", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumAnalogInBits(int idxDevice, out int pnBits);

    [DllImport("dwf.dll", EntryPoint="FDwfEnumAnalogInFrequency", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int FDwfEnumAnalogInFrequency(int idxDevice, out double phzFrequency);

}
