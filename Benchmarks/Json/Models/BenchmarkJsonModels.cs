using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Json.Models
{    public class DotnetReleaseInfo
    {
        public string channelversion { get; set; }
        public string latestrelease { get; set; }
        public string latestreleasedate { get; set; }
        public string latestruntime { get; set; }
        public string latestsdk { get; set; }
        public string supportphase { get; set; }
        public string releasetype { get; set; }
        public string eoldate { get; set; }
        public string lifecyclepolicy { get; set; }
        public Release[] releases { get; set; }
    }

    public class Release
    {
        public string releasedate { get; set; }
        public string releaseversion { get; set; }
        public bool security { get; set; }
        public CveList[] cvelist { get; set; }
        public string releasenotes { get; set; }
        public Runtime runtime { get; set; }
        public Sdk sdk { get; set; }
        public Sdk1[] sdks { get; set; }
        public AspnetcoreRuntime aspnetcoreruntime { get; set; }
        public Windowsdesktop windowsdesktop { get; set; }
    }

    public class Runtime
    {
        public string version { get; set; }
        public string versiondisplay { get; set; }
        public string vsversion { get; set; }
        public string vsmacversion { get; set; }
        public BinaryFile[] files { get; set; }
    }

    public class BinaryFile
    {
        public string name { get; set; }
        public string rid { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
    }

    public class Sdk
    {
        public string version { get; set; }
        public string versiondisplay { get; set; }
        public string runtimeversion { get; set; }
        public string vsversion { get; set; }
        public string vsmacversion { get; set; }
        public string vssupport { get; set; }
        public string vsmacsupport { get; set; }
        public string csharpversion { get; set; }
        public string fsharpversion { get; set; }
        public string vbversion { get; set; }
        public File1[] files { get; set; }
    }

    public class File1
    {
        public string name { get; set; }
        public string rid { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
    }

    public class AspnetcoreRuntime
    {
        public string version { get; set; }
        public string versiondisplay { get; set; }
        public string[] versionaspnetcoremodule { get; set; }
        public string vsversion { get; set; }
        public File2[] files { get; set; }
    }

    public class File2
    {
        public string name { get; set; }
        public string rid { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
        public string akams { get; set; }
    }

    public class Windowsdesktop
    {
        public string version { get; set; }
        public string versiondisplay { get; set; }
        public File3[] files { get; set; }
    }

    public class File3
    {
        public string name { get; set; }
        public string rid { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
    }

    public class CveList
    {
        public string cveid { get; set; }
        public string cveurl { get; set; }
    }

    public class Sdk1
    {
        public string version { get; set; }
        public string versiondisplay { get; set; }
        public string runtimeversion { get; set; }
        public string vsversion { get; set; }
        public string vsmacversion { get; set; }
        public string vssupport { get; set; }
        public string vsmacsupport { get; set; }
        public string csharpversion { get; set; }
        public string fsharpversion { get; set; }
        public string vbversion { get; set; }
        public File4[] files { get; set; }
    }

    public class File4
    {
        public string name { get; set; }
        public string rid { get; set; }
        public string url { get; set; }
        public string hash { get; set; }
    }

}
