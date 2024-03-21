using FileHider.Data.Models;
using StegoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHider.Core
{
    public class StegoEngine
    {
        private string _userId;
        public StegoEngine(string userId)
        {
            _userId = userId;
        }

        public void HideMessageInImage(string content, StegoImage stegoImage, ImageStegoStrategy imageStegoStrategy)
        {
            stegoImage.Strategy = imageStegoStrategy.AsStegoStrategy;

            stegoImage.EmbedPayload(content);
        }
    }
}
