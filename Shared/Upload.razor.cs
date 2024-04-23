﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.Shared
{
    public partial class Upload : ComponentBase, IDisposable
    {
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public List<FileDataModel> Files { get; set; } = new();

        [Parameter]
        public string Filter { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public bool AllowMulitple { get; set; }

        [Parameter]
        public Action Uploaded { get; set; }

        // Define a delegate for the custom event
        public delegate void DismissBadgeHandler();

        // Define the custom event
        public event DismissBadgeHandler DismissBadge;
        public bool Busy { get; set; } = false;

        async Task UploadFile()
        {
            Busy = true;
            List<FileDataModel> results = new List<FileDataModel>();
            try
            {
                var result = await JSRuntime.InvokeAsync<object>("blazorExtensions.GetFileData", "Xinputfile00");
                if (result is not null)
                {
                    FileData[] files = Newtonsoft.Json.JsonConvert.DeserializeObject<FileData[]>(result.ToString());
                    foreach (var file in files)
                    {
                        byte[] compressedbytes;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                            {
                                gzipStream.Write(file.Bytes, 0, file.Bytes.Length);
                            }
                            compressedbytes = memoryStream.ToArray();
                        }

                        results.Add(new FileDataModel
                        {
                            FileName = file.FileName,
                            Base64 = Convert.ToBase64String(compressedbytes),
                            MIMEType = file.MIMEType,
                            Bytes = compressedbytes
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

            this.Files = results.ToList();
            if (Uploaded != null)
            {
                Uploaded();
            }

            Busy = false;
        }

        async Task RemoveItem(FileDataModel item)
        {
            Files.Remove(item);
            DismissBadge.Invoke();
        }

        async Task ClickUpload()
        {
            await JSRuntime.InvokeVoidAsync("blazorExtensions.InvokeClick", "Xinputfile00");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}