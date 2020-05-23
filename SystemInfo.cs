using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace SystemMonitorBySeld
{

    class CIM_ManagedSystemElement
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    };
    class CIM_LogicalDevice:CIM_ManagedSystemElement
    {
        private static Dictionary<int, string> DictAvailability = new Dictionary<int, string> {
            { 1, "Other" },
            { 2, "Unknown"},
            { 3, "Running/Full Power" },
            { 4, "Warning"},
            { 5, "In Test"},
            { 6, "Not Applicable"},
            { 7, "Power Off"},
            { 8, "Off Line"},
            { 9, "Off Duty"},
            { 10, "Degraded"},
            { 11, "Not Installed"},
            { 12, "Install Error"},
            { 13, "Power Save - Unknown"},
            { 14, "Power Save - Low Power Mode"},
            { 15, "Power Save - Standby"},
            { 16, "Power Cycle"},
            { 17, "Power Save - Warning"},
            { 18, "Paused"},
            { 19, "Not Ready"},
            { 20, "Not Configured"},
            { 21, "Quiesced"}

        };
        private static Dictionary<int, string> DictConfigManagerErrorCode = new Dictionary<int, string> {
            {0,"This device is working properly." },
            {1,"This device is not configured correctly." },
            {2,"Windows cannot load the driver for this device." },
            {3,"The driver for this device might be corrupted, or your system may be running low on memory or other resources."},
            {4,"This device is not working properly. One of its drivers or your registry might be corrupted." },
            {5,"The driver for this device needs a resource that Windows cannot manage." },
            {6,"The boot configuration for this device conflicts with other devices." },
            {7,"Cannot filter." },
            {8,"The driver loader for the device is missing." },
            {9,"This device is not working properly because the controlling firmware is reporting the resources for the device incorrectly." },
            {10,"This device cannot start."},
            {11,"This device failed." },
            {12,"This device cannot find enough free resources that it can use." },
            {13,"Windows cannot verify this device's resources." },
            {14,"This device cannot work properly until you restart your computer." },
            {15,"This device is not working properly because there is probably a re-enumeration problem." },
            {16,"Windows cannot identify all the resources this device uses." },
            {17,"This device is asking for an unknown resource type." },
            {18,"Reinstall the drivers for this device." },
            {19,"Failure using the VxD loader." },
            {20,"Your registry might be corrupted." },
            {21,"System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device." },
            {22,"This device is disabled." },
            {23,"System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation." },
            {24,"This device is not present, is not working properly, or does not have all its drivers installed." },
            {25,"Windows is still setting up this device." },
            {26,"Windows is still setting up this device." },
            {27,"This device does not have valid log configuration." },
            {28,"The drivers for this device are not installed." },
            {29,"This device is disabled because the firmware of the device did not give it the required resources." },
            {30,"This device is using an Interrupt Request (IRQ) resource that another device is using." },
            {31,"This device is not working properly because Windows cannot load the drivers required for this device." }
        };
        private static Dictionary<int, string> DictPowerManagementCapabilities = new Dictionary<int, string>
        {
            {0,"Unknown" },
            {1,"Not Supported" },
            {2,"Disabled" },
            {3,"Enabled" },
            {4,"Power Saving Modes Entered Automatically" },
            {5,"Power State Settable" },
            {6,"Power Cycling Supported" },
            {7,"Timed Power On Supported" },

        };
        private static Dictionary<int, string> DictStatusInfo = new Dictionary<int, string>
        {
            {1,"Other" },
            {2,"Unknown" },
            {3,"Enabled" },
            {4,"Disabled" },
            {5,"Not Applicable" }
        };

        public UInt16? Availability { get; set; }
        public string AvailabilityString { 
            get 
            {
                return Availability!=null ? DictAvailability[(int)Availability] : null;
            } 
        }
        public UInt32? ConfigManagerErrorCode { get; set; }
        public string ConfigManagerErrorString { 
            get 
            {
                return ConfigManagerErrorCode != null? DictConfigManagerErrorCode[(int)ConfigManagerErrorCode] : null;
            } 
        }
        public Boolean? ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string DeviceID { get; set; }
        public UInt16[] PowerManagementCapabilities { get; set; }
        public string[] PowerManagementCapabilitiesString {
            get
            {

                if (PowerManagementCapabilities != null)
                {
                    string[] PowerManagementCapabilitiesStringArray = new string[PowerManagementCapabilities.Length];
                    for (int i = 0; i < PowerManagementCapabilities.Length; i++)
                    {
                        PowerManagementCapabilitiesStringArray[i] = DictPowerManagementCapabilities[PowerManagementCapabilities[i]];
                    }
                    return PowerManagementCapabilitiesStringArray;
                }
                return null;
                    
            }
            
            
        }
        public Boolean? ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public  UInt32? LastErrorCode { get; set; }
        public string PNPDeviceID { get; set; }
        public Boolean? PowerManagementSupported { get; set; }
        public UInt16? StatusInfo { get; set; }
        public string StatusInfoString { 
            get 
            {
                return StatusInfo != null ? DictStatusInfo[(int)StatusInfo] : null;
            } 
        }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
    }

    class CIM_Processor : CIM_LogicalDevice
    {
        private Dictionary<int, string> DictUpgradeMethod = new Dictionary<int, string>
        {
            {1,"Other" },
            {2,"Unknown" },
            {3,"Daughter Board" },
            {4,"ZIF Socket" },
            {5,"Replacement/Piggy Back" },
            {6,"None" },
            {7,"LIF Socket" },
            {8,"Slot 1" },
            {9,"Slot 2" },
            {10,"370 Pin Socket" },
            {11,"Slot A" },
            {12,"Slot M" },
            {13,"Socket 423" },
            {14,"Socket A (Socket 462)" },
            {15,"Socket 478" },
            {16,"Socket 754" },
            {17,"Socket 940" },
            {18,"Socket 939" }
        };
        public UInt16? AddressWidth { get; set; }
      
       
       public UInt32? CurrentClockSpeed { get; set; }
       public UInt16? DataWidth { get; set; }
       

       public UInt16? Family { get; set; }
        
       public UInt16? LoadPercentage { get; set; }
       public UInt32? MaxClockSpeed { get; set; }
    
       public string OtherFamilyDescription { get; set; }
       
       public string Role { get; set; }
       
       public  string Stepping { get; set; }
       
       public  string UniqueId { get; set; }
       public  UInt16? UpgradeMethod { get; set; }
       public string UpgradeMethodString { 
            get 
            {
                return UpgradeMethod != null ? DictUpgradeMethod[(int)UpgradeMethod] : null;
            } 
        }

    };
    class SystemInfo
    {
        
        
        private static Dictionary<int, string> DictProcessorType = new Dictionary<int, string>
        {
            {1,"Other" },
            {2,"Unknown" },
            {3,"Central Processor" },
            {4,"Math Processor" },
            {5,"DSP Processor"},
            {6,"Video Processor" }
        };
        private static Dictionary<int, string> DictCpuStatus = new Dictionary<int, string>
        {
            {0,"Unknown" },
            {1,"CPU Enabled" },
            {2,"CPU Disabled by User via BIOS Setup" },
            {3,"CPU Disabled By BIOS (POST Error)" },
            {4,"CPU is Idle" },
            {5,"Reserved" },
            {6,"Reserved" },
            {7,"Other" }
        };
       
        private static Dictionary<int, string> DictArchitecture = new Dictionary<int, string>
        {
            {0,"x86" },
            {1,"MIPS" },
            {2,"Alpha" },
            {3,"PowerPc" },
            {5,"ARM" },
            {6,"ia64" },
            {9,"x64" }
        };
        private static Dictionary<int, string> DictVoltageCaps = new Dictionary<int, string>
        {
            {1,"5" },
            {2,"3.3" },
            {4,"2.9" }
        };
        private static Dictionary<int, double> DictVoltageCapsDouble = new Dictionary<int, double>
        {
            {1,5 },
            {2,3.3 },
            {4,2.9 }
        };
       
        public List<CPU> CPUDATALIST = new List<CPU>();
       
        private void GetCPUinfo()
        {

            var query = string.Format("Select * from Win32_Processor");
            var search = new ManagementObjectSearcher("root\\CIMV2", query);
            var results = search.Get();
            foreach(ManagementObject obj in results)
            {
                CPUDATALIST.Add(new CPU(obj));
            }

            

        }
        public SystemInfo()
        {


            
            GetCPUinfo();
        }
       public  class CPU:CIM_Processor{
            
            
            public UInt16? Architecture { get; set; }
            public string ArchitectureString { 
                get 
                {
                   return Architecture != null?DictArchitecture[(int)Architecture]:null;
                }
            }

            public string AssetTag { get; set; }

            public UInt32? Characteristics { get; set; }

            public UInt16? CpuStatus { get; set; }
            public string CpuStatusString {
                get { return CpuStatus != null ? DictCpuStatus[(int)CpuStatus] : null; }
                
             }
            public UInt16? CurrentVoltage { get; set; }
            public UInt32? ExtClock { get; set; }
            public UInt32? L2CacheSize { get; set; }
            public UInt32? L2CacheSpeed { get; set; }
            public UInt32? L3CacheSize { get; set; }
            public UInt32? L3CacheSpeed { get; set; }
            public UInt16? Level { get; set; }
            public UInt32? NumberOfCores { get; set; }

            public string Manufacturer { get; set; }
            public UInt32? NumberOfEnabledCore { get; set; }
            
            public UInt32? NumberOfLogicalProcessors { get; set; }
            public string PartNumber { get; set; }
            public string ProcessorId { get; set; }
            public UInt16? ProcessorType { get; set; }

            public string ProcessorTypeString { 
                get
                {
                    return ProcessorType != null ? DictProcessorType[(int)ProcessorType] : null;
                } 
            }
            public UInt16? Revision { get; set; }
            public bool? SecondLevelAddressTranslationExtensions { get; set; }
            public string SerialNumber { get; set; }
            public string SocketDesignation { get; set; }
            public UInt32? ThreadCount { get; set; }
            public string Version { get; set; }
            public bool? VirtualizationFirmwareEnabled { get; set; }
            public bool? VMMonitorModeExtensions { get; set; }
            public UInt32? VoltageCaps { get; set; }
            public string VoltageCapsString { 
                get 
                {
                    return VoltageCaps != null ? DictVoltageCaps[(int)VoltageCaps] : null;
                }
            }
            public double? VoltageCapsDouble { 
                get 
                {
                    if (VoltageCaps != null)
                        return DictVoltageCapsDouble[(int)VoltageCaps];
                    return   null;
                } 
            }
            public CPU(ManagementObject mObject)
            {
                AddressWidth = (UInt16?)mObject["AddressWidth"];
                Architecture = (UInt16?)mObject["Architecture"];
                AssetTag = (string)mObject["AssetTag"];
                Availability = (UInt16?)mObject["Availability"];
                Caption = (string)mObject["Caption"];
                Characteristics = (UInt32?)mObject["Characteristics"];
                ConfigManagerErrorCode = (UInt32?)mObject["ConfigManagerErrorCode"];
                ConfigManagerUserConfig = (bool?)mObject["ConfigManagerUserConfig"];
                CpuStatus = (UInt16?)mObject["CpuStatus"];
                CreationClassName = (string)mObject["CreationClassName"];
                CurrentClockSpeed = (UInt32?)mObject["CurrentClockSpeed"];
                CurrentVoltage = (UInt16?)mObject["CurrentVoltage"];
                DataWidth = (UInt16?)mObject["DataWidth"];
                Description = (string)mObject["Description"];
                DeviceID = (string)mObject["DeviceID"];
                ErrorCleared = (bool?)mObject["ErrorCleared"];
                ErrorDescription = (string)mObject["ErrorDescription"];
                ExtClock = (UInt32?)mObject["ExtClock"];
                Family = (UInt16?)mObject["Family"];
                InstallDate = (DateTime?)mObject["InstallDate"];
                L2CacheSize = (UInt32?)mObject["L2CacheSize"];
                L2CacheSpeed = (UInt32?)mObject["L2CacheSpeed"];
                L3CacheSize = (UInt32?)mObject["L3CacheSize"];
                L3CacheSpeed = (UInt32?)mObject["L3CacheSpeed"];
                LastErrorCode = (UInt32?)mObject["LastErrorCode"];
                Level = (UInt16?)mObject["Level"];
                LoadPercentage = (UInt16?)mObject["LoadPercentage"];
                Manufacturer = (string)mObject["Manufacturer"];
                MaxClockSpeed = (UInt32?)mObject["MaxClockSpeed"];
                Name = (string)mObject["Name"];
                NumberOfCores = (UInt32?)mObject["NumberOfCores"];
                NumberOfEnabledCore = (UInt32?)mObject["NumberOfEnabledCore"];
                NumberOfLogicalProcessors = (UInt32?)mObject["NumberOfLogicalProcessors"];
                OtherFamilyDescription = (string)mObject["OtherFamilyDescription"];
                PartNumber = (string)mObject["PartNumber"];
                PNPDeviceID = (string)mObject["PNPDeviceID"];
                PowerManagementCapabilities=(UInt16[])mObject["PowerManagementCapabilities"];
                PowerManagementSupported = (bool?)mObject["PowerManagementSupported"];
                ProcessorId = (string)mObject["ProcessorId"];
                ProcessorType = (UInt16?)mObject["ProcessorType"];
                Revision = (UInt16?)mObject["Revision"];
                Role = (string)mObject["Role"];
                SecondLevelAddressTranslationExtensions = (bool?)mObject["SecondLevelAddressTranslationExtensions"];
                SerialNumber = (string)mObject["SerialNumber"];
                SocketDesignation = (string)mObject["SocketDesignation"];
                Status = (string)mObject["Status"];
                StatusInfo = (UInt16?)mObject["StatusInfo"];
                Stepping = (string)mObject["Stepping"];
                SystemCreationClassName = (string)mObject["SystemCreationClassName"];
                SystemName = (string)mObject["SystemName"];
                ThreadCount = (UInt32?)mObject["ThreadCount"];
                UniqueId = (string)mObject["UniqueId"];
                UpgradeMethod = (UInt16?)mObject["UpgradeMethod"];
                Version = (string)mObject["Version"];
                VirtualizationFirmwareEnabled = (bool?)mObject["VirtualizationFirmwareEnabled"];
                VMMonitorModeExtensions = (bool?)mObject["VMMonitorModeExtensions"];
                VoltageCaps = (UInt32?)mObject["VoltageCaps"];

            }

        }
    }
}

       








