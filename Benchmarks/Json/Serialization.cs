using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using Benchmarks.Json.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Runtime = Benchmarks.Json.Models.Runtime;

namespace Benchmarks.Json
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class Serialization
    {
        // data via https://github.com/dotnet/core/blob/main/release-notes/8.0/releases.json
        // https://github.com/richlander/convenience/blob/main/releasejson/releasejson/JsonSerializerBenchmark.cs

        private string jsonStringContent;

        private byte[] jsonByteContent;

        [GlobalSetup]
        public void Setup()
        {
            jsonStringContent = File.ReadAllText("Json/BenchmarkJson.json", System.Text.Encoding.UTF8);
            jsonByteContent = File.ReadAllBytes("Json/BenchmarkJson.json");
        }

        [Benchmark]
        public DotnetReleaseInfo Deserialize_SystemTextJson()
        {
            // Deserialize the JSON content using System.Text.Json
            return JsonSerializer.Deserialize<DotnetReleaseInfo>(jsonStringContent);
        }

        [Benchmark]
        public DotnetReleaseInfo Deserialize_SystemTextJson_Byte()
        {
            // Deserialize the JSON content using System.Text.Json
            return JsonSerializer.Deserialize<DotnetReleaseInfo>(jsonByteContent);
        }

        [Benchmark]
        public DotnetReleaseInfo Deserialize_JsonNode()
        {
            JsonNode jsonNode = JsonNode.Parse(jsonByteContent);
            return jsonNode.Deserialize<DotnetReleaseInfo>();
        }

        // only for working with specific bits of a big json file
        [Benchmark]
        public DotnetReleaseInfo Deserialize_Utf8JsonReader()
        {
            var reader = new Utf8JsonReader(jsonByteContent);
            var releaseInfo = new DotnetReleaseInfo();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "channel-version":
                            releaseInfo.channelversion = reader.GetString();
                            break;
                        case "latest-release":
                            releaseInfo.latestrelease = reader.GetString();
                            break;
                        case "latest-release-date":
                            releaseInfo.latestreleasedate = reader.GetString();
                            break;
                        case "latest-runtime":
                            releaseInfo.latestruntime = reader.GetString();
                            break;
                        case "latest-sdk":
                            releaseInfo.latestsdk = reader.GetString();
                            break;
                        case "support-phase":
                            releaseInfo.supportphase = reader.GetString();
                            break;
                        case "release-type":
                            releaseInfo.releasetype = reader.GetString();
                            break;
                        case "eol-date":
                            releaseInfo.eoldate = reader.GetString();
                            break;
                        case "lifecycle-policy":
                            releaseInfo.lifecyclepolicy = reader.GetString();
                            break;
                        case "releases":
                            releaseInfo.releases = ReadReleases(ref reader);
                            break;
                    }
                }
            }

            return releaseInfo;

        }
        private Release[] ReadReleases(ref Utf8JsonReader reader)
        {
            var releases = new List<Release>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var release = new Release();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "release-date":
                                    release.releasedate = reader.GetString();
                                    break;
                                case "release-version":
                                    release.releaseversion = reader.GetString();
                                    break;
                                case "security":
                                    release.security = reader.GetBoolean();
                                    break;
                                case "cve-list":
                                    release.cvelist = ReadCveList(ref reader);
                                    break;
                                case "release-notes":
                                    release.releasenotes = reader.GetString();
                                    break;
                                case "runtime":
                                    release.runtime = ReadRuntime(ref reader);
                                    break;
                                case "sdk":
                                    release.sdk = ReadSdk(ref reader);
                                    break;
                                case "sdks":
                                    release.sdks = ReadSdks(ref reader);
                                    break;
                                case "aspnetcore-runtime":
                                    release.aspnetcoreruntime = ReadAspNetCoreRuntime(ref reader);
                                    break;
                                case "windowsdesktop":
                                    release.windowsdesktop = ReadWindowsDesktop(ref reader);
                                    break;
                            }
                        }
                    }

                    releases.Add(release);
                }
            }

            return releases.ToArray();
        }

        private CveList[] ReadCveList(ref Utf8JsonReader reader)
        {
            var cveList = new List<CveList>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var cve = new CveList();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "cve-id":
                                    cve.cveid = reader.GetString();
                                    break;
                                case "cve-url":
                                    cve.cveurl = reader.GetString();
                                    break;
                            }
                        }
                    }

                    cveList.Add(cve);
                }
            }

            return cveList.ToArray();
        }

        private Runtime ReadRuntime(ref Utf8JsonReader reader)
        {
            var runtime = new Runtime();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "version":
                            runtime.version = reader.GetString();
                            break;
                        case "version-display":
                            runtime.versiondisplay = reader.GetString();
                            break;
                        case "vs-version":
                            runtime.vsversion = reader.GetString();
                            break;
                        case "vs-mac-version":
                            runtime.vsmacversion = reader.GetString();
                            break;
                        case "files":
                            runtime.files = ReadFiles(ref reader);
                            break;
                    }
                }
            }

            return runtime;
        }

        private Sdk ReadSdk(ref Utf8JsonReader reader)
        {
            var sdk = new Sdk();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "version":
                            sdk.version = reader.GetString();
                            break;
                        case "version-display":
                            sdk.versiondisplay = reader.GetString();
                            break;
                        case "vs-version":
                            sdk.vsversion = reader.GetString();
                            break;
                        case "vs-mac-version":
                            sdk.vsmacversion = reader.GetString();
                            break;
                        case "files":
                            sdk.files = ReadFiles1(ref reader);
                            break;
                    }
                }
            }

            return sdk;
        }

        private Sdk1[] ReadSdks(ref Utf8JsonReader reader)
        {
            var sdks = new List<Sdk1>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var sdk = new Sdk1();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "version":
                                    sdk.version = reader.GetString();
                                    break;
                                case "version-display":
                                    sdk.versiondisplay = reader.GetString();
                                    break;
                                case "vs-version":
                                    sdk.vsversion = reader.GetString();
                                    break;
                                case "vs-mac-version":
                                    sdk.vsmacversion = reader.GetString();
                                    break;
                                case "files":
                                    sdk.files = ReadFiles4(ref reader);
                                    break;
                            }
                        }
                    }

                    sdks.Add(sdk);
                }
            }

            return sdks.ToArray();
        }

        private Windowsdesktop ReadWindowsDesktop(ref Utf8JsonReader reader)
        {
            var windowsDesktop = new Windowsdesktop();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "version":
                            windowsDesktop.version = reader.GetString();
                            break;
                        case "version-display":
                            windowsDesktop.versiondisplay = reader.GetString();
                            break;                       
                        case "files":
                            windowsDesktop.files = ReadFiles3(ref reader);
                            break;
                    }
                }
            }

            return windowsDesktop;
        }

        private BinaryFile[] ReadFiles(ref Utf8JsonReader reader)
        {
            var files = new List<BinaryFile>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var file = new BinaryFile();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "name":
                                    file.name = reader.GetString();
                                    break;
                                case "rid":
                                    file.rid = reader.GetString();
                                    break;
                                case "url":
                                    file.url = reader.GetString();
                                    break;
                                case "hash":
                                    file.hash = reader.GetString();
                                    break;
                            }
                        }
                    }

                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        private File1[] ReadFiles1(ref Utf8JsonReader reader)
        {
            var files = new List<File1>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var file = new File1();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "name":
                                    file.name = reader.GetString();
                                    break;
                                case "rid":
                                    file.rid = reader.GetString();
                                    break;
                                case "url":
                                    file.url = reader.GetString();
                                    break;
                                case "hash":
                                    file.hash = reader.GetString();
                                    break;
                            }
                        }
                    }

                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        private File3[] ReadFiles3(ref Utf8JsonReader reader)
        {
            var files = new List<File3>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var file = new File3();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "name":
                                    file.name = reader.GetString();
                                    break;
                                case "rid":
                                    file.rid = reader.GetString();
                                    break;
                                case "url":
                                    file.url = reader.GetString();
                                    break;
                                case "hash":
                                    file.hash = reader.GetString();
                                    break;
                            }
                        }
                    }

                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        private File4[] ReadFiles4(ref Utf8JsonReader reader)
        {
            var files = new List<File4>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var file = new File4();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "name":
                                    file.name = reader.GetString();
                                    break;
                                case "rid":
                                    file.rid = reader.GetString();
                                    break;
                                case "url":
                                    file.url = reader.GetString();
                                    break;
                                case "hash":
                                    file.hash = reader.GetString();
                                    break;
                            }
                        }
                    }

                    files.Add(file);
                }
            }

            return files.ToArray();
        }

        private AspnetcoreRuntime ReadAspNetCoreRuntime(ref Utf8JsonReader reader)
        {
            var aspnetcoreRuntime = new AspnetcoreRuntime();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "version":
                            aspnetcoreRuntime.version = reader.GetString();
                            break;
                        case "version-display":
                            aspnetcoreRuntime.versiondisplay = reader.GetString();
                            break;
                        case "vs-version":
                            aspnetcoreRuntime.vsversion = reader.GetString();
                            break;
                        case "version-aspnetcoremodule":
                            aspnetcoreRuntime.versionaspnetcoremodule = ReadVersionAspNetCoreModule(ref reader);
                            break;
                        case "files":
                            aspnetcoreRuntime.files = ReadFiles2(ref reader);
                            break;
                    }
                }
            }

            return aspnetcoreRuntime;
        }

        private string[] ReadVersionAspNetCoreModule(ref Utf8JsonReader reader)
        {
            var versions = new List<string>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    versions.Add(reader.GetString());
                }
            }

            return versions.ToArray();
        }

        private File2[] ReadFiles2(ref Utf8JsonReader reader)
        {
            var files = new List<File2>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var file = new File2();

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            switch (propertyName)
                            {
                                case "name":
                                    file.name = reader.GetString();
                                    break;
                                case "rid":
                                    file.rid = reader.GetString();
                                    break;
                                case "url":
                                    file.url = reader.GetString();
                                    break;
                                case "hash":
                                    file.hash = reader.GetString();
                                    break;
                            }
                        }
                    }

                    files.Add(file);
                }
            }

            return files.ToArray();
        }
    }
}
